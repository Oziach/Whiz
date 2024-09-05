using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((collision.transform.root.GetComponent<Player>()))
        {
            if (MusicManager.Instance != null) { MusicManager.Instance.EnableMusic(); }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (MusicManager.Instance != null) { MusicManager.Instance.DisableMusic(); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
