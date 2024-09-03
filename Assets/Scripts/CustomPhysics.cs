using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomPhysics : MonoBehaviour {
    [SerializeField] BoxCollider2D[] boxColliders;
    [SerializeField] ContactFilter2D contactFilter2D;
    [SerializeField] CustomGravityObject customGravityObject;
    [SerializeField] float screwUnityExtentCheckMagicNumber = 0.0025f;
    [SerializeField] float stepHeight = 0.01f;
    [SerializeField] bool immovable = false;
    [SerializeField] float flushResolutionDist = 0.001f;
    [SerializeField] float movementHappenedThresholdDist = 0.0001f;
    Rigidbody2D rb; //required component
    public bool isGrounded; //exposed for debugging
    private float skinThickness;

    public Vector2 velocity;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

    }

    private void FixedUpdate() {

        Vector2 movement = velocity * Time.fixedDeltaTime;
        isGrounded = false;

        Vector2 finalPosition = rb.position;

        HorizontalMovemement(movement, ref finalPosition);
        VerticalMovemement(movement, ref finalPosition);

        rb.MovePosition(finalPosition);//


    }

    //horizontal movement

    //vertical movement
    bool VerticalMovemement(Vector2 movement, ref Vector2 finalPosition) {

        Vector2 gravityDirection = customGravityObject.GetGravityDirection();

        foreach (BoxCollider2D boxCollider in boxColliders) {
            if (!boxCollider.gameObject.activeInHierarchy) { continue; }

            //do calculations for the first collider you find. then just break. it is assumed that only one collider is active at a time.
            Vector2 boxSize = boxCollider.bounds.size;
            Vector2 boxcastOrigin = finalPosition;
            Vector2 boxcastDirection = movement.y == 0f ? Vector2.zero : Vector2.up * Mathf.Sign(movement.y);

            float boxcastDistance = movement.y == 0f ? 0f : Mathf.Abs(movement.y) + skinThickness;

            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            int numberOfHits = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, contactFilter2D, hits, boxcastDistance);
            int numberOfOtherHits = 0; //if only self hit, then this will remain zero

  
            var hitRay = Physics2D.Raycast(new Vector2(finalPosition.x, finalPosition.y - boxCollider.bounds.size.y / 2), Vector2.down, 1f, contactFilter2D.layerMask);

            foreach (RaycastHit2D hit in hits) {

                if (hit.collider.gameObject == boxCollider.gameObject) { continue; }

                Vector2 minBounds = finalPosition - (Vector2)boxCollider.bounds.size / 2;
                Vector2 maxBounds = finalPosition + (Vector2)boxCollider.bounds.size / 2;
/*
                if (hit.point.x + screwUnityExtentCheckMagicNumber < minBounds.x || hit.point.x - screwUnityExtentCheckMagicNumber > maxBounds.x) {
                   // Debug.Log("My data: " + gameObject.name + "My  min and max x points:" + boxCollider.bounds.min.x + " " + boxCollider.bounds.max.x + "Other collider's data - name" +
                     //   hit.collider.gameObject.name + " HITpoint x: " +
                      // hit.point.x);
                    continue;
                }*/

                numberOfOtherHits++;

                //Debug.Log("Hit point: " + hit.point.x + " Min bound: " + boxCollider.bounds.min.x);
               
                if (hit.distance <= 0f) {
                    if (immovable) { return false; }
                    Debug.DrawRay(hit.point, hit.normal, Color.yellow);
                    Debug.Log("Physics update. Me " + gameObject.name + " is resolving vertical clipping with:" + hit.collider.gameObject.name);

                    if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;
                        //finalPosition = (hit.point + hit.normal.normalized * (boxCollider.bounds.extents));
                        //finalPosition += hit.normal.normalized * skinThickness;
                    }
                    else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {

                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;
                    }

                    //transform.position = finalPosition; //if clipping, directly move. don't interpolate using rb.move
                    //rb.position = finalPosition;

                    return false;
                }

                //stepping stufffffffffffff
                //not clipping
                //if stepping can potentiall resolve the conflict, then try stepping in the direction opposite to gravity
                Vector2 newFinalPos = finalPosition;
                if (gravityDirection.y == 0) { //gravity direciton must obviousl be perpendicular to movement to count as a 'step'
                    //find the bound of the other collider against direction of grravity. Have to do this using raycast since all ground is a single collider
                    float raycastOriginY = hit.point.y + Mathf.Sign(movement.y) * (skinThickness + flushResolutionDist);
                    float raycastOriginX = finalPosition.x + gravityDirection.x * (boxCollider.bounds.size.x / 2 + flushResolutionDist);
                    Vector2 raycastOrigin = new Vector2(raycastOriginX, raycastOriginY);
                    Debug.DrawRay(raycastOrigin, -gravityDirection / 2, Color.white);
                    RaycastHit2D rayHit = Physics2D.Raycast(raycastOrigin, -gravityDirection, stepHeight);

                    if (rayHit && hit.collider.CompareTag("Ground")) { //since ground's extentes need to be manually calculated
                        Debug.Log(hit.point.x + " My extent: " + (finalPosition.x + gravityDirection.x * (boxCollider.bounds.size.x / 2)));
                        if ((gravityDirection == Vector2.right && finalPosition.x + boxCollider.bounds.size.x / 2 - stepHeight < hit.point.x)
                            || (gravityDirection == Vector2.left && finalPosition.x - boxCollider.bounds.size.x / 2 + stepHeight > hit.point.x)) {

                            Vector2 tempPos = newFinalPos;
                            Debug.Log("stepped. other collider extent pos: " + hit.point.x);

                            float stepMovementHorizontal = new Vector2(rayHit.point.x - (finalPosition.x + gravityDirection.x * (boxCollider.bounds.size.x / 2)), 0f).magnitude;

                            if (HorizontalMovemementOnce(-gravityDirection * (stepMovementHorizontal + skinThickness), ref tempPos)) {
                                if (VerticalMovemementOnce(movement, ref tempPos)) { //step goddamn succesfull.
                                    newFinalPos = tempPos;
                                }
                                else {
                                    Debug.Log("not gonna move horizontallyhere");
                                }
                            }
                            else {
                                Debug.Log("not gonna move vertically here");
                            }
                        }

                    }

                }
                //stepping stuf enddddddddddddddddddddddd


                //step bloody succesfull bich
                if (newFinalPos != finalPosition) {
                    Debug.Log(gameObject.name + " old pos: " + finalPosition + "new final pos vertically adjusted: " + newFinalPos);
                    finalPosition = newFinalPos;
                    rb.position = finalPosition;
                    transform.position = finalPosition;
                }
                else { finalPosition.y = (hit.point + (-boxcastDirection * (skinThickness + (boxCollider.bounds.extents.y)))).y; }
                velocity = new Vector2(velocity.x, customGravityObject.GetGravityDirection().y); //have unit velocity in gravity direction to prevent clipping issues at tiny units
                //Debug.DrawRay(new Vector2(finalPosition.x, finalPosition.y - boxCollider.bounds.size.y / 2), hit.point - new Vector2(finalPosition.x, finalPosition.y - boxCollider.bounds.size.y / 2), Color.white);

                //set grounded
                if (customGravityObject.GetGravityDirection().y == Mathf.Sign(movement.y)) { isGrounded = true; }

                ///////////////////IMPORTANT///////////////////////////
                ///FOR NOW, I'LL CHECK ONLY FOR A SINGLE HIT, WHICH WOULD BE THE FIRST THING WE HIT. SO IF WE FIX THIS FOR BOX COLLIDERS IN PARTICULAR, IT'LL AUTOMATICALLY ALSO NOT HIT OTHER HITS
                break;
            }


            if (numberOfOtherHits == 0) {
                finalPosition = new Vector2(finalPosition.x, rb.position.y + movement.y);

            }

        }
        return true;

    }

    bool HorizontalMovemement(Vector2 movement, ref Vector2 finalPosition) {

        Vector2 gravityDirection = customGravityObject.GetGravityDirection();

        foreach (BoxCollider2D boxCollider in boxColliders) {
            if (!boxCollider.gameObject.activeInHierarchy) { continue; }


            Vector2 boxSize = boxCollider.bounds.size;
            Vector2 boxcastOrigin = finalPosition;
            Vector2 boxcastDirection = movement.x == 0f ? Vector2.zero : Vector2.right * Mathf.Sign(movement.x);


            float boxcastDistance = movement.x == 0f ? 0 : Mathf.Abs(movement.x) + skinThickness;


            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            int numberOfHits = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, contactFilter2D, hits, boxcastDistance);
            int numberOfOtherHits = 0; //if only self hit, then this will remain zero

            //Debug.Log(numberOfHits);



            //ahhh i see the problem. you'll have to recheck after fixing for every single hit. my god.
            foreach (RaycastHit2D hit in hits) {


                Vector2 minBounds = finalPosition - (Vector2)boxCollider.bounds.size / 2;
                Vector2 maxBounds = finalPosition + (Vector2)boxCollider.bounds.size / 2;
                if (hit.collider.gameObject == boxCollider.gameObject) { continue; }
        /*        if (hit.point.y + screwUnityExtentCheckMagicNumber < minBounds.y || hit.point.y - screwUnityExtentCheckMagicNumber > maxBounds.y) {
                    continue;
                }*/
                numberOfOtherHits++;

                if (hit.distance <= 0) {
                    if (immovable) { return false; }
                    Debug.Log("Physics update. Me," + gameObject.name + "is resolving horizontal clipping");

                    if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                        finalPosition.x = (hit.point + hit.normal * (boxCollider.bounds.extents.x + skinThickness)).x;
                    }
                    else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {
                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;

                    }
                    //finalPosition = (hit.point + hit.normal * (boxCollider.bounds.extents));
                    //finalPosition += hit.normal.normalized * skinThickness;

                    transform.position = finalPosition;  //if clipping, directly move. don't interpolate using rb.move
                    rb.position = finalPosition;
                    return false;
                }

                //stepping stufffffffffffff
                //not clipping
                //if stepping can potentiall resolve the conflict, then try stepping in the direction opposite to gravity
                Vector2 newFinalPos = finalPosition;
                if (gravityDirection.x == 0) { //gravity direciton must obviousl be perpendicular to movement to count as a 'step'
                    //find the bound of the other collider against direction of grravity. Have to do this using raycast since all ground is a single collider
                    float raycastOriginX = hit.point.x + Mathf.Sign(movement.x) * (Physics2D.defaultContactOffset + flushResolutionDist);
                    float raycastOriginY = finalPosition.y + gravityDirection.y * (boxCollider.bounds.size.y / 2 + flushResolutionDist);
                    Vector2 raycastOrigin = new Vector2(raycastOriginX, raycastOriginY);
                    Debug.DrawRay(raycastOrigin, -gravityDirection / 2, Color.white);
                    RaycastHit2D rayHit = Physics2D.Raycast(raycastOrigin, -gravityDirection, stepHeight);

                    if (rayHit && hit.collider.CompareTag("Ground")) { //since ground's extentes need to be manually calculated
                        Debug.Log(hit.point.y + " My extent: " + (finalPosition.y + gravityDirection.y * (boxCollider.bounds.size.y / 2)));
                        if ((gravityDirection == Vector2.up && finalPosition.y + boxCollider.bounds.size.y / 2 - stepHeight < hit.point.y)
                            || (gravityDirection == Vector2.down && finalPosition.y - boxCollider.bounds.size.y / 2 + stepHeight > hit.point.y)) {

                            Vector2 tempPos = newFinalPos;
                            Debug.Log("stepped. other collider extent pos: " + hit.point.y);

                            float stepMovementVertical = new Vector2(0f, rayHit.point.y - (finalPosition.y + gravityDirection.y * (boxCollider.bounds.size.y / 2))).magnitude;
       
                           if (VerticalMovemementOnce(-gravityDirection * (stepMovementVertical + skinThickness), ref tempPos)) {
                                if (HorizontalMovemementOnce(movement, ref tempPos)) { //step goddamn succesfull.
                                    newFinalPos = tempPos;
                                }
                                else {
                                    Debug.Log("not gonna move horizontallyhere");
                                }
                            }
                            else {
                                Debug.Log("not gonna move vertically here");
                            }
                        }

                    }

                }
                //stepping stuf enddddddddddddddddddddddd

                //step bloody succesfull bich
                if (newFinalPos != finalPosition) {
                    Debug.Log(gameObject.name + " old pos: " + finalPosition + "new final pos vertically adjusted: " + newFinalPos);
                    finalPosition = newFinalPos;
                    rb.position = finalPosition;
                    transform.position = finalPosition;
                }
                else { finalPosition.x = (hit.point + (-boxcastDirection * (skinThickness + (boxCollider.bounds.extents.x)))).x; }
          
                velocity = new Vector2(customGravityObject.GetGravityDirection().x, velocity.y);
    
                //set gorunded
                if (customGravityObject.GetGravityDirection().x == Mathf.Sign(movement.x)) { isGrounded = true; }
                Debug.DrawRay(hit.point, hit.normal, Color.yellow);

                ///////////////////IMPORTANT///////////////////////////
                ///FOR NOW, I'LL CHECK ONLY FOR A SINGLE HIT, WHICH WOULD BE THE FIRST THING WE HIT. SO IF WE FIX THIS FOR BOX COLLIDERS IN PARTICULAR, IT'LL AUTOMATICALLY ALSO NOT HIT OTHER HITS
                break;

            }



            if (numberOfOtherHits == 0) { //if this collider's boxcast doesn't hit anything
                finalPosition = new Vector2(rb.position.x + movement.x, finalPosition.y);
            }


        }

        return true;

    }


    bool HorizontalMovemementOnce(Vector2 movement, ref Vector2 finalPosition) {

        Vector2 gravityDirection = customGravityObject.GetGravityDirection();

        foreach (BoxCollider2D boxCollider in boxColliders) {
            if (!boxCollider.gameObject.activeInHierarchy) { continue; }


            Vector2 boxSize = boxCollider.bounds.size;
            Vector2 boxcastOrigin = finalPosition;
            Vector2 boxcastDirection = movement.x == 0f ? Vector2.zero : Vector2.right * Mathf.Sign(movement.x);


            float boxcastDistance = movement.x == 0f ? 0 : Mathf.Abs(movement.x) + skinThickness;


            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            int numberOfHits = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, contactFilter2D, hits, boxcastDistance);
            int numberOfOtherHits = 0; //if only self hit, then this will remain zero

            //Debug.Log(numberOfHits);



            //ahhh i see the problem. you'll have to recheck after fixing for every single hit. my god.
            foreach (RaycastHit2D hit in hits) {


                Vector2 minBounds = finalPosition - (Vector2)boxCollider.bounds.size / 2;
                Vector2 maxBounds = finalPosition + (Vector2)boxCollider.bounds.size / 2;
                if (hit.collider.gameObject == boxCollider.gameObject) { continue; }
                if (hit.point.y + screwUnityExtentCheckMagicNumber < minBounds.y || hit.point.y - screwUnityExtentCheckMagicNumber > maxBounds.y) {
                    continue;
                }
                numberOfOtherHits++;

                if (hit.distance <= 0) {
                    if (immovable) { return false; }
                    Debug.Log("Physics update. Me," + gameObject.name + "is resolving horizontal clipping with: " + hit.collider.gameObject.name);

                    if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                        finalPosition.x = (hit.point + hit.normal * (boxCollider.bounds.extents.x + skinThickness)).x;
                    }
                    else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {
                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;

                    }
                    //finalPosition = (hit.point + hit.normal * (boxCollider.bounds.extents));
                    //finalPosition += hit.normal.normalized * skinThickness;

                    transform.position = finalPosition;  //if clipping, directly move. don't interpolate using rb.move
                    rb.position = finalPosition;
                    return false;
                }

                Vector2 prevFinalPosition = finalPosition;
                finalPosition.x = (hit.point + (-boxcastDirection * (skinThickness + (boxCollider.bounds.extents.x)))).x;

                velocity = new Vector2(customGravityObject.GetGravityDirection().x, velocity.y);

                //set gorunded
                if (customGravityObject.GetGravityDirection().x == Mathf.Sign(movement.x)) { isGrounded = true; }
                Debug.DrawRay(hit.point, hit.normal, Color.yellow);

                ///////////////////IMPORTANT///////////////////////////
                ///FOR NOW, I'LL CHECK ONLY FOR A SINGLE HIT, WHICH WOULD BE THE FIRST THING WE HIT. SO IF WE FIX THIS FOR BOX COLLIDERS IN PARTICULAR, IT'LL AUTOMATICALLY ALSO NOT HIT OTHER HITS
                return  Mathf.Sign(movement.x) * (finalPosition.x - prevFinalPosition.x) > movementHappenedThresholdDist;

            }



            if (numberOfOtherHits == 0) { //if this collider's boxcast doesn't hit anything
                finalPosition = new Vector2(rb.position.x + movement.x, finalPosition.y);
            }


        }

        return true;

    }

    bool VerticalMovemementOnce(Vector2 movement, ref Vector2 finalPosition) {

        foreach (BoxCollider2D boxCollider in boxColliders) {
            if (!boxCollider.gameObject.activeInHierarchy) { continue; }

            //do calculations for the first collider you find. then just break. it is assumed that only one collider is active at a time.
            Vector2 boxSize = boxCollider.bounds.size;
            Vector2 boxcastOrigin = finalPosition;
            Vector2 boxcastDirection = movement.y == 0f ? Vector2.zero : Vector2.up * Mathf.Sign(movement.y);

            float boxcastDistance = movement.y == 0f ? 0f : Mathf.Abs(movement.y) + skinThickness;

            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            int numberOfHits = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, contactFilter2D, hits, boxcastDistance);
            int numberOfOtherHits = 0; //if only self hit, then this will remain zero


            var hitRay = Physics2D.Raycast(new Vector2(finalPosition.x, finalPosition.y - boxCollider.bounds.size.y / 2), Vector2.down, 1f, contactFilter2D.layerMask);

            foreach (RaycastHit2D hit in hits) {

                if (hit.collider.gameObject == boxCollider.gameObject) { continue; }

                Vector2 minBounds = finalPosition - (Vector2)boxCollider.bounds.size / 2;
                Vector2 maxBounds = finalPosition + (Vector2)boxCollider.bounds.size / 2;

                if (hit.point.x + screwUnityExtentCheckMagicNumber < minBounds.x || hit.point.x - screwUnityExtentCheckMagicNumber > maxBounds.x) {
                    // Debug.Log("My data: " + gameObject.name + "My  min and max x points:" + boxCollider.bounds.min.x + " " + boxCollider.bounds.max.x + "Other collider's data - name" +
                    //   hit.collider.gameObject.name + " HITpoint x: " +
                    // hit.point.x);
                    continue;
                }

                numberOfOtherHits++;

                //Debug.Log("Hit point: " + hit.point.x + " Min bound: " + boxCollider.bounds.min.x);

                if (hit.distance <= 0f) {
                    if (immovable) { return false; }
                    Debug.DrawRay(hit.point, hit.normal, Color.yellow);
                    Debug.Log("Physics update. Me " + gameObject.name + " is resolving vertical clipping");

                    if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;
                        //finalPosition = (hit.point + hit.normal.normalized * (boxCollider.bounds.extents));
                        //finalPosition += hit.normal.normalized * skinThickness;
                    }
                    else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {

                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;
                    }

                    //transform.position = finalPosition; //if clipping, directly move. don't interpolate using rb.move
                    //rb.position = finalPosition;`
                    return false;
                }

                Vector2 prevFinalPosition = finalPosition;

                finalPosition.y = (hit.point + (-boxcastDirection * (skinThickness + (boxCollider.bounds.extents.y)))).y;
                velocity = new Vector2(velocity.x, customGravityObject.GetGravityDirection().y); //have unit velocity in gravity direction to prevent clipping issues at tiny units
                //Debug.DrawRay(new Vector2(finalPosition.x, finalPosition.y - boxCollider.bounds.size.y / 2), hit.point - new Vector2(finalPosition.x, finalPosition.y - boxCollider.bounds.size.y / 2), Color.white);

                //set grounded
                if (customGravityObject.GetGravityDirection().y == Mathf.Sign(movement.y)) { isGrounded = true; }

                ///////////////////IMPORTANT///////////////////////////
                ///FOR NOW, I'LL CHECK ONLY FOR A SINGLE HIT, WHICH WOULD BE THE FIRST THING WE HIT. SO IF WE FIX THIS FOR BOX COLLIDERS IN PARTICULAR, IT'LL AUTOMATICALLY ALSO NOT HIT OTHER HITS
                Debug.Log("final position" + finalPosition.y + " prev position" + prevFinalPosition.y);
                return  Mathf.Sign(movement.y) * (finalPosition.y - prevFinalPosition.y) > movementHappenedThresholdDist;

            }


            if (numberOfOtherHits == 0) {
                finalPosition = new Vector2(finalPosition.x, rb.position.y + movement.y);

            }

        }
        return true;

    }

    public bool IsGrounded() {
        return isGrounded;
    }

    public float getSkinThickness() {
        return skinThickness;
    }

    // Start is called before the first frame update
    void Start() {
        skinThickness = Physics2D.defaultContactOffset;
    }

    // Update is called once per frame
    void Update() {
    }

}
