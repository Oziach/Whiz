using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterSegment : MonoBehaviour
{
    [SerializeField] PressurePad pressurePad;
    [SerializeField] int shutterHeight;
    [SerializeField] float shutterSpeed;
                
    private Vector3 initialLocalPosition;   
    private Vector3 finaLocalPosition;

    private CustomPhysics rb;
    private CustomGravityObject customGravityObject;
    private BoxCollider2D segmentCollider;
    private SpriteRenderer spriteRenderer;

    [SerializeField] bool overlap = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<CustomPhysics>();
        customGravityObject = GetComponent<CustomGravityObject>();
        //customGravityObject.SetGravityDirection(Vector2.zero);
        segmentCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        initialLocalPosition = transform.localPosition;
        finaLocalPosition = transform.up;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("ShutterHider")) {
            segmentCollider.enabled = false;
            spriteRenderer.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("ShutterHider")) {
            segmentCollider.enabled = true;
            spriteRenderer.enabled = true;
        }
    }

    // Update is called once per frame
    void Update() {

        if (overlap) {
            ProceduralSegmentGen();
            return;
        }

        bool opening = pressurePad.GetPressed();

        if (opening) {
            rb.velocity = transform.up * shutterSpeed;
            if (transform.localPosition.y >= finaLocalPosition.y) {
                transform.localPosition = new Vector2(transform.localPosition.x, finaLocalPosition.y);
                rb.velocity = Vector2.zero;
            }
        }
        else {
            rb.velocity = -transform.up * shutterSpeed;

            if (transform.localPosition.y <= initialLocalPosition.y) {
                transform.localPosition = new Vector2(transform.localPosition.x, initialLocalPosition.y);
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void ProceduralSegmentGen() {
        bool opening = pressurePad.GetPressed();

        if (opening) {
            rb.velocity = transform.up * shutterSpeed;

            if (transform.localPosition.y >= finaLocalPosition.y) {
                transform.localPosition = new Vector2(transform.localPosition.x, finaLocalPosition.y);
                rb.velocity = Vector2.zero;
            }
        }
        else {
            rb.velocity = -transform.up * shutterSpeed;

            if (transform.localPosition.y <= initialLocalPosition.y) {
                transform.localPosition = new Vector2(transform.localPosition.x, initialLocalPosition.y);
                rb.velocity = Vector2.zero;

            }
        }
    }
}
