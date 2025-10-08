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

    // NUEVO: Referencia al Canvas principal (el que contiene los botones del juego)
    [SerializeField] private Canvas mainCanvas;

    // Lista para guardar los botones que estaban activos antes de pausar
    private List<Button> mainCanvasButtons = new List<Button>();

    // NUEVO: Propiedad para que otros scripts sepan si está pausado
    public static bool IsPaused { get; private set; } = false;

    void Awake()
    {
        _inputActions = new PlayerControls();
        _inputActions.UI.MenuOpenClose.performed += ctx => TogglePause();
    }

    // NUEVO: Método para obtener las acciones del jugador desde otros scripts
    public PlayerControls GetInputActions()
    {
        return _inputActions;
    }

    void OnEnable()
    {
        _inputActions.Enable();
    }

    void OnDisable()
    {
        _inputActions.Disable();
    }

    void Start()
    {
        _transitionAnimator = GetComponentInChildren<Animator>();

        // NUEVO: Si no asignaste el Canvas manualmente, buscarlo
        if (mainCanvas == null)
        {
            mainCanvas = GetComponent<Canvas>();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        IsPaused = isPaused; // Actualizar variable estática

        if (isPaused)
        {
            // Activar panel de pausa
            pausePanel.SetActive(true);
            Time.timeScale = 0f;

            // NUEVO: Desactivar todos los botones del canvas principal
            DisableMainCanvasButtons();

            // NUEVO: Desactivar las acciones del gameplay (Player, etc.)
            DisableGameplayInputs();
        }
        else
        {
            // Desactivar panel de pausa
            pausePanel.SetActive(false);
            Time.timeScale = 1f;

            // NUEVO: Reactivar todos los botones del canvas principal
            EnableMainCanvasButtons();

            // NUEVO: Reactivar las acciones del gameplay
            EnableGameplayInputs();
        }
    }

    // NUEVO: Método para desactivar botones del canvas principal
    private void DisableMainCanvasButtons()
    {
        if (mainCanvas == null) return;

        // Limpiar la lista
        mainCanvasButtons.Clear();

        // Obtener todos los botones hijos del canvas
        Button[] allButtons = mainCanvas.GetComponentsInChildren<Button>();

        foreach (Button button in allButtons)
        {
            // Solo desactivar botones que NO estén dentro del panel de pausa
            if (!button.transform.IsChildOf(pausePanel.transform) && button.interactable)
            {
                mainCanvasButtons.Add(button);
                button.interactable = false;

                // También desactivar navegación
                Navigation nav = button.navigation;
                nav.mode = Navigation.Mode.None;
                button.navigation = nav;
            }
        }
    }

    // NUEVO: Método para reactivar botones del canvas principal
    private void EnableMainCanvasButtons()
    {
        foreach (Button button in mainCanvasButtons)
        {
            if (button != null)
            {
                button.interactable = true;

                // Restaurar navegación
                Navigation nav = button.navigation;
                nav.mode = Navigation.Mode.Automatic;
                button.navigation = nav;
            }
        }

        mainCanvasButtons.Clear();
    }

    // NUEVO: Desactivar inputs del gameplay cuando se pausa
    private void DisableGameplayInputs()
    {
        // Desactivar completamente todas las acciones del gameplay
        _inputActions.Player.Disable();
        _inputActions.Gameplay.Disable();
        _inputActions.Camera.Disable();

        // Mantener solo UI activo
        _inputActions.UI.Enable();

        Debug.Log("Inputs de gameplay desactivados - Solo UI activo");
    }

    // NUEVO: Reactivar inputs del gameplay cuando se despausa
    private void EnableGameplayInputs()
    {
        // Reactivar todas las acciones del gameplay
        _inputActions.Player.Enable();
        _inputActions.Gameplay.Enable();
        _inputActions.Camera.Enable();

        Debug.Log("Inputs de gameplay reactivados");
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