using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shutter : MonoBehaviour
{
    [SerializeField] PressurePad pressurePad;
    [SerializeField] Transform shutterSegments;
    [SerializeField] CustomPhysics rb;
    [SerializeField] int shutterHeight;
    [SerializeField] float shutterSpeed;

    private Vector3 initialLocalPosition;
    private Vector3 finaLocalPosition;

    private void Start() {
        initialLocalPosition = transform.position;
        finaLocalPosition = initialLocalPosition + transform.up * shutterHeight;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("ShutterSegment")) {
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("ShutterSegment")) {
            other.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool opening = pressurePad.GetPressed();

        if (opening) {
            opening = false;

            rb.velocity = transform.up * shutterSpeed;
            if (shutterSegments.localPosition.y > finaLocalPosition.y) {
                shutterSegments.localPosition = new Vector2(shutterSegments.localPosition.x, finaLocalPosition.y);

            }
        }
        else {
            rb.velocity = -transform.up * shutterSpeed;
            if (shutterSegments.localPosition.y < initialLocalPosition.y) {
                shutterSegments.localPosition = new Vector2(transform.localPosition.x, initialLocalPosition.y);
            }
        }
    }
}
