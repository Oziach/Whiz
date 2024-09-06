using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    [SerializeField] GameObject pressurePadVisual;
    [SerializeField] BoxCollider2D trigger;
    [SerializeField] LayerMask triggerLayerMask;
    [SerializeField] float onePixelVal = 0.06125f;

    private Vector2 originalPosition;
    private Vector2 finalPosition;

    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = pressurePadVisual.transform.position;
        finalPosition = originalPosition - (Vector2)transform.up * onePixelVal;
    }

    private void FixedUpdate() {
        var collision = Physics2D.OverlapBox(trigger.bounds.center, trigger.bounds.size, 0f, triggerLayerMask);

        if (collision){
            CustomGravityObject cgo = collision.transform.root.GetComponent<CustomGravityObject>();
            if (cgo && cgo.GetGravityDirection() == -(Vector2)transform.up) {
                if(pressed == false) {
                    if (SoundManager.Instance) { SoundManager.Instance.PlayShutterOpenSound(); }
                }
                pressed = true;

            }

        }
        else {
            if(pressed == true) {
                if (SoundManager.Instance) { SoundManager.Instance.PlayShutterCloseSound(); }
            }
            pressed = false;
        }
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
