using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    private Animator _transitionAnimator;
    public float _transitionTime;
    public GameObject pausePanel;
    private bool isPaused = false;
    [SerializeField] private GameObject _volume;
    [SerializeField] private GameObject _controls;
    [SerializeField] private GameObject _settings;
    private PlayerControls _inputActions;

    void Awake()
    {
        _inputActions = new PlayerControls();
        _inputActions.UI.MenuOpenClose.performed += ctx => TogglePause();
    }




    void OnEnable()
    {
        _inputActions.UI.Enable();
    }

    void OnDisable()
    {
        _inputActions.UI.Disable();
    }
  
    void Start()
    {
        _transitionAnimator = GetComponentInChildren<Animator>();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }


    public void NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);

        Debug.Log("UnlockedLevels: " + unlockedLevels + " | CurrentLevel: " + currentLevel);

        if (currentLevel >= unlockedLevels)
        {
            unlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels);
            PlayerPrefs.Save();
            Debug.Log("Nuevo nivel desbloqueado: " + unlockedLevels);
        }

        // Actualizar CurrentLevel al siguiente
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        PlayerPrefs.SetInt("CurrentLevel", nextLevel);
        PlayerPrefs.Save();

        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(SceneLoad(nextLevel));
        Time.timeScale = 1f;
        /* if (currentSceneName == "Level3" || currentSceneName == "Level7" || currentSceneName == "Level13")
         {
             StartCoroutine(SceneLoad(0)); // Volver al menú o selector
         }
         else
         {
             StartCoroutine(SceneLoad(nextLevel));
             Time.timeScale = 1f;
         }*/
    }

    public IEnumerator SceneLoad(int sceneIndex)
    {
        _transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

  /*  public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void GoToSandBox()
    {
        SceneManager.LoadScene("LevelTest");
    }*/

    public void SettingsControls()
    {
        TogglePause();
        _controls.SetActive(true);
        _volume.SetActive(false);
        _settings.SetActive(false);
    }

    public void SettingsVolume()
    {
        TogglePause();
        _controls.SetActive(false);
        _volume.SetActive(true);
        _settings.SetActive(false);
    }
    public void Exit()
    {
        Debug.Log("Cerrando el juego...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void NextLvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PreviousLvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
