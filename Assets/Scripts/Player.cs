using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] CustomPhysics rb;
    [SerializeField] PlayerAnimHandler playerAnimHandler;

    [SerializeField] GameObject playerVisual;
    [SerializeField] GameObject playerDeathPrefab;

    [SerializeField] private float deathFadeTime = 1f;
    [SerializeField] private float deathCameraShakeTime = 0.25f;
    [SerializeField] private float deathCameraShakeIntensity = 1f;
    [SerializeField] private int keys = 0;


    void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<CustomPhysics>();

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Trigger netered: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Damager")) {
            Die();
        }
    }

    public void Die() {
        LevelManager.Instance.RestartLevel(deathFadeTime);

        CreateDeathPrefab();
        Destroy(gameObject);

        SoundManager.Instance?.PlayPlayerDeathSound();
        CinemachineShake.Instance?.ShakeCamera(deathCameraShakeIntensity, deathCameraShakeTime);
    }

    private void CreateDeathPrefab() {
        GameObject instantiated = Instantiate(playerDeathPrefab);
        instantiated.transform.rotation = playerAnimHandler.transform.rotation;
        instantiated.transform.position = playerAnimHandler.transform.position;
        instantiated.transform.localScale = playerAnimHandler.transform.localScale;
    }

    public bool IsRunning() {
        if (!playerMovement) { return false; }
        return playerMovement.IsRunning();
    }

    public bool IsGrounded() {
        return rb.IsGrounded();
    }

    //getters and setters
    public void AddKey() {
        keys++;
    }

    public int GetKeys() {
        return keys;
    }
}
