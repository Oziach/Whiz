using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAnimHandler : MonoBehaviour
{
    [SerializeField] Spell spell;
    [SerializeField] Transform spellArrowVisual;
    [SerializeField] Transform spellHullVisual;
    [SerializeField] Transform particles;

    [SerializeField] float spellRotatePositionOffset = 0.03125f;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void AnimateSpell(Vector2 spellDirection, Vector2 gravityDirection ) {
        RotateSpell(gravityDirection);

        //RotateParticleSystem(spellDirection);
    }

    private void RotateParticleSystem(Vector2 spellDirection) {
        int rotationIndex = 0;

        if (spellDirection == Vector2.up) {
            rotationIndex = 1;
        }
        else if (spellDirection == Vector2.down) {
            rotationIndex = 3;
        }
        else if (spellDirection == Vector2.left) {
            rotationIndex = 2;
        }

        particles.Rotate(0f, 0f, 90f * rotationIndex);
        spellHullVisual.Rotate(0f, 0f, 90f * rotationIndex);
    }

    private void RotateSpell(Vector2 gravDirection) {

        int rotationIndex = 0;

        if (gravDirection == Vector2.up) {
            rotationIndex = 1;
        }
        else if (gravDirection == Vector2.down) {
            rotationIndex = 3;
        }
        else if (gravDirection == Vector2.left) {
            rotationIndex = 2;
        }

        //spellArrowVisual.Rotate(0f, 0f, 90f * rotationIndex);
        spellArrowVisual.rotation = Quaternion.Euler(0f, 0f, 90f * rotationIndex);
        spellArrowVisual.position += spellRotatePositionOffset * spellArrowVisual.right;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
