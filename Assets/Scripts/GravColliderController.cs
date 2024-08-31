using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravColliderController : MonoBehaviour
{
    [SerializeField] GameObject verticalCollider;
    [SerializeField] GameObject horizontalCollider;
    [SerializeField] CustomGravityObject customGravityObject;
    [SerializeField] GameObject superParent;
    [SerializeField] Rigidbody2D superParentRb;
    [SerializeField] LayerMask layerMask;

    [SerializeField] float bufferDistance;
    // Start is called before the first frame update
    void Start()
    {
        ResetColliders();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetColliders() {
        Vector2 newGravityDir = customGravityObject.GetGravityDirection();
        if (horizontalCollider == verticalCollider) {
            return;
        }

        if (newGravityDir.x == 0) {
            horizontalCollider.SetActive(false);
            verticalCollider.SetActive(true);

        } else {
            horizontalCollider.SetActive(true);
            verticalCollider.SetActive(false);
        }
     
    }

    public bool SetSuperParentPosition() {

        Vector2 newGravityDir = customGravityObject.GetGravityDirection();

        RaycastHit2D hit, hit2;

        if (newGravityDir.x == 0) { //vertical


            BoxCollider2D collider = verticalCollider.GetComponent<BoxCollider2D>();
            ResetColliders();

            float boxcastDist = collider.bounds.size.y / 2;
            Vector2 boxSize = new Vector2(collider.bounds.size.x, Physics2D.defaultContactOffset);

            //check for up/down
            hit = Physics2D.BoxCast(transform.position, boxSize, 0, newGravityDir, boxcastDist, layerMask);
            //check for down/up, in case the other side of the collider clips
            hit2 = Physics2D.BoxCast(transform.position, boxSize, 0, -newGravityDir, boxcastDist, layerMask);

            //don't get stuck between crates etc.
            if (hit && hit2) {
                return false;
            }

            if (hit) {
                Vector2 newPosition = hit.point - newGravityDir * (boxcastDist + Physics2D.defaultContactOffset + bufferDistance);
                superParentRb.position = (newPosition);
                superParent.transform.position = (newPosition);
            }

          
            
            else if (hit2) {
                Vector2 newPosition = hit2.point + newGravityDir * (boxcastDist + Physics2D.defaultContactOffset + bufferDistance);
                superParent.transform.position = (newPosition);
                superParentRb.position = newPosition;
            }

        } 
        else {

            BoxCollider2D collider = horizontalCollider.GetComponent<BoxCollider2D>();
            ResetColliders();

            float boxcastDist = collider.bounds.size.x/2;
            Vector2 boxSize = new Vector2(Physics2D.defaultContactOffset, collider.bounds.size.y);

            //check left/right clipping
            hit = Physics2D.BoxCast(transform.position, boxSize, 0, newGravityDir, boxcastDist, layerMask);
            //check right/left
            hit2 = Physics2D.BoxCast(transform.position, boxSize, 0, -newGravityDir, boxcastDist, layerMask);

            //don't get stuck between crates etc.
            if (hit && hit2) {
                return false;
            }

            if (hit) {
                Vector2 newPosition = hit.point - newGravityDir * (boxcastDist + Physics2D.defaultContactOffset + bufferDistance);
                superParent.transform.position = (newPosition);
                superParentRb.position = newPosition;

            }

            
            else if (hit2) {
                Vector2 newPosition = hit2.point + newGravityDir * (boxcastDist + Physics2D.defaultContactOffset + bufferDistance);
                superParent.transform.position = (newPosition);
                superParentRb.position = newPosition;

            }


        }

        return true;
    }


}
