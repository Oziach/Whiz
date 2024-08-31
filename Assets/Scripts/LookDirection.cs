using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDirection : MonoBehaviour
{
    [SerializeField] CustomPhysics rb;
    [SerializeField] CustomGravityObject customGravityObject;


    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update() {
        Vector2 gravityDirection = customGravityObject.GetGravityDirection();
        //if gravity vertical
        if (gravityDirection.x == 0) {

            if (rb.velocity.x > 0) {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < 0) {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

        }
        //if gravity to left
        else if(gravityDirection.x > 0){
            if (rb.velocity.y > 0) {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.y < 0) {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        //if gravity to right
        else {
            if (rb.velocity.y < 0) {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.y > 0) {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
