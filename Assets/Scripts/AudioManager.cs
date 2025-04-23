using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource soundSource;

    [Header("Music")]
    public AudioClip _gameMusic;
    public AudioClip _menuMusic;

    [Header("Sound")]
    public AudioClip _turning;
    public AudioClip _portal;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);        

        AudioSource src = GetComponent<AudioSource>();
        src.loop = true;                       
        if (!src.isPlaying)
            src.Play();
    }
    void Start()
    {
     //   SetBackgroundMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetBackgroundMusic()
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
    }
}
