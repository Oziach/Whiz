using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.PackageManager;
using UnityEngine;


public class PlayerCasting : MonoBehaviour
{
    public class SpellcastEventArgs : EventArgs {
        public Vector2 direction { get; set; }
    }
    public event EventHandler<SpellcastEventArgs> OnSpellCasted;
    public event EventHandler<SpellcastEventArgs> OnGravcast;

    [SerializeField] CustomPhysics rb;
    [SerializeField] CustomGravityObject customGravityObject;
    [SerializeField] GameObject SpellPrefab;
    [SerializeField] GameObject wandPopPrefab;

    [SerializeField] Transform playerVisual;
    [SerializeField] Transform wandPopOrigin;
    [SerializeField] Transform spellcastOrigin;
    [SerializeField] float spellcastOffset;


    [SerializeField] private float rechargeTime = 0f;
    private float rechargeCountdown;
    private bool recharging = false;


    int currentGravityRotation = 0;

    // Start is called before the first frame update
    void Start()
    {

        rechargeCountdown = rechargeTime;

        if (!GameInput.Instance || !GameplayUI.Instance) { return; }

        GameInput.Instance.OnRotateGravityPerformed += GameInput_OnRotateGravityPerformed;
        GameInput.Instance.OnGravcastPerformed += GameInput_OnGravcastPerformed;
        GameInput.Instance.OnSpellcastPerformed += GameInput_OnSpellcastPerformed;
        GameplayUI.Instance.SetGravityArrowDirection(RotationToDirection());
        rb = GetComponent<CustomPhysics>();
    }

    private void GameInput_OnSpellcastPerformed(object sender, System.EventArgs e) {
        CastSpell();
        recharging = true;
    }

    public void CastSpell() {
        if (recharging) { return; }
        Debug.Log(spellcastOrigin.position);
        Vector2 currentGravity = customGravityObject.GetGravityDirection();

        Vector2 inputVector = GameInput.Instance ? GameInput.Instance.GetMovementVector() : Vector2.zero;
        Vector2 spellcastDirection;
        Vector2 spellcastPosition = spellcastOrigin.position;

        if (currentGravity.x == 0) { //vertical gravity
            spellcastDirection = inputVector.y == 0 ? Vector2.right * playerVisual.localScale.x : Vector2.up * Mathf.Sign(inputVector.y); //i should've made a GetLookDirection() method.
            spellcastPosition = spellcastDirection.x == 0 ? (Vector2)spellcastOrigin.position + (spellcastDirection * spellcastOffset) : spellcastOrigin.position;
        }
        else { //horizontal gravity
            if (inputVector.x == 0) {
                spellcastDirection = currentGravity == Vector2.right ? Vector2.up * playerVisual.localScale.x : Vector2.up * -playerVisual.localScale.x;

            }
            else {
                spellcastDirection = Vector2.right * Mathf.Sign(inputVector.x);
            }
            spellcastPosition = spellcastDirection.y == 0 ? (Vector2)spellcastOrigin.position + (spellcastDirection * spellcastOffset) : spellcastOrigin.position;
        }


        GameObject instantiated = Instantiate(SpellPrefab, spellcastPosition, Quaternion.identity, null);
        Instantiate(wandPopPrefab, wandPopOrigin.position, Quaternion.identity, wandPopOrigin);
        Spell spell = instantiated.GetComponent<Spell>();

        Debug.Log(spellcastDirection);
        spell.Fire(spellcastDirection, RotationToDirection());
        OnSpellCasted?.Invoke(this, new SpellcastEventArgs() { direction = spellcastDirection });
    }

    private void GameInput_OnRotateGravityPerformed(object sender, GameInput.RotateGravityEventArgs e) {
        RotateGravity(e.gravityIndex);
    }

    private void RotateGravity(int gravityIndex) {
        currentGravityRotation = gravityIndex;
        GameplayUI.Instance.SetGravityArrowDirection(RotationToDirection());
    }

    private void GameInput_OnGravcastPerformed(object sender, System.EventArgs e) {
        Gravcast();
    }

    private void Gravcast() {

        if (rb && !rb.IsGrounded()) { return;  }

        Vector2 newGravityDirection = RotationToDirection();
        customGravityObject.SetGravityDirection(newGravityDirection);
        OnGravcast?.Invoke(this, new SpellcastEventArgs { direction = newGravityDirection });
    }

    public Vector2 RotationToDirection() {
        switch (currentGravityRotation) {
            case 0:
                return Vector2.up;
   
            case 1:
                return Vector2.right;

            case 2:
                return Vector2.down;

            case 3:
                return Vector2.left;

        }

        return Vector2.down;
    }

    // Update is called once per frame
    void Update() {
        HandleRecharge();
    }


    private void OnDestroy() {
        if (!GameInput.Instance) { return; }
        GameInput.Instance.OnRotateGravityPerformed -= GameInput_OnRotateGravityPerformed;
        GameInput.Instance.OnGravcastPerformed -= GameInput_OnGravcastPerformed;
        GameInput.Instance.OnSpellcastPerformed -= GameInput_OnSpellcastPerformed;
    }

    private void HandleRecharge() {
        if (recharging) {
            rechargeCountdown -= Time.deltaTime;
            if (rechargeCountdown <= 0) {
                recharging = false;
                rechargeCountdown = rechargeTime;
            }
        }
    }

    
    public bool IsRecharging() {
        return recharging;
    }

}
