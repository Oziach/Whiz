/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomPhysics : MonoBehaviour {
    [SerializeField] BoxCollider2D[] boxColliders;
    [SerializeField] ContactFilter2D contactFilter2D;
    [SerializeField] CustomGravityObject customGravityObject;


    Rigidbody2D rb; //required component
    public bool isGrounded; //exposed for debugging
    [SerializeField] private float skinThickness = 0.01f;

    public Vector2 velocity;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

    }

    private void FixedUpdate() {
        Vector2 movement = velocity * Time.fixedDeltaTime;
        isGrounded = false;

        Vector2 finalPosition = rb.position;

        VerticalMovemement(movement, ref finalPosition);
        HorizontalMovemement(movement, ref finalPosition);
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


            foreach (RaycastHit2D hit in hits) {

                if (hit.collider.gameObject == boxCollider.gameObject) { continue; }
                numberOfOtherHits++;

                if (hit.distance <= 0f) {
                    Debug.Log("Physics update. Me " + gameObject.name + " is resolving vertical clipping");

                    if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                        finalPosition.x = (hit.point + hit.normal * (boxCollider.bounds.extents.x + skinThickness)).x;
                    }
                    else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {

                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;
                    }

                    transform.position = finalPosition; //if clipping, directly move. don't interpolate using rb.move
                    rb.position = finalPosition;
                    continue;
                }

                finalPosition.y = (hit.point + (-boxcastDirection * (skinThickness + (boxCollider.bounds.extents.y)))).y;
                velocity = new Vector2(velocity.x, customGravityObject.GetGravityDirection().y); //have unit velocity in gravity direction to prevent clipping issues at tiny units

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

                if (hit.collider.gameObject == boxCollider.gameObject) { continue; }
                numberOfOtherHits++;

                if (hit.distance <= 0) {
                    Debug.Log("Physics update. Me," + gameObject.name + "is resolving horizontal clipping");

                    if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                        finalPosition.x = (hit.point + hit.normal * (boxCollider.bounds.extents.x + skinThickness)).x;
                    }
                    else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {
                        finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + skinThickness)).y;
                    }

                    transform.position = finalPosition;  //if clipping, directly move. don't interpolate using rb.move
                    rb.position = finalPosition;
                    continue;
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

    }

    // Update is called once per frame
    void Update() {
    }

}
*/