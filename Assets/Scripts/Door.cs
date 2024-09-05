using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    bool locked = false;

    private void OnTriggerStay2D(Collider2D other) {

        if (locked){ return; }

        LevelManager.Instance.LoadNextScene();
    } 
        
    public void UnlockDoor() {
        locked = false;
    }

    
}
