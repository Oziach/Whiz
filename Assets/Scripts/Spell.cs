using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpellAnimHandler anim;
   
    [SerializeField] float moveSpeed;
    private Vector2 gravDirection;
    private Vector2 spellDir;

    [SerializeField] GameObject spellFizzle;
    [SerializeField] Transform fizzleCenter;
    [SerializeField] GameObject gravityChangedVFX;

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        CustomGravityObject customGravityObject = other.transform.root.GetComponent<CustomGravityObject>(); 

        if (customGravityObject != null) {
            customGravityObject.SetGravityDirection(gravDirection);
        }

        //wanna play some animation/particle effects for spoosh vfx later. 
        //hi, its later.
        Debug.Log("Spell collided with: " + other.gameObject.name);
        Destroy(gameObject);

        Instantiate(spellFizzle, fizzleCenter.position, Quaternion.identity, null);

        GameObject instantiatedVisual = Instantiate(gravityChangedVFX, other.transform.position, Quaternion.identity, other.transform);
        instantiatedVisual.transform.right = gravDirection;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(Vector2 spellDirection, Vector2 gravityDirection) {
        gravDirection = gravityDirection;
        spellDir = spellDirection;
        rb.velocity = spellDirection * moveSpeed;
        transform.right = spellDirection;
        anim.AnimateSpell(spellDirection, gravityDirection);
        Debug.Log(spellDirection);
    }

    public Vector2 GetGravityDirection() {
        return gravDirection;
    }

    public Vector2 GetSpellDirection() {
        return spellDir;
    }
}
