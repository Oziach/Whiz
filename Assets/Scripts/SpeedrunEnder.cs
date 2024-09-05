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

        if (!SpeedrunHandler.Instance) { return;  }
        if(!SpeedrunHandler.Instance.IsSpeedrunning()) { return; }

        //if speedrun mode
        SpeedrunHandler.Instance.StopSpeedrunning(); //speedrunnign will set to false, but timer will still preserve the time.

        if (speedrunEndText != null) {
            speedrunEndText.gameObject.SetActive(true);
            speedrunEndText.text = "Final Time : " + SpeedrunHandler.Instance.GetCurrSpeedrunTime().ToString("F2");
        }
    }

}
