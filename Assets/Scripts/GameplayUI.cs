using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayUI : MonoBehaviour
{

    public static GameplayUI Instance { get; private set; }

    const string SHOW_ERROR_TEXT_TRIGGER = "ShowErrorTextTrigger";

    [SerializeField] GameObject gravityArrow;
    [SerializeField] Animator errorTextAnim;

    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGravityArrowDirection(Vector2 dir) {
        gravityArrow.transform.right = dir;
    }

    public void ShowErrorText() {
        errorTextAnim?.SetTrigger(SHOW_ERROR_TEXT_TRIGGER);
    }
}
