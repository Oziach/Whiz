using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] Door door;

    private void OnTriggerStay2D(Collider2D other) {

        if(other.CompareTag("Player")) {
            Player player = other.transform.root.GetComponent<Player>();
            if (!player || player.GetKeys() <= 0) { return; }

            door.UnlockDoor();
            Destroy(gameObject);

        }

    }
}
