using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class GameplayUI : MonoBehaviour
{

    public static GameplayUI Instance { get; private set; }

    const string SHOW_ERROR_TEXT_TRIGGER = "ShowErrorTextTrigger";

    [SerializeField] GameObject gravityArrow;
    [SerializeField] Animator errorTextAnim;
    [SerializeField] UnityEngine.UI.Image hintButtonImage;
    [SerializeField] UnityEngine.UI.Button hintButtonButton;

    [SerializeField] float hintButtonDisabledOpacityValue = 0.8f;

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
        if (!gravityArrow) { return;  }
        gravityArrow.transform.right = dir;
    }

    public void ShowErrorText() {
        if (errorTextAnim) { errorTextAnim.SetTrigger(SHOW_ERROR_TEXT_TRIGGER); }
    }

    public void ShowHint() {

        if (LevelManager.Instance) {
            LevelManager.Instance.ShowHint();
        }

        Color imageCol = hintButtonImage.color;
        hintButtonImage.color = new Color(imageCol.r, imageCol.g, imageCol.b, hintButtonDisabledOpacityValue);

        hintButtonButton.enabled = false;
    }
}
