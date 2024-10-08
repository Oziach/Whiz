using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//this code is exquisitely bad.
public class SlimePatrol : MonoBehaviour
{
    [SerializeField] Slime slime;
    [SerializeField] float groundBufferDistance;
    [SerializeField] float moveBufferDistance;
    [SerializeField] BoxCollider2D mainCollider;
    [SerializeField] CustomPhysics rb;
    [SerializeField] CustomGravityObject customGravityObject;
    [SerializeField] LayerMask groundMask;
    float skinThickness;

    private void FixedUpdate() {

    }

    private void Start() {
        skinThickness = rb.getSkinThickness();
    }

    public void HandleVelocityFlipping() {

        Vector2 movement = Time.fixedDeltaTime * rb.velocity;
        Vector2 gravityDirection = customGravityObject.GetGravityDirection();

        //if gravity vertical, check horizontally
        if (gravityDirection.x == 0) {

            Vector2 boxcastOrigin = transform.position;
            Vector2 boxSize = mainCollider.bounds.size;
            Vector2 boxcastDirection = Vector2.right * Mathf.Sign(movement.x);
            float boxcastDistance = movement.x + moveBufferDistance + skinThickness ;

            bool canMoveInVelDir = true;
            //first raycast
            RaycastHit2D hit = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
            if (hit) { //check if we can move in current vel direction.
                canMoveInVelDir = false;
            }
            else {

                float movementSign = Mathf.Sign(movement.x);
                Vector2 raycastOrigin = transform.position;
                raycastOrigin += new Vector2(movementSign * (mainCollider.bounds.size.x / 2 + Mathf.Abs(movement.x) + moveBufferDistance), gravityDirection.y * mainCollider.bounds.size.y / 2);
                Vector2 raycastDirection = gravityDirection;
                float raycastDistance = skinThickness + groundBufferDistance;
                Debug.Log("Can move in direction no wall: " + boxcastDirection);
      

                //else, perform a second boxcast to check we onl move if we remain grounded
  
                Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.red);
                bool checkedForSmallGaps = false;
                while (true) {

                    if (checkedForSmallGaps) { raycastOrigin.x += movementSign * moveBufferDistance; }

                    //RaycastHit2D hit2 = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
                    RaycastHit2D hit2 = Physics2D.Raycast(raycastOrigin, raycastDirection, raycastDistance, groundMask);

                    if (!hit2) { canMoveInVelDir = false;}
                    else { canMoveInVelDir = true; }

                    if (checkedForSmallGaps) { break; } else { checkedForSmallGaps = true; }


                }



            }

            if (canMoveInVelDir) { return; }


            //now do the same for the other direction

            boxcastOrigin = transform.position;
            boxSize = mainCollider.bounds.size;
            boxcastDirection = Vector2.right * -Mathf.Sign(movement.x);
            boxcastDistance = movement.x + moveBufferDistance + skinThickness;

            bool canMoveAgainstVelDir = true;

            //first raycast
            hit = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
            if (hit) { //check if we can move in current vel direction.
                Debug.Log(hit.transform.gameObject.name);
                canMoveAgainstVelDir = false;
            }
            else {

                float movementSign = -Mathf.Sign(movement.x);
                Vector2 raycastOrigin = transform.position;
                raycastOrigin += new Vector2(movementSign * (mainCollider.bounds.size.x / 2 + Mathf.Abs(movement.x) + moveBufferDistance), gravityDirection.y * mainCollider.bounds.size.y / 2);
                Vector2 raycastDirection = gravityDirection;
                float raycastDistance = skinThickness + groundBufferDistance;

                Debug.DrawRay(raycastOrigin, gravityDirection, Color.red);
                Debug.Log("Can move in direction no wall: " + boxcastDirection);

                //else, perform a second boxcast to check we onl move if we remain grounded

                bool checkedForSmallGaps = false;
                while (true) {

                    if (checkedForSmallGaps) { raycastOrigin.x += movementSign * moveBufferDistance; }
                    //RaycastHit2D hit2 = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
                    RaycastHit2D hit2 = Physics2D.Raycast(raycastOrigin, raycastDirection, raycastDistance, groundMask);

                    if (!hit2) { canMoveAgainstVelDir = false; }
                    else { canMoveAgainstVelDir = true; }

                    if (checkedForSmallGaps) { break; } else { checkedForSmallGaps = true; }


                }
            }

            if (canMoveAgainstVelDir) { rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y); }

            if(!canMoveInVelDir && !canMoveAgainstVelDir) {

                if (gameObject.GetComponent<ShutterSegment>()) { Debug.Log("Set to zero"); }
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }

        }
        else { //if gravity horizontal, check vertically

            Vector2 boxcastOrigin = transform.position;
            Vector2 boxSize = mainCollider.bounds.size;
            Vector2 boxcastDirection = Vector2.up * Mathf.Sign(movement.y);
            float boxcastDistance = movement.y + moveBufferDistance + skinThickness;

            bool canMoveInVelDir = true;
            //first raycast
            RaycastHit2D hit = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
            if (hit) { //check if we can move in current vel direction.
                canMoveInVelDir = false;
            }
            else {
                float movementSign = Mathf.Sign(movement.y);
                Vector2 raycastOrigin = transform.position;
                raycastOrigin += new Vector2(gravityDirection.x * mainCollider.bounds.size.x / 2, movementSign * (mainCollider.bounds.size.y / 2 + Mathf.Abs(movement.y) + moveBufferDistance));
                Vector2 raycastDirection = gravityDirection;
                float raycastDistance = skinThickness + groundBufferDistance;

                Debug.DrawRay(raycastOrigin, gravityDirection * raycastDistance, Color.red);

                Debug.Log("Can move in direction no wall: " + boxcastDirection);

                //else, perform a second boxcast to check we onl move if we remain grounded

                bool checkedForSmallGaps = false;
                while (true) {

                    if (checkedForSmallGaps) { raycastOrigin.y += movementSign * moveBufferDistance; }

                    //RaycastHit2D hit2 = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
                    RaycastHit2D hit2 = Physics2D.Raycast(raycastOrigin, raycastDirection, raycastDistance, groundMask);


                    if (!hit2) { canMoveInVelDir = false; }
                    else { canMoveInVelDir = true; }

                    if (checkedForSmallGaps) { break; } else { checkedForSmallGaps = true; }


                }
            }

            if (canMoveInVelDir) { return; }

            //now do the same for the other direction

            boxcastOrigin = transform.position;
            boxSize = mainCollider.bounds.size;
            boxcastDirection = Vector2.up * -Mathf.Sign(movement.y);
            boxcastDistance = movement.y + moveBufferDistance;

            bool canMoveAgainstVelDir = true;

            //first raycast
            hit = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
            if (hit) { //check if we can move in current vel direction.
                canMoveAgainstVelDir = false;
            }
            else {

                float movementSign = -Mathf.Sign(movement.y);
                Vector2 raycastOrigin = transform.position;
                raycastOrigin += new Vector2(gravityDirection.x * mainCollider.bounds.size.x / 2, movementSign * (mainCollider.bounds.size.y / 2 + Mathf.Abs(movement.y) + moveBufferDistance));
                Vector2 raycastDirection = gravityDirection;
                float raycastDistance = skinThickness + groundBufferDistance;

                Debug.Log("Can move in direction no wall: " + boxcastDirection);


                bool checkedForSmallGaps = false;
                while (true) {

                    if (checkedForSmallGaps) { raycastOrigin.y += movementSign * moveBufferDistance; }

                    //RaycastHit2D hit2 = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
                    RaycastHit2D hit2 = Physics2D.Raycast(raycastOrigin, raycastDirection, raycastDistance, groundMask);


                    if (!hit2) { canMoveAgainstVelDir = false; }

                    else { canMoveAgainstVelDir = true; }

                    if (checkedForSmallGaps) { break; } else { checkedForSmallGaps = true; }


                }
            }

            if (canMoveAgainstVelDir) { rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y); }

            if (!canMoveInVelDir && !canMoveAgainstVelDir) { rb.velocity = new Vector2(rb.velocity.x, 0f); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
