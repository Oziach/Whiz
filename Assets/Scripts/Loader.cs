using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{

    public void LoadLevel(int level) {
        GameManager.Instance.LoadLevel(level);
    }

    public void LoadMainMenu() {
        GameManager.Instance.LoadMainMenu();
    }

    public void LoadLevelSelect() {
        GameManager.Instance.LoadLevelSelect();
    }


    public void LoadLatestLevel() {

        GameManager.Instance.LoadLatestLevel();
    }
}
