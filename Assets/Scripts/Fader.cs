using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] float fadeTime;
    [SerializeField] bool reverse;

    private float timeLeft;
    private float initialAlpha;

    private void Awake() {
        timeLeft = fadeTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        initialAlpha = spriteRenderer.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0) { return; }

        timeLeft -= Time.deltaTime;
        Color currColor = spriteRenderer.color;
        float alpha = initialAlpha * timeLeft / fadeTime;
        if (reverse) { alpha = 1 - alpha; }
        spriteRenderer.color = new Color(currColor.r, currColor.g, currColor.b, alpha);
    }
}
