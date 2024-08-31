using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] CustomPhysics rb;
    [SerializeField] SlimePatrol slimePatrol;
    [SerializeField] CustomGravityObject customGravityObject;
    [SerializeField] GameObject slimeDeathPrefab;
    [SerializeField] GameObject slimeVisual;
    


    // Start is called before the first frame update
    void Start()
    {
        UpdateVelocity();
    }

    private void FixedUpdate() {
        UpdateVelocity();
        slimePatrol.HandleVelocityFlipping();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Damager")) {
            Die();
        }
    }

    private void Die() {
        GameObject instantiated = Instantiate(slimeDeathPrefab, transform.position, Quaternion.identity, null);
        instantiated.transform.position = slimeVisual.transform.position;
        instantiated.transform.localScale = slimeVisual.transform.localScale;
        instantiated.transform.rotation = slimeVisual.transform.rotation;
        Destroy(gameObject);
    }

    public void UpdateVelocity() {
        if (!rb.IsGrounded()) { return; }
        Vector2 gravityDirection = customGravityObject.GetGravityDirection();
        int randomMul = Random.Range(0, 1);
        if(randomMul == 0) { randomMul = -1; }

        if (gravityDirection.x == 0 && rb.velocity.x == 0) { //vertical gravity
            rb.velocity = new Vector2(randomMul * moveSpeed, rb.velocity.y);
        }
        else if(gravityDirection.y == 0 && rb.velocity.y == 0){ //horizontal gravity
            rb.velocity = new Vector2(rb.velocity.x, randomMul * moveSpeed);
        }
    }

    private void OnDestroy() {

    }
}
