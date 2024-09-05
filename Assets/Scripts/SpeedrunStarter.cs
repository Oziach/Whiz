using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
