using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedrunEnder : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI speedrunEndText;

    // Start is called before the first frame update
    void Start()
    {
        speedrunEndText.gameObject.SetActive(false);
        SaveLoadSystem.SaveFinishedGameOnce();

        if (!SpeedrunHandler.Instance) { return;  }
        if(!SpeedrunHandler.Instance.IsSpeedrunning()) { return; }

        //if speedrun mode
        SpeedrunHandler.Instance.StopSpeedrunning(); //speedrunnign will set to false, but timer will still preserve the time.

        if (speedrunEndText != null) {
            speedrunEndText.gameObject.SetActive(true);


            speedrunEndText.text = "Final Time : " + GetFormattedEndTime();
        }
    }


    string GetFormattedEndTime() {

        float currTime = SpeedrunHandler.Instance.GetCurrSpeedrunTime();
        int mins = (int)(currTime / 60);
        int hours = (int)(currTime / 60 / 60);
        float seconds = (int)(currTime - (hours * 60 * 60) - (mins * 60));

        string timerText = "";
        if (hours > 0) { timerText += hours.ToString() + ":"; }
        if (mins > 0) { timerText += mins.ToString() + ":"; }
        timerText += seconds.ToString();

        timerText = String.Format("{0:00}:{1:00:00}", mins, seconds);

        return timerText;
    }

}
