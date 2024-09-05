using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private float sfxVolume = 1f;
    private int muteVol = 1;

    [SerializeField] AudioClip gravcastSound;
    [SerializeField] AudioClip spellcastSound;
    [SerializeField] AudioClip playerDeathSound;
    [SerializeField] AudioClip slimeDeathSound;
    [SerializeField] AudioClip spellImpactSound;
    [SerializeField] AudioClip shutterOpenSound;
    [SerializeField] AudioClip shutterCloseSound;

    [SerializeField] float gravcastVolume;
    [SerializeField] float spellcastVolume;
    [SerializeField] float playerDeathSoundVolume;
    [SerializeField] float slimeDeathSoundVolume;
    [SerializeField] float spellImpactSoundVolume;
    [SerializeField] float shutterOpenVolume;
    [SerializeField] float shutterCloseVolume;





    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
        //might want to DontDestroyOnLoad() this
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGravcastSound() {
        AudioSource.PlayClipAtPoint(gravcastSound, Camera.main.transform.position, sfxVolume * muteVol * gravcastVolume);
    }

    public void PlayPlayerDeathSound() {
        AudioSource.PlayClipAtPoint(playerDeathSound, Camera.main.transform.position, sfxVolume * muteVol * playerDeathSoundVolume);
    }

    public void PlaySlimeDeathSound() {
        AudioSource.PlayClipAtPoint(slimeDeathSound, Camera.main.transform.position, sfxVolume * muteVol * slimeDeathSoundVolume);

    }
    public void PlaySpellcastSound() {
        AudioSource.PlayClipAtPoint(spellcastSound, Camera.main.transform.position, sfxVolume * muteVol * spellcastVolume);
    }

    public void PlaySpellImpactSound() {
        AudioSource.PlayClipAtPoint(spellImpactSound, Camera.main.transform.position, sfxVolume * muteVol * spellImpactSoundVolume);
    }

    public void PlayShutterOpenSound() {
        AudioSource.PlayClipAtPoint(shutterOpenSound, Camera.main.transform.position, sfxVolume * muteVol * shutterOpenVolume);
    }

    public void PlayShutterCloseSound() {
        AudioSource.PlayClipAtPoint(shutterCloseSound, Camera.main.transform.position, sfxVolume * muteVol * shutterCloseVolume);
    }


    public bool IsMuted() {
        return muteVol == 0;
    }

    public void EnableSFX() {
        muteVol = 1;
    }

    public void DisableSFX() { 
        muteVol = 0; 
    }

    public void SetVolume(float vol) {
        sfxVolume = vol;
    }
}
