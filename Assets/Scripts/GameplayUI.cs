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
    [SerializeField] TextMeshProUGUI speedrunTimer;

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
    void Start() {
        SpeedrunTimerInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpeedrunHandler.Instance && SpeedrunHandler.Instance.IsSpeedrunning() && speedrunTimer) {
            speedrunTimer.text = GetFormattedTime();
        }
    }

    public void SetGravityArrowDirection(Vector2 dir) {
        if (!gravityArrow) { return;  }
        gravityArrow.transform.right = dir;
    }
    
    string GetFormattedTime() {

        float currTime = SpeedrunHandler.Instance.GetCurrSpeedrunTime();
        int mins = (int)(currTime / 60);
        int hours = (int)(currTime / 60 / 60);
        int seconds = (int) (currTime - (hours * 60 * 60) - (mins * 60));

        string timerText = "";
        if (hours > 0) { timerText += hours.ToString() + ":"; }
        if (mins > 0) { timerText += mins.ToString() + ":"; }
        timerText += seconds.ToString();

        return timerText;
    }


    public void ShowErrorText() {
        if (errorTextAnim) { errorTextAnim.SetTrigger(SHOW_ERROR_TEXT_TRIGGER); }
    }

    public void ShowHint() {

        if (LevelManager.Instance) {
            LevelManager.Instance.ShowHint();
        }

        if (!hintButtonImage) { return;  }

        Color imageCol = hintButtonImage.color;
        hintButtonImage.color = new Color(imageCol.r, imageCol.g, imageCol.b, hintButtonDisabledOpacityValue);

        hintButtonButton.enabled = false;
    }

    private void SpeedrunTimerInit() {
        if (SpeedrunHandler.Instance && SpeedrunHandler.Instance.IsSpeedrunning() && speedrunTimer) {
            speedrunTimer.gameObject.SetActive(true);
            speedrunTimer.text = GetFormattedTime();
        }
        else if(speedrunTimer){
            speedrunTimer.gameObject.SetActive(false);
        }
    }


}
