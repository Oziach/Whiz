using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public class RotateGravityEventArgs : EventArgs {
        public int gravityIndex;
    }

    public event EventHandler OnGravcastPerformed;
    public event EventHandler<RotateGravityEventArgs> OnRotateGravityPerformed;
    public event EventHandler OnSpellcastPerformed;
    
    private PlayerInputActions playerInputActions;

    [SerializeField] float actionInputDelay = 0.025f;


    private void Awake() {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.DefaultMap.Gravcast.performed += Gravcast_Performed;

        playerInputActions.DefaultMap.RotateGravityRight.performed += RotateGravityRight_Performed;
        playerInputActions.DefaultMap.RotateGravityLeft.performed += RotateGravityLeft_Performed;
        playerInputActions.DefaultMap.RotateGravityUp.performed += RotateGravityUp_Performed;
        playerInputActions.DefaultMap.RotateGravityDown.performed += RotateGravityDown_Performed;

        playerInputActions.DefaultMap.Spellcast.performed += Spellcast_Performed;

    }

    private void Spellcast_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        StartCoroutine(SpellcastPerformed());
    }

    private IEnumerator SpellcastPerformed() {
        yield return new WaitForSeconds(actionInputDelay);

        if (this && gameObject) {
            OnSpellcastPerformed?.Invoke(this, EventArgs.Empty);
        }
    }



    private void RotateGravityRight_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRotateGravityPerformed?.Invoke(this, new RotateGravityEventArgs { gravityIndex = 1 });
    }

    private void RotateGravityLeft_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRotateGravityPerformed?.Invoke(this, new RotateGravityEventArgs { gravityIndex = 3 });

    }
    private void RotateGravityUp_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRotateGravityPerformed?.Invoke(this, new RotateGravityEventArgs { gravityIndex = 0 });
    }
    private void RotateGravityDown_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRotateGravityPerformed?.Invoke(this, new RotateGravityEventArgs { gravityIndex = 2 });
    }

    private void Gravcast_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        StartCoroutine(GravcastPerformed());
    }

    private IEnumerator GravcastPerformed() {
        yield return new WaitForSeconds(actionInputDelay);

        if (this && gameObject) {
            OnGravcastPerformed?.Invoke(this, EventArgs.Empty);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (this == null) { return; }
    }

    public Vector2 GetMovementVector() {
        return playerInputActions.DefaultMap.Movement.ReadValue<Vector2>();
    }

    private void OnDestroy() {
        playerInputActions.DefaultMap.Gravcast.performed -= Gravcast_Performed;

        playerInputActions.DefaultMap.RotateGravityRight.performed -= RotateGravityRight_Performed;
        playerInputActions.DefaultMap.RotateGravityLeft.performed -= RotateGravityLeft_Performed;
        playerInputActions.DefaultMap.RotateGravityUp.performed -= RotateGravityUp_Performed;
        playerInputActions.DefaultMap.RotateGravityDown.performed -= RotateGravityDown_Performed;

        playerInputActions.DefaultMap.Spellcast.performed -= Spellcast_Performed;
    }

}
