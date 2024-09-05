using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{

    static string HIGHEST_LEVEL_REACHED = "HighestLevelReached";
    static string MUSIC_VOLUME = "MusicVolume";
    static string SOUNDS_VOLUME = "SoundsVolume";

    public static void SaveHighestLevelReached(int level) {
        PlayerPrefs.SetInt(HIGHEST_LEVEL_REACHED, level);
        PlayerPrefs.Save();
    }

    public static int LoadHighestLevelReached() {
       return PlayerPrefs.GetInt(HIGHEST_LEVEL_REACHED);

    }

    public static void SaveMusicVolume(float volume) { 
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.Save();

    }
    public static float LoadMusicVolume() { 
        return PlayerPrefs.GetFloat(MUSIC_VOLUME);
    }

    public static void SaveSoundsVolume(float volume) {
        PlayerPrefs.SetFloat(SOUNDS_VOLUME, volume);
        PlayerPrefs.Save();

    }

    public static float LoadSoundsVolume() { 
       return PlayerPrefs.GetFloat(SOUNDS_VOLUME);
    }

    public static bool HasHighestLevel() {
        return PlayerPrefs.HasKey(HIGHEST_LEVEL_REACHED);
    }

    public static bool HasMusicVolume() {
        return PlayerPrefs.HasKey(MUSIC_VOLUME);
    }

    public static bool HasSoundsVolume() {
        return PlayerPrefs.HasKey(SOUNDS_VOLUME);
    }


}
