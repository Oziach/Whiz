using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [SerializeField] int level1Index = 1;
    [SerializeField] int highestPossibleLevel = 1;
    private int highestLevelReached = 1;

    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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


    //getters and setters
    public void SetHighestLevelReached(int level) {
        highestLevelReached = Mathf.Min(level, highestLevelReached);
        highestLevelReached = Mathf.Min(highestLevelReached, highestPossibleLevel);
    }

    public int GetHighestLevelReached() {
        return highestLevelReached;
    }
}
