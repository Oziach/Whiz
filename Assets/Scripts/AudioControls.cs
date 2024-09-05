using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControls : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundsSlider;

    // Start is called before the first frame update
    void Start()
    {
        MusicVolume();
        SoundsVolume();
    }

    // Update is called once per frame
    void Update() {
        SoundsVolume();
        MusicVolume();
    }

    private void MusicVolume() {
        if (MusicManager.Instance) {
            MusicManager.Instance.SetVolume(musicSlider.value);
        }
    }

    private void SoundsVolume() {
        if (SoundManager.Instance) {
            SoundManager.Instance.SetVolume(soundsSlider.value);
        }
    }

}
