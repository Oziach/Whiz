using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public event EventHandler OnGravityChanged;

    private Rigidbody2D rb;

    private float gravity;
    private Vector2 gravityDirection = Vector2.down;

    [SerializeField] float gravityChangeVelBoost;
    [SerializeField] float velocityCap;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = Mathf.Abs(Physics2D.gravity.y);
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity += gravityDirection * Time.deltaTime * gravity;
        if (Mathf.Abs(rb.velocity.x) > velocityCap){ rb.velocity = new Vector2(Math.Sign(rb.velocity.x) * velocityCap, rb.velocity.y); }
        if (Math.Abs(rb.velocity.y) > velocityCap){ rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * velocityCap); }

    }

    public void SetGravityDirection(Vector2 newGravityScale) {
        if (gravityDirection == newGravityScale) { return; }
        gravityDirection = newGravityScale;
        rb.velocity = gravityDirection * gravityChangeVelBoost;
        OnGravityChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetGravityDirection() { 
        return gravityDirection;
    }
}
