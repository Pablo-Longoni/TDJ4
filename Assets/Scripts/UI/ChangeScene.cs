
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class ChangeScene : MonoBehaviour
{
    private Animator _transitionAnimator;
    public float _transitionTime;
    public GameObject pausePanel;
    private bool isPaused = false;

    // etapas
    private Dictionary<int, string> firstLevelsByStage = new Dictionary<int, string>()
    {
        { 1, "Level1" },
        { 2, "Level4" },
        { 3, "Level7" }
    };

    //lista de niveles
    private List<string> levelsStage1 = new List<string>() { "Level1", "Level2", "Level3" };
    private List<string> levelsStage2 = new List<string>() { "Level4", "Level5" };
    private List<string> levelsStage3 = new List<string>() { "Level7", "Level9", "Level10", "Level6" };

    void Start()
    {
        _transitionAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = GetNextLevel(currentScene);
        if (nextScene != "")
        {
            StartCoroutine(SceneLoad(nextScene));
        }
        else
        {
            StartCoroutine(SceneLoad("Stages"));
        }
    }

    string GetNextLevel(string currentLevel)
    {
        int index;

        if (levelsStage1.Contains(currentLevel))
        {
            index = levelsStage1.IndexOf(currentLevel);
            if (index < levelsStage1.Count - 1)
                return levelsStage1[index + 1];
        }
        else if (levelsStage2.Contains(currentLevel))
        {
            index = levelsStage2.IndexOf(currentLevel);
            if (index < levelsStage2.Count - 1)
                return levelsStage2[index + 1];
        }
        else if (levelsStage3.Contains(currentLevel))
        {

            index = levelsStage3.IndexOf(currentLevel);
            if (index < levelsStage3.Count - 1)
                return levelsStage3[index + 1];
        }

        return "";
    }

    public IEnumerator SceneLoad(string sceneName)
    {
        _transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void StartButton()
    {
        SceneManager.LoadScene("Stages");
    }

    public void StageSelector(int stageNumber)
    {
        if (firstLevelsByStage.ContainsKey(stageNumber))
        {
            SceneManager.LoadScene(firstLevelsByStage[stageNumber]);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void GoToStage()
    {
        SceneManager.LoadScene("Stages");
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
}
