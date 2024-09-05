using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CustomGravityObject customGravityObject;
    private CustomPhysics rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float airResistanceSpeedMultiplier = 0.5f;

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

        float speedMul = rb.IsGrounded() ? 1 : airResistanceSpeedMultiplier;
        float currSpeed = speedMul * moveSpeed;

        Debug.Log(movementVector);

        if (gravityDir == Vector2.down || gravityDir == Vector2.up) {

            rb.velocity = new Vector2(movementVector.x * currSpeed, rb.velocity.y);
        }
        else {
            rb.velocity = new Vector2(rb.velocity.x, movementVector.y * currSpeed);
        }
    }

    public bool IsRunning() {

        if (!GameInput.Instance) { return false;}
        Vector2 movementVector = GameInput.Instance.GetMovementVector();

        if (!rb.IsGrounded()) { return false; }

        if (customGravityObject.GetGravityDirection().x == 0) {
            return movementVector.x != 0;
        } else {
            return movementVector.y != 0;
        }
        
    }
}
