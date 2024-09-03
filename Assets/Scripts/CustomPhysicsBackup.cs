/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomPhysics : MonoBehaviour {
    [SerializeField] BoxCollider2D[] boxColliders;
    [SerializeField] ContactFilter2D contactFilter2D;
    [SerializeField] CustomGravityObject customGravityObject;
    [SerializeField] float screwUnityExtentCheckMagicNumber = 0.0025f;
    [SerializeField] bool immovable = false;

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
    void VerticalMovemement(Vector2 movement, ref Vector2 finalPosition) {

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
                    if (immovable) { return; }

                    Debug.Log("Physics update. Me " + gameObject.name + " is resolving vertical clipping");

                    if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                        finalPosition.x = (hit.point + hit.normal * (boxCollider.bounds.extents.x + skinThickness)).x;
                    }
                    else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {

                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;
                    }

                    transform.position = finalPosition; //if clipping, directly move. don't interpolate using rb.move
                    rb.position = finalPosition;

                    break;
                }


                finalPosition.y = (hit.point + (-boxcastDirection * (skinThickness + (boxCollider.bounds.extents.y)))).y;
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
    }

    void HorizontalMovemement(Vector2 movement, ref Vector2 finalPosition) {


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
                    if (immovable) { return; }
                    Debug.Log("Physics update. Me," + gameObject.name + "is resolving horizontal clipping");

                    if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                        finalPosition.x = (hit.point + hit.normal * (boxCollider.bounds.extents.x + skinThickness)).x;
                    }
                    else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {
                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;
                    }

                    transform.position = finalPosition;  //if clipping, directly move. don't interpolate using rb.move
                    rb.position = finalPosition;
                    break;
                }

                finalPosition.x = (hit.point + (-boxcastDirection * (skinThickness + (boxCollider.bounds.extents.x)))).x;
                velocity = new Vector2(customGravityObject.GetGravityDirection().x, velocity.y);

                //set gorunded
                if (customGravityObject.GetGravityDirection().x == Mathf.Sign(movement.x)) { isGrounded = true; }

                ///////////////////IMPORTANT///////////////////////////
                ///FOR NOW, I'LL CHECK ONLY FOR A SINGLE HIT, WHICH WOULD BE THE FIRST THING WE HIT. SO IF WE FIX THIS FOR BOX COLLIDERS IN PARTICULAR, IT'LL AUTOMATICALLY ALSO NOT HIT OTHER HITS
                break;

            }



            if (numberOfOtherHits == 0) { //if this collider's boxcast doesn't hit anything
                finalPosition = new Vector2(rb.position.x + movement.x, finalPosition.y);
            }

        }
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
*//**/