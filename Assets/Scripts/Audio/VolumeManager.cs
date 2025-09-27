using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _sfxToggle;
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


      /*  bool isMusicMuted = PlayerPrefs.GetInt("isMusicMuted", 0) == 1;
        bool isSfxMuted = PlayerPrefs.GetInt("isSfxMuted", 0) == 1;
        _musicToggle.isOn = isMusicMuted;
        _sfxToggle.isOn = isSfxMuted;
        _musicToggle.onValueChanged.AddListener(AudioManager.Instance.MuteMusic);
        _sfxToggle.onValueChanged.AddListener(AudioManager.Instance.MuteSfx);*/
    }
}
