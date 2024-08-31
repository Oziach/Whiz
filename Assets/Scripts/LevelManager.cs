using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance {  get; private set; }
    private bool restart = false;
    private float timeToRestart = 0f;

    [SerializeField] private float defaultLevelRestartDelay = 1f;

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
    void Update() {
        if (!restart) { return; }

        timeToRestart -= Time.deltaTime;
        if (timeToRestart > 0) { return; }

        ReloadScene();
    }

    private static void ReloadScene() {
        //restart the level
        var currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIndex);
    }

    private static void LoadNextScene() {
        //restart the level
        var currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIndex+1);
    }

    public void RestartLevel(float restartDelay = -1f) {
        restartDelay = restartDelay == -1f ? defaultLevelRestartDelay : restartDelay;
        restart = true;
        timeToRestart = restartDelay;
    }

    public void LoadNextLevel() {
        RestartLevel(0f);
    }
}
