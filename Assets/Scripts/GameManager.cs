using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    int level1Index = 3;
    int highestPossibleLevel = 13;
    private int highestLevelReached = 1;

    private const int MAIN_MENU_INDEX = 1;
    private const int LEVEL_SELECT_INDEX = 2;

    bool inputNullPrevFrame = false;
    private void Awake() {

        if (Instance) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (SaveLoadSystem.HasHighestLevel()) {
            highestLevelReached = SaveLoadSystem.LoadHighestLevelReached();
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

    public void LoadLatestLevel() {
        LoadLevel(highestLevelReached);
    }

    public void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(int level) {
        SceneManager.LoadScene(level1Index + level - 1);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(MAIN_MENU_INDEX);
    }

    public void LoadLevelSelect() {
        SceneManager.LoadScene(LEVEL_SELECT_INDEX);
    }

    //getters and setters
    public void SetHighestLevelReached(int level) {

        int prevHighestLevelReached = highestLevelReached;

        highestLevelReached = Mathf.Max(level, highestLevelReached);
        highestLevelReached = Mathf.Min(highestLevelReached, highestPossibleLevel);

        if (highestLevelReached > prevHighestLevelReached) {
            SaveLoadSystem.SaveHighestLevelReached(highestLevelReached);
        }
    }

    public int GetHighestLevelReached() {
        return highestLevelReached;
    }

    public int GetHighestPossibleLevel() {
        return highestPossibleLevel;
    }
    public int GetLevel1Index() {
        return level1Index;
    }
}
