using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    [SerializeField] ShutterSegment shutter;
    [SerializeField] GameObject pressurePadVisual;
    [SerializeField] float onePixelVal = 0.06125f;

    private Vector2 originalPosition;
    private Vector2 finalPosition;

    private bool pressed = false;

    private void OnTriggerStay2D(Collider2D other) {
        CustomPhysics otherRb = other.transform.root.GetComponent<CustomPhysics>();
        if (!otherRb || !otherRb.IsGrounded()) { return; }

        pressed = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = pressurePadVisual.transform.position;
        finalPosition = originalPosition - (Vector2)transform.up * onePixelVal;
    }

    private void FixedUpdate() {
        pressed = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (pressed) {
            pressurePadVisual.transform.position = finalPosition;
        }
        else {
            pressurePadVisual.transform.position = originalPosition;
        }
    }

    public bool GetPressed() {
        return pressed;
    }
}
