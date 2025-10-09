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
            // Verificar si el nivel está desbloqueado
            bool isUnlocked = i <= _unlockedLevels;

            // Configurar interacción
            _levelButtons[i].interactable = isUnlocked;

            // SOLUCIÓN: Desactivar navegación para niveles bloqueados
            Navigation nav = _levelButtons[i].navigation;
            nav.mode = isUnlocked ? Navigation.Mode.Automatic : Navigation.Mode.None;
            _levelButtons[i].navigation = nav;

            // Opcional: Cambiar visual de botones bloqueados
            // Puedes descomentar esto si quieres que se vean diferentes
            /*
            if (!isUnlocked)
            {
                // Cambiar alpha o color
                var colors = _levelButtons[i].colors;
                colors.normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                _levelButtons[i].colors = colors;
            }
            */
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