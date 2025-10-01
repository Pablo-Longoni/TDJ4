using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.EventSystems;

public class StageUnlock : MonoBehaviour
{
    [Header("Cámaras")]
    [SerializeField] private CinemachineCamera _stageCamera;
    [SerializeField] private CinemachineCamera _canvasCamera;

    [Header("Canvas")]
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Canvas[] _selectorCanvases;

    [Header("Cube")]
    [SerializeField] private CubeMenu _cubeMenu;
    [SerializeField] private GameObject _cube;
    [SerializeField] private Renderer _cubeRenderer;

    [Header("Botones")]
    [SerializeField] private GameObject firstLevelButton;
    [SerializeField] private Button menuPlayButton;

    [Header("Scripts")]
    [SerializeField] private MenuNavigator menuNavigator;

    [Header("Fade")]
    public float _fadeDuration = 1f;
    private Material _cubeMaterial;
    private Color originalColor;

    private bool _isCanvasCamera;

    void Start()
    {
        _isCanvasCamera = true;
        _cubeMaterial = _cubeRenderer.material;
        originalColor = _cubeMaterial.color;

        if (_mainCanvas != null) _mainCanvas.gameObject.SetActive(true);
        foreach (Canvas c in _selectorCanvases)
            c.gameObject.SetActive(false);

        if (menuPlayButton != null)
            EventSystem.current.SetSelectedGameObject(menuPlayButton.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void GoToSelector()
    {
        _cubeMenu.StopRotation();
        _cube.transform.rotation = Quaternion.identity;
        _isCanvasCamera = false;

        // CRÍTICO: Desactivar MenuNavigator
        if (menuNavigator != null)
        {
            menuNavigator.enabled = false;
            Debug.Log("[StageUnlock] MenuNavigator DESACTIVADO");
        }

        // Apago menú principal
        if (_mainCanvas != null)
            _mainCanvas.gameObject.SetActive(false);

        // Enciendo canvases del selector
        foreach (Canvas c in _selectorCanvases)
            c.gameObject.SetActive(true);

        _cubeMenu.onStage = true;
        _canvasCamera.Priority = 1;
        _stageCamera.Priority = 2;

        StartCoroutine(SelectFirstCoroutine());
    }

    IEnumerator SelectFirstCoroutine()
    {
        // Limpiar la selección anterior
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();

        // Esperar frames adicionales
        yield return null;

        // Asignar el primer botón del selector
        if (firstLevelButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstLevelButton);
        }

        yield return null;

        Debug.Log("[StageUnlock] Selector activo. Botón seleccionado: " +
            (EventSystem.current.currentSelectedGameObject != null
            ? EventSystem.current.currentSelectedGameObject.name
            : "null"));
    }

    public void GoToMenuCanvas()
    {
        _isCanvasCamera = true;

        // Apago selector canvases
        foreach (Canvas c in _selectorCanvases)
            c.gameObject.SetActive(false);

        // Prendo menú principal
        if (_mainCanvas != null)
            _mainCanvas.gameObject.SetActive(true);

        // CRÍTICO: Reactivar MenuNavigator
        if (menuNavigator != null)
        {
            menuNavigator.enabled = true;
            Debug.Log("[StageUnlock] MenuNavigator ACTIVADO");
        }

        _cubeMenu.onStage = false;
        _canvasCamera.Priority = 2;
        _stageCamera.Priority = 1;

        StartCoroutine(SelectPlayCoroutine());
    }

    IEnumerator SelectPlayCoroutine()
    {
        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(null);
        yield return null;

        if (menuPlayButton != null)
            EventSystem.current.SetSelectedGameObject(menuPlayButton.gameObject);

        Debug.Log("[StageUnlock] Volvió al menú, botón seleccionado: " +
            (EventSystem.current.currentSelectedGameObject != null
            ? EventSystem.current.currentSelectedGameObject.name
            : "null"));
    }

    // Fades
    public void FadeOut()
    {
        StartCoroutine(FadeTo(1f));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeTo(0f));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = _cubeMaterial.color.a;
        float time = 0f;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
            Color newColor = _cubeMaterial.color;
            newColor.a = alpha;
            _cubeMaterial.color = newColor;
            yield return null;
        }

        Color finalColor = _cubeMaterial.color;
        finalColor.a = targetAlpha;
        _cubeMaterial.color = finalColor;
    }
}