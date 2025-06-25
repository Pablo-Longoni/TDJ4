using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    void Start()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        _musicSlider.value = savedMusicVolume;
        AudioManager.Instance.VolumeMusic(savedMusicVolume);

        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        _sfxSlider.value = savedSFXVolume;
        AudioManager.Instance.VolumeSFX(savedSFXVolume);

        // Agregar los listeners dinámicamente
        _musicSlider.onValueChanged.AddListener(AudioManager.Instance.VolumeMusic);
        _sfxSlider.onValueChanged.AddListener(AudioManager.Instance.VolumeSFX);
    }
}
