using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] float fadeTime;
    [SerializeField] bool reverse;

    private float timeLeft;

    private void Awake() {
        timeLeft = fadeTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0) { return; }

        timeLeft -= Time.deltaTime;
        Color currColor = spriteRenderer.color;
        float alpha = timeLeft / fadeTime;
        if (reverse) { alpha = 1 - alpha; }
        spriteRenderer.color = new Color(currColor.r, currColor.g, currColor.b, alpha);
    }
}
