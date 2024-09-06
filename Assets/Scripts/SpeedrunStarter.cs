using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        
        if(GameManager.Instance && SaveLoadSystem.HasFinishedGameOnce()) {
            gameObject.SetActive(true);
        }

        //speedrun handler - terminate all existing speedrun stats
        if (!SpeedrunHandler.Instance) { return;  }

        SpeedrunHandler.Instance.StopSpeedrunning();
        SpeedrunHandler.Instance.ResetSpeedrunTimer();

    }

    public void StartSpeedrunMode() {
        if (!GameManager.Instance || !SpeedrunHandler.Instance) { return; }

        SpeedrunHandler.Instance.RestartSpeedrun();
        GameManager.Instance.LoadLevel(1);
    }


}
