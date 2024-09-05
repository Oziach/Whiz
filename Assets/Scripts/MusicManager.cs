using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private AudioSource audioSource;

    private float musicVolume = 1f;
    private float originalVolume;


    public static MusicManager Instance { get; private set; }

    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

        DontDestroyOnLoad(this);

        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume;

    }

    private void Start() {
        audioSource.volume = musicVolume * originalVolume;
        audioSource.Play();
    }

    public void UnmuteMusic() {
        musicVolume = 1;
        audioSource.volume = musicVolume * originalVolume;
    }

    public void MuteMusic() {
        musicVolume = 0;
        audioSource.volume = musicVolume * originalVolume;
    }

    public void EnableMusic() {
        audioSource.enabled = true;
    }

    public void DisableMusic() {
        audioSource.enabled = false;
    }
}
