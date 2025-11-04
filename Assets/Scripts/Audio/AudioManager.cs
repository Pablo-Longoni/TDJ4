using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using static Unity.VisualScripting.Member;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
 //   [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource soundSource;
    [SerializeField] private AudioMixer audioMixer;

   [Header("Music")]
    public AudioClip [] _gameMusic;
    public AudioClip _menuMusic;
  
    [Header("Sound Figures")]
    public AudioClip _turning;
 
    [Header("Sound Player")]
    public AudioClip _playerLand;

    [Header("Sound Teleport")]
    public AudioClip _triggerTeleport;
    public AudioClip _teleportPlayer;

    [Header("Sound Portal and Pressed")]
    public AudioClip _portal;
    public AudioClip _activatePortal;
    public AudioClip _pressedSound;
    public AudioClip _releasedSound;

    [Header("Sound Other")]
    public AudioClip _splashWater;
    public AudioClip _flip;

    private int lastMusicIndex;
    private AudioDistortionFilter _distortionFilter;
    private AudioLowPassFilter _lowPassFilter;

    public bool _isMusicMuted;
    public bool _isSfxMuted;

    //new music
    [Header("Sources")]
    [SerializeField] private AudioSource musicSourceA;
    [SerializeField] private AudioSource musicSourceB;
 //   [SerializeField] private AudioSource sfxSource;

    private AudioSource currentSource;
    private AudioSource nextSource;

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
        if (_gameMusic == null || _gameMusic.Length == 0) return;

        // Configurar los dos sources
        musicSourceA.loop = false;
        musicSourceB.loop = false;

        currentSource = musicSourceA;
        nextSource = musicSourceB;

        StartCoroutine(PlayMusicLoop());

       /* _distortionFilter = musicSource.gameObject.AddComponent<AudioDistortionFilter>();
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
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            RestoredMusic();
        }
        if (Input.GetKeyUp(KeyCode.Y))
        {
            DistoredMusic();
        }*/
    }

    private AudioClip GetNextTrack()
    {
        if (_gameMusic.Length == 1) return _gameMusic[0];

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, _gameMusic.Length);
        } while (randomIndex == lastMusicIndex);

        lastMusicIndex = randomIndex;
        return _gameMusic[randomIndex];
    }

  /*  public void MusicSelector()
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
    }*/

    private IEnumerator PlayMusicLoop()
    {
        currentSource.clip = GetNextTrack();
        currentSource.volume = 0;
        currentSource.Play();

        yield return FadeIn(currentSource, 2f, 1f);

        while (true)
        {
            float remainingTime = currentSource.clip.length - currentSource.time;
            if (remainingTime <= 10f) // empieza el crossfade 3 segundos antes de terminar
            {
                nextSource.clip = GetNextTrack();
                nextSource.volume = 0;
                nextSource.Play();

                yield return CrossFade(currentSource, nextSource, 3f);

                // intercambiar referencias
                var temp = currentSource;
                currentSource = nextSource;
                nextSource = temp;
            }
            yield return null;
        }
    }

    private IEnumerator CrossFade(AudioSource from, AudioSource to, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            from.volume = Mathf.Lerp(1f, 0f, t);
            to.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        from.Stop();
        from.volume = 0;
        to.volume = 1f;
    }

  /*  public IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.volume = 0;
    }
    */
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

  /*  public void DistoredMusic()
    {
        // Activar filtros
        _lowPassFilter.enabled = true;
        _lowPassFilter.cutoffFrequency = 800f; 

        // Eco / Reverb
        var reverb = musicSource.GetComponent<AudioReverbFilter>();
        if (reverb == null)
            reverb = musicSource.gameObject.AddComponent<AudioReverbFilter>();

        reverb.enabled = true;
        reverb.reverbPreset = AudioReverbPreset.Cave; // o Hallway, Cathedral, etc.

        Debug.Log("Music distorted and slowed");
    }

    public void RestoredMusic()
    {
        _lowPassFilter.cutoffFrequency = 22000f;


        var reverb = musicSource.GetComponent<AudioReverbFilter>();
        if (reverb != null)
            reverb.enabled = false;

        Debug.Log("music restored to normal");
    }
  */
}
