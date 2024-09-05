using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    bool locked = false;

    private void OnTriggerStay2D(Collider2D other) {

        if (locked){ return; }

        if (GameManager.Instance) {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int currentLevel = currentSceneIndex - GameManager.Instance.GetLevel1Index() + 1;
            GameManager.Instance.SetHighestLevelReached(currentLevel+1);
        }

        LevelManager.Instance.LoadNextScene();
    } 
        
    public void UnlockDoor() {
        locked = false;
    }

    
}
