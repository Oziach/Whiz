using System.Collections;
using System.Collections.Generic;
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

    private void FixedUpdate() {

    }

    public void HandleVelocityFlipping() {

        Vector2 movement = Time.fixedDeltaTime * rb.velocity;
        Vector2 gravityDirection = customGravityObject.GetGravityDirection();

        //if gravity vertical, check horizontally
        if (gravityDirection.x == 0) {

            Vector2 boxcastOrigin = transform.position;
            Vector2 boxSize = mainCollider.bounds.size;
            Vector2 boxcastDirection = Vector2.right * Mathf.Sign(movement.x);
            float boxcastDistance = movement.x + moveBufferDistance;

            bool canMoveInVelDir = true;
            //first raycast
            RaycastHit2D hit = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
            if (hit) { //check if we can move in current vel direction.
                canMoveInVelDir = false;
            }
            else {
                //else, perform a second boxcast to check we onl move if we remain grounded
                boxcastOrigin.x += movement.x + (Mathf.Sign(movement.x) * (moveBufferDistance + mainCollider.bounds.size.x));
                boxcastDirection = gravityDirection;
                boxcastDistance = Physics2D.defaultContactOffset + groundBufferDistance;
                RaycastHit2D hit2 = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);

                if (!hit2) { canMoveInVelDir = false; }

            }

            if (canMoveInVelDir) { return; }


            //now do the same for the other direction

            boxcastOrigin = transform.position;
            boxSize = mainCollider.bounds.size;
            boxcastDirection = Vector2.right * -Mathf.Sign(movement.x);
            boxcastDistance = movement.x + moveBufferDistance;

            bool canMoveAgainstVelDir = true;

            //first raycast
            hit = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
            if (hit) { //check if we can move in current vel direction.
                Debug.Log(hit.transform.gameObject.name);
                canMoveAgainstVelDir = false;
            }
            else {
                //else, perform a second boxcast to check we onl move if we remain grounded
                boxcastOrigin.x -= movement.x + (Mathf.Sign(movement.x) * (moveBufferDistance+ mainCollider.bounds.size.x));
                boxcastDirection = gravityDirection;
                boxcastDistance = Physics2D.defaultContactOffset + groundBufferDistance;
                RaycastHit2D hit2 = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);

                if (!hit2) { canMoveAgainstVelDir = false; }

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
            float boxcastDistance = movement.y + moveBufferDistance;

            bool canMoveInVelDir = true;
            //first raycast
            RaycastHit2D hit = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);
            if (hit) { //check if we can move in current vel direction.
                canMoveInVelDir = false;
            }
            else {
                //else, perform a second boxcast to check we onl move if we remain grounded
                boxcastOrigin.y += movement.y + (Mathf.Sign(movement.y) * (moveBufferDistance + mainCollider.bounds.size.y));
                boxcastDirection = gravityDirection;
                boxcastDistance = Physics2D.defaultContactOffset + groundBufferDistance;
                RaycastHit2D hit2 = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);

                if (!hit2) { canMoveInVelDir = false; }

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
                //else, perform a second boxcast to check we onl move if we remain grounded
                boxcastOrigin.y = movement.y + (Mathf.Sign(movement.y) * (moveBufferDistance + mainCollider.bounds.size.y));
                boxcastDirection = gravityDirection;
                boxcastDistance = Physics2D.defaultContactOffset + groundBufferDistance;
                RaycastHit2D hit2 = Physics2D.BoxCast(boxcastOrigin, boxSize, 0f, boxcastDirection, boxcastDistance, groundMask);

                if (!hit2) { canMoveAgainstVelDir = false; }

            }

            if (canMoveAgainstVelDir) { rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y); }

            if (!canMoveInVelDir && !canMoveAgainstVelDir) { rb.velocity = new Vector2(rb.velocity.x, 0f); }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
