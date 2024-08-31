using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CustomGravityObject customGravityObject;
    private CustomPhysics rb;

    [SerializeField] private int moveSpeed;

    private void Awake() {
        rb = GetComponent<CustomPhysics>();
        customGravityObject = GetComponent<CustomGravityObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }


    // Update is called once per frame
    void Update()
    {
        HandleMovement(); 
    }

    private void HandleMovement() {
        
        if (!GameInput.Instance) { return; }

        //movement
        Vector2 movementVector = GameInput.Instance.GetMovementVector();
        Vector2 gravityDir = customGravityObject.GetGravityDirection();

        if (gravityDir == Vector2.down || gravityDir == Vector2.up) {

            rb.velocity = new Vector2(movementVector.x * moveSpeed, rb.velocity.y);
        }
        else {
            rb.velocity = new Vector2(rb.velocity.x, movementVector.y * moveSpeed);
        }
    }

    public bool IsRunning() {

        if (!GameInput.Instance) { return false;}
        Vector2 movementVector = GameInput.Instance.GetMovementVector();

        if (customGravityObject.GetGravityDirection().x == 0) {
            return movementVector.x != 0;
        } else {
            return movementVector.y != 0;
        }
        
    }
}
