using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;
public class UIfromPause : MonoBehaviour
{
    private Animator _transitionAnimator;
    public float _transitionTime;
    public GameObject pausePanel;
    private bool isPaused = false;

    [SerializeField] private GameObject [] _tabs;
    [SerializeField] private Image [] _buttonTab;
    public Vector2 _inactiveButtonSize, _activeButtonSize;

    [SerializeField] private GameObject _gamePadControls;
    [SerializeField] private GameObject _keyboardControls;

    void Start()
    {
        _transitionAnimator = GetComponentInChildren<Animator>();
    }

  /*  public void TogglePause()
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
    */

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
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
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
    }
    */
   /* public void SettingsControls()
    {
        TogglePause();
        _controls.gameObject.SetActive(true);
        _volume.gameObject.SetActive(false);
    }

    public void SettingsVolume()
    {
        TogglePause();
        _controls.gameObject.SetActive(false);
        _volume.gameObject.SetActive(true);
    }*/

    public void SwitchTabs(int TabId)
    {
        foreach (GameObject i in _tabs)
        {
            i.SetActive(false);
        }
        _tabs[TabId].SetActive(true);

        foreach (Image i in _buttonTab)
        {
         //   i.color = Color.white;
            i.rectTransform.sizeDelta = _inactiveButtonSize;
        }
      //  _buttonTab[TabId].color = Color.gray;
        _buttonTab[TabId].rectTransform.sizeDelta = _activeButtonSize;

    }

    public void SwitchToKeyboardImage()
    {
        _gamePadControls.SetActive(false);
        _keyboardControls.SetActive(true);
    }

    public void SwitchToGamepadImage()
    {
        _keyboardControls.SetActive(false);
        _gamePadControls.SetActive(true);
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
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PreviousLvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
