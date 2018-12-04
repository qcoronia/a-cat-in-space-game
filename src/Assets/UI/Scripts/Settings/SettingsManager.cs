using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public void UpdateBgmVolume()
    {
        masterMixer.SetFloat("BgmVolume", bgmSlider.value);
    }

    public void UpdateSfxVolume()
    {
        masterMixer.SetFloat("SfxVolume", sfxSlider.value);
    }
}
