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
    [SerializeField] private GameObject hintGameOjbect;

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
        SaveHighestLevelReached();
    }

    private static void SaveHighestLevelReached() {
        if (GameManager.Instance) {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int currentLevel = currentSceneIndex - GameManager.Instance.GetLevel1Index() + 1;
            GameManager.Instance.SetHighestLevelReached(currentLevel);
        }
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

    public void RestartLevel(float restartDelay = -1f) {
        restartDelay = restartDelay == -1f ? defaultLevelRestartDelay : restartDelay;
        restart = true;
        timeToRestart = restartDelay;
    }

    public void LoadNextScene() {
        GameManager.Instance.LoadNextScene();
    }

    public void ShowErrorText() {
        if (GameplayUI.Instance) {GameplayUI.Instance.ShowErrorText(); }
    }

    public void ShowHint() {
        if (!hintGameOjbect) { return; }
        hintGameOjbect.SetActive(true);
    }
}
