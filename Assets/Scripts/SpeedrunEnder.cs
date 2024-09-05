using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunEnder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!SpeedrunHandler.Instance) { return;  }

        SpeedrunHandler.Instance.StopSpeedrunning(); //speedrunnign will set to false, but timer will still preserve the time.
    }

}
