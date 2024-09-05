using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControls : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundsSlider;

    private float prevMusicVal = -1f;
    private float prevSoundVal = -1f;

    // Start is called before the first frame update
    void Start()
    {
        LoadMusicVolume();
        LoadSoundsVolume();

        MusicVolume();
        SoundsVolume();
    }

    private void LoadSoundsVolume() {
        if (SaveLoadSystem.HasSoundsVolume()) {
            soundsSlider.value = SaveLoadSystem.LoadSoundsVolume();
        }
    }

    private void LoadMusicVolume() {
        if (SaveLoadSystem.HasMusicVolume()) { 
            musicSlider.value = SaveLoadSystem.LoadMusicVolume();
        }
    }

    // Update is called once per frame
    void Update() {
        SoundsVolume();
        MusicVolume();
    }

    private void MusicVolume() {
        float currMusicVolume = musicSlider.value;

        if (currMusicVolume == prevMusicVal) { return; } //will alwys give false when loading scene for the first time

        //val changed
        if (MusicManager.Instance) {
            MusicManager.Instance.SetVolume(currMusicVolume);
            SaveLoadSystem.SaveMusicVolume(currMusicVolume);
        }
    }

    private void SoundsVolume() {
        float currSoundsVolume = soundsSlider.value;

        if (currSoundsVolume == prevSoundVal) { return; }

        //val changed
        if (SoundManager.Instance) {
            SoundManager.Instance.SetVolume(currSoundsVolume);
            SaveLoadSystem.SaveSoundsVolume(currSoundsVolume);
        }
    }

}
