using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    public void UpdateVolumes()
    {
        audioMixer.SetFloat("SFXVolume", LogarithmicDbTransform(Mathf.Clamp01(sfxSlider.value)));
        audioMixer.SetFloat("MusicVolume", LogarithmicDbTransform(Mathf.Clamp01(musicSlider.value)));
    }

    protected static float LogarithmicDbTransform(float volume)
    {
        volume = (Mathf.Log(89 * volume + 1) / Mathf.Log(90)) * 80;
        return volume - 80;
    }
}