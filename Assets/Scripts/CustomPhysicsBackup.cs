/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomPhysics : MonoBehaviour {
    [SerializeField] BoxCollider2D[] boxColliders;
    [SerializeField] ContactFilter2D contactFilter2D;
    Rigidbody2D rb; //required component

    public Vector2 velocity;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

    }

    private void FixedUpdate() {
        Vector2 movement = velocity * Time.fixedDeltaTime;

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

            float boxcastDistance = movement.y == 0f ? 0f : Mathf.Abs(movement.y) + Physics2D.defaultContactOffset;

            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            int numberOfHits = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, contactFilter2D, hits, boxcastDistance);

            if (numberOfHits > 1) { //1 means only self hit
                foreach (RaycastHit2D hit in hits) {

                    if (hit.collider.gameObject == boxCollider.gameObject) { continue; }

                    if (hit.distance <= 0f) {

                        if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                            Debug.Log("fixing");
                            finalPosition.x = (hit.point + hit.normal * (boxCollider.bounds.extents.x + Physics2D.defaultContactOffset)).x;
                        }
                        else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {
                            Debug.Log("fixing");

                            finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + Physics2D.defaultContactOffset)).y;
                        }

                        continue;
                    }

                    finalPosition.y = (hit.point + (-boxcastDirection * (Physics2D.defaultContactOffset + (boxCollider.bounds.extents.y)))).y;
                    velocity = new Vector2(velocity.x, 0f);
                }
            }
            else {
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

            float boxcastDistance = movement.x == 0f ? 0 : Mathf.Abs(movement.x) + Physics2D.defaultContactOffset;


            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            int numberOfHits = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, contactFilter2D, hits, boxcastDistance);

            //Debug.Log(numberOfHits);
            if (numberOfHits > 1) { //1 means only self hit

                foreach (RaycastHit2D hit in hits) {

                    if (hit.collider.gameObject == boxCollider.gameObject) { continue; }

                    if (hit.distance <= 0) {



                        if (Mathf.Abs(hit.normal.x) > 0.99f && Mathf.Abs(hit.normal.y) < 0.01f) {
                            Debug.Log("fixing");
                            finalPosition.x = (hit.point + hit.normal * (boxCollider.bounds.extents.x + Physics2D.defaultContactOffset)).x;
                        }
                        else if (Mathf.Abs(hit.normal.x) < 0.1f && Mathf.Abs(hit.normal.y) > 0.99f) {
                            Debug.Log("fixing");

                            finalPosition.y = (hit.point + hit.normal * (boxCollider.bounds.extents.y + Physics2D.defaultContactOffset)).y;
                        }

                        continue;
                    }

                    finalPosition.x = (hit.point + (-boxcastDirection * (Physics2D.defaultContactOffset + (boxCollider.bounds.extents.x)))).x;
                    velocity = new Vector2(0f, velocity.y);
                }
            }


            else {
                finalPosition = new Vector2(rb.position.x + movement.x, finalPosition.y);
            }

        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
    }

}
*//**/