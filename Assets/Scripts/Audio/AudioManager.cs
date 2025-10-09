using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource soundSource;
    [SerializeField] private AudioMixer audioMixer;

   [Header("Music")]
    public AudioClip [] _gameMusic;
    public AudioClip _menuMusic;
  
    [Header("Sound")]
    public AudioClip _turning;
    public AudioClip _portal;
    public AudioClip _activatePortal;
    public AudioClip _pressedSound;

    private int lastMusicIndex;
    private AudioDistortionFilter _distortionFilter;
    private AudioLowPassFilter _lowPassFilter;

    public bool _isMusicMuted;
    public bool _isSfxMuted;
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

    private void Start()
    {
        if( musicSource == null ) musicSource.loop = false;
        StartCoroutine(PlayMusicLoop());

        /*  _distortionFilter = musicSource.gameObject.AddComponent<AudioDistortionFilter>();
          _distortionFilter.distortionLevel = 0;

          _lowPassFilter = musicSource.gameObject.AddComponent<AudioLowPassFilter>();
          _lowPassFilter.enabled = false;*/

     /*   _isMusicMuted = PlayerPrefs.GetInt("isMusicMuted", 0) == 1;
        _isSfxMuted = PlayerPrefs.GetInt("isSfxMuted", 0) == 1;

        if (_isMusicMuted) MuteMusic(true);
        if (_isSfxMuted) MuteSfx(true);*/
    }

    private void Update()
    {
      /*  if (Input.GetKeyDown(KeyCode.M))
        {
            if (!_isMuted)
            {
                MuteAllVolume();
                Debug.Log("Muteada");
            }
            else
            {
                UnMuteAllVolume();
            }
        }*/
      /*  if (Input.GetKeyUp(KeyCode.A))
        {
            RestoredMusic();
        }*/
    }
    public void MusicSelector()
    {
        if (_gameMusic == null || _gameMusic.Length == 0)
        {
            Debug.LogWarning("No hay música asignada en _gameMusic");
            return;
        }

        int randomIndex;

        if (_gameMusic.Length == 1)
        {
            randomIndex = 0;
        }
        else
        {

            do
            {
                randomIndex = Random.Range(0, _gameMusic.Length);
            } while (randomIndex == lastMusicIndex);
        }

        lastMusicIndex = randomIndex;

        musicSource.clip = _gameMusic[randomIndex];
        musicSource.Play();

        Debug.Log($"AudioManager: Reproduciendo [{randomIndex}] {_gameMusic[randomIndex].name}");
    }

    private IEnumerator PlayMusicLoop()
    {
        MusicSelector();
        yield return FadeIn(musicSource, 2f, 5f);
        while (true)
        {
            yield return new WaitUntil(() => !musicSource.isPlaying);

            yield return FadeOut(musicSource, 1.5f); 
            MusicSelector();
            yield return FadeIn(musicSource, .5f, 1f); 
        }
    }

    public IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.volume = 0;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        audioSource.volume = 0;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, targetVolume, t / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }


    public void VolumeMusic(float sliderMusic)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderMusic) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderMusic);
      // Debug.Log("Volumen música prefs: "+ sliderMusic);
    }

    public void VolumeSFX(float sliderSFX)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderSFX) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sliderSFX);
      //  Debug.Log("Volumen música prefs: " + sliderSFX);
    }

   /* public void MuteMusic(bool isMusicMuted)
    {
        _isMusicMuted = isMusicMuted;

        if (isMusicMuted)
        {
            audioMixer.SetFloat("MusicVolume", -80f);

            Debug.Log("Música muteada");
        }
        else
        {
            // Restaurar volumen guardado en prefs
            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            VolumeMusic(savedMusicVolume);
            Debug.Log("Música desmuteada");
        }

        PlayerPrefs.SetInt("isMusicMuted", isMusicMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void MuteSfx(bool isSfxMuted)
    {
        _isSfxMuted = isSfxMuted;

        if (isSfxMuted)
        {
            audioMixer.SetFloat("SFXVolume", -80f);
            Debug.Log("Sfx muteada");
        }
        else
        {
            // Restaurar volumen guardado en prefs
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

            VolumeSFX(savedSFXVolume);
            Debug.Log("Sfx desmuteada");
        }

        PlayerPrefs.SetInt("isSfxMuted", isSfxMuted ? 1 : 0);
        PlayerPrefs.Save();
    }*/

 /*   public void DistoredMusic()
    {
        _lowPassFilter.enabled = true;
        _distortionFilter.distortionLevel = .7f;
        audioMixer.SetFloat("LowPassCutOff", 10000f);

  
        musicSource.pitch = 0.75f;
        Debug.Log("Music distored");
    }

    public void RestoredMusic()
    {
        _distortionFilter.distortionLevel = 0;
        _lowPassFilter.enabled = false;
        audioMixer.SetFloat("LowPassCutOff", 22000f); 
        musicSource.pitch = 1f;
        Debug.Log("Music restored");
    }*/

}
