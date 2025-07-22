using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource soundSource;
    [SerializeField] private AudioMixer audioMixer;

  /* [Header("Music")]
    public AudioClip _gameMusic;
    public AudioClip _menuMusic;
  */
    [Header("Sound")]
    public AudioClip _turning;
    public AudioClip _portal;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("AudioManager inicializado");
        }
        else
        {
            Debug.Log("AudioManager destruido");
            Destroy(gameObject);
        }
    }

    public void VolumeMusic(float sliderMusic)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderMusic) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderMusic);
        Debug.Log("Volumen música prefs: "+ sliderMusic);
    }

    public void VolumeSFX(float sliderSFX)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderSFX) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sliderSFX);
        Debug.Log("Volumen música prefs: " + sliderSFX);
    }

    /*  private void SetBackgroundMusic()
      {
          string sceneName = SceneManager.GetActiveScene().name;

          switch (sceneName)
          {
              case "Menu":
                  ChangeBackgroundMusic(_menuMusic);
                  break;
              case "GameScene":
                  ChangeBackgroundMusic(_gameMusic);
                  break;
          }

          soundSource.Play();
          musicSource.Play();
      }

      public void playSound(AudioClip clip)
      {
          soundSource.PlayOneShot(clip);
      }

      public void ChangeBackgroundMusic(AudioClip newMusic)
      {
          musicSource.clip = newMusic;
          musicSource.Play();
      }*/
}
