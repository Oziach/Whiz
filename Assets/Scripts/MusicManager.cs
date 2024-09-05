using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private AudioSource audioSource;

    private float musicVolume = 1f;
    private float originalVolume;
    private int muteVol = 1;


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
        muteVol = 1;
        audioSource.volume = musicVolume * originalVolume * muteVol;
    }

    public void MuteMusic() {
        muteVol = 0;
        audioSource.volume = musicVolume * originalVolume * muteVol;
    }

    public void EnableMusic() {
        audioSource.enabled = true;
    }

    public void DisableMusic() {
        audioSource.enabled = false;
    }

    public void SetVolume(float vol) { 
        musicVolume = vol;
        audioSource.volume = musicVolume;
    }
}
