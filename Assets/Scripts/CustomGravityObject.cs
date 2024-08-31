using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravityObject : MonoBehaviour {
    public event EventHandler OnGravityChanged;

    [SerializeField] GravColliderController gravColliderController;

    private CustomPhysics rb;

    private float gravity;
    private Vector2 gravityDirection = Vector2.down;

    [SerializeField] float gravityChangeVelBoost;
    [SerializeField] float velocityCap;

    private void Awake() {
        rb = GetComponent<CustomPhysics>();
    }

    // Start is called before the first frame update
    void Start() {
        gravity = Mathf.Abs(Physics2D.gravity.y);
    }

    // Update is called once per frame
    void Update() {
        rb.velocity += gravityDirection * Time.deltaTime * gravity;
        if (Mathf.Abs(rb.velocity.x) > velocityCap) { rb.velocity = new Vector2(Math.Sign(rb.velocity.x) * velocityCap, rb.velocity.y); }
        if (Math.Abs(rb.velocity.y) > velocityCap) { rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * velocityCap); }

    }

    public void SetGravityDirection(Vector2 newGravityScale) {
        if (gravityDirection == newGravityScale) { return; }
        
        Vector2 oldGravityScale = gravityDirection;
        gravityDirection = newGravityScale;

        //if rotation not possible, don't.
        if (gravColliderController && !gravColliderController.SetSuperParentPosition()) {
            Debug.Log("There isn't enough space to rotate gravity here...By - " + gameObject.name);

            gravityDirection = oldGravityScale; //reset gravity to what we started from
            gravColliderController.ResetColliders();

            return;
        }

        //else if rotation possible
        gravityDirection = newGravityScale;
        if(gravColliderController) gravColliderController.ResetColliders();

        rb.velocity = gravityDirection * gravityChangeVelBoost;
        OnGravityChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetGravityDirection() {
        return gravityDirection;
    }
}