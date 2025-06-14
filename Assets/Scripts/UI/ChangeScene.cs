

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private Animator _transitionAnimator;
    public float _transitionTime;
    public GameObject pausePanel;
    private bool isPaused = false;

    void Start()
    {
        _transitionAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            TogglePause();
        }
    }

    void TogglePause()
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public void GoToSandBox()
    {
        SceneManager.LoadScene("LevelTest");
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
