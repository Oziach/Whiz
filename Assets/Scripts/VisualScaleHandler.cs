using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualScaleHandler : MonoBehaviour
{

    [SerializeField] CustomGravityObject customGravityObject;

    Vector3 originalSpriteLocalPosition;

    // Start is called before the first frame update
    void Start()
    {
        customGravityObject.OnGravityChanged += CustomGravityObject_OnGravityChanged;

        Vector2 gravityDirection = customGravityObject.GetGravityDirection();
        originalSpriteLocalPosition = transform.localPosition * gravityDirection * Vector2.down; //this is definetely completley wrong
                                                                                            //supposed to handle if object starts out flipped, to retrieve the local position if it was striaght
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CustomGravityObject_OnGravityChanged(object sender, System.EventArgs e) {

        //adjust sprite accordingly
        Vector2 gravityDirection = customGravityObject.GetGravityDirection();
        Vector3 currentScale = transform.localScale;

        if (gravityDirection == Vector2.down) {
            transform.localScale = new Vector3(currentScale.x, Mathf.Abs(currentScale.y), currentScale.z);
            transform.localPosition = originalSpriteLocalPosition;
            transform.localRotation = Quaternion.identity;

        } else if (gravityDirection == Vector2.up) {
            transform.localScale = new Vector3(currentScale.x, -Mathf.Abs(currentScale.y), currentScale.z);
            transform.localPosition = new Vector3(originalSpriteLocalPosition.x,
                -originalSpriteLocalPosition.y, originalSpriteLocalPosition.z);
            transform.localRotation = Quaternion.identity;

        } else if (gravityDirection == Vector2.right) {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            transform.localPosition = new Vector3(-originalSpriteLocalPosition.y, originalSpriteLocalPosition.x, originalSpriteLocalPosition.z);
        } else if (gravityDirection == Vector2.left) {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
            transform.localPosition = new Vector3(originalSpriteLocalPosition.y, -originalSpriteLocalPosition.x, originalSpriteLocalPosition.z);
        
        }
    }

    private void OnDestroy() {
        customGravityObject.OnGravityChanged -=  CustomGravityObject_OnGravityChanged;

    }

}
