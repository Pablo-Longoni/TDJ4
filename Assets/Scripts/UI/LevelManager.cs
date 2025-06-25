using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static int _currentLevel;
    public static int _unlockedLevels;
    public Button[] _levelButtons;


    public void onClickLevel(int level)
    {
        _currentLevel = level;
        PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
        PlayerPrefs.Save();
        SceneManager.LoadScene(level);
    }

    void Start()
    {
        _unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        Debug.Log("Unlocked levels al iniciar: " + _unlockedLevels);
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            if (i <= _unlockedLevels)
            {
                _levelButtons[i].interactable = true;
            }
        }

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}