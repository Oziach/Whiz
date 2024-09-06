using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunHandler : MonoBehaviour
{
    public static SpeedrunHandler Instance { get; private set; }


    bool speedrunning = false;
    float currTime = 0f;

    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currTime = 0f;
        speedrunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (speedrunning) {
            currTime += Time.deltaTime;
        }
    }

    public bool IsSpeedrunning() {
        return speedrunning;
    }

    public float GetCurrSpeedrunTime() {
        return currTime;
    }


    public void RestartSpeedrun() {
        speedrunning = true;
        currTime = 0f;
    }

    public void ResetSpeedrunTimer() {
        currTime = 0f;
    }

    public void StopSpeedrunning() {
        speedrunning = false;
    }

}
