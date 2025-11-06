using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CubeMenu : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public bool onStage = false;
    private Coroutine _rotationCoroutine;

    private float _rotateCooldown = 1f;
    private bool _isInCooldown = false;

    [Header("Canvas References")]
    [SerializeField] private GameObject canvasCube;
    [SerializeField] private GameObject canvasCube1;
    [SerializeField] private GameObject canvasCube2;
    [SerializeField] private GameObject canvasCube3;

    [Header("First Buttons to Select")]
    [SerializeField] private GameObject firstButtonCanvasCube;
    [SerializeField] private GameObject firstButtonCanvasCube1;
    [SerializeField] private GameObject firstButtonCanvasCube2;
    [SerializeField] private GameObject firstButtonCanvasCube3;

    [Header("Back Buttons (Fallback)")]
    [SerializeField] private GameObject backButtonCanvasCube;
    [SerializeField] private GameObject backButtonCanvasCube1;
    [SerializeField] private GameObject backButtonCanvasCube2;
    [SerializeField] private GameObject backButtonCanvasCube3;

    private GameObject _currentActiveCanvas;
    private bool _hasInitialized = false;

    private void Start()
    {
        _rotationCoroutine = StartCoroutine(RotateCycle());
    }

    private void OnEnable()
    {
        // Reset al habilitar
        _hasInitialized = false;
        _currentActiveCanvas = null;
    }

    IEnumerator RotateCycle()
    {
        while (true)
        {
            yield return RotateBy(Vector3.forward);
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator RotateBy(Vector3 axis)
    {
        float rotated = 0f;
        while (rotated < 90f)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(axis, step);
            rotated += step;
            yield return null;
        }

        transform.Rotate(axis, 90f - rotated);

        // Actualizar canvas activo después de la rotación
        UpdateActiveCanvas();
    }

    public void StopRotation()
    {
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = null;
        }
    }

    public void RotateLeft()
    {
        if (!_isInCooldown)
        {
            StartCoroutine(RotateBy(Vector3.back));
            StartCoroutine(RotationCooldown());
        }
    }

    public void RotateRight()
    {
        if (!_isInCooldown)
        {
            StartCoroutine(RotateBy(Vector3.forward));
            StartCoroutine(RotationCooldown());
        }
    }
    private void Update()
    {
        // Inicializar solo cuando onStage se activa por primera vez
        if (onStage && !_hasInitialized)
        {
            _hasInitialized = true;
            _currentActiveCanvas = null; // Forzar actualización
            StartCoroutine(InitializeCanvasDelayed());
        }

        // IMPORTANTE: Detectar cuando se desactiva onStage para resetear
        if (!onStage && _hasInitialized)
        {
            ResetCanvasState();
        }

        if (onStage && !_isInCooldown)
        {
            if (Input.GetKeyDown(KeyCode.A) ||
               (Gamepad.current != null && Gamepad.current.leftShoulder.wasPressedThisFrame))
            {
                StartCoroutine(RotateBy(Vector3.back));
                StartCoroutine(RotationCooldown());
            }
            else if (Input.GetKeyDown(KeyCode.D) ||
                    (Gamepad.current != null && Gamepad.current.rightShoulder.wasPressedThisFrame))
            {
                StartCoroutine(RotateBy(Vector3.forward));
                StartCoroutine(RotationCooldown());
            }
        }
    }

    private void ResetCanvasState()
    {
        _hasInitialized = false;
        _currentActiveCanvas = null;

        // Habilitar todos los canvas cuando salimos
        EnableAllCanvasInteraction();
    }

    private void EnableAllCanvasInteraction()
    {
        EnableCanvasOnly(canvasCube);
        EnableCanvasOnly(canvasCube1);
        EnableCanvasOnly(canvasCube2);
        EnableCanvasOnly(canvasCube3);
    }

    private void EnableCanvasOnly(GameObject canvas)
    {
        if (canvas != null)
        {
            CanvasGroup group = canvas.GetComponent<CanvasGroup>();
            if (group != null)
            {
                group.interactable = true;
                group.blocksRaycasts = true;
            }
        }
    }

    private IEnumerator InitializeCanvasDelayed()
    {
        // Esperar a que StageUnlock termine su inicialización
        yield return new WaitForSeconds(0.1f);
        UpdateActiveCanvas();
    }

    private void UpdateActiveCanvas()
    {
        if (!onStage) return;

        // Determinar qué cara está de frente basado en la rotación del cubo
        float zRotation = transform.eulerAngles.z;
        zRotation = (zRotation + 360) % 360; // Normalizar a 0-360

        GameObject newActiveCanvas = null;
        GameObject firstButton = null;
        GameObject backButton = null;

        // Determinar canvas activo según rotación
        if (zRotation >= 315 || zRotation < 45)
        {
            newActiveCanvas = canvasCube;
            firstButton = firstButtonCanvasCube;
            backButton = backButtonCanvasCube;
        }
        else if (zRotation >= 45 && zRotation < 135)
        {
            newActiveCanvas = canvasCube1;
            firstButton = firstButtonCanvasCube1;
            backButton = backButtonCanvasCube1;
        }
        else if (zRotation >= 135 && zRotation < 225)
        {
            newActiveCanvas = canvasCube2;
            firstButton = firstButtonCanvasCube2;
            backButton = backButtonCanvasCube2;
        }
        else if (zRotation >= 225 && zRotation < 315)
        {
            newActiveCanvas = canvasCube3;
            firstButton = firstButtonCanvasCube3;
            backButton = backButtonCanvasCube3;
        }

        // Actualizar siempre si no hay canvas activo previo
        bool shouldUpdate = (_currentActiveCanvas == null) || (newActiveCanvas != _currentActiveCanvas);

        if (shouldUpdate)
        {
            DisableAllCanvasInteraction();
            EnableCanvasInteraction(newActiveCanvas, firstButton, backButton);
            _currentActiveCanvas = newActiveCanvas;
        }
    }

    private void DisableAllCanvasInteraction()
    {
        DisableCanvas(canvasCube);
        DisableCanvas(canvasCube1);
        DisableCanvas(canvasCube2);
        DisableCanvas(canvasCube3);
    }

    private void DisableCanvas(GameObject canvas)
    {
        if (canvas != null)
        {
            CanvasGroup group = canvas.GetComponent<CanvasGroup>();
            if (group == null) group = canvas.AddComponent<CanvasGroup>();

            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    private void EnableCanvasInteraction(GameObject canvas, GameObject firstButton, GameObject backButton)
    {
        if (canvas != null)
        {
            CanvasGroup group = canvas.GetComponent<CanvasGroup>();
            if (group == null) group = canvas.AddComponent<CanvasGroup>();

            group.interactable = true;
            group.blocksRaycasts = true;

            // Determinar qué botón seleccionar
            GameObject buttonToSelect = GetSelectableButton(firstButton, backButton);

            // NO seleccionar el botón aquí si es la primera vez
            // StageUnlock ya lo hizo
            if (_currentActiveCanvas != null && buttonToSelect != null)
            {
                // Solo re-seleccionar cuando cambiamos de cara
                StartCoroutine(SelectButtonNextFrame(buttonToSelect));
            }
        }
    }

    private GameObject GetSelectableButton(GameObject firstButton, GameObject backButton)
    {
        // Verificar si el primer botón está interactuable
        if (firstButton != null)
        {
            Button button = firstButton.GetComponent<Button>();
            if (button != null && button.interactable)
            {
                return firstButton;
            }
        }

        // Si no está disponible, usar el botón de volver
        return backButton;
    }

    private IEnumerator SelectButtonNextFrame(GameObject button)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(button);
    }

    private IEnumerator RotationCooldown()
    {
        _isInCooldown = true;
        yield return new WaitForSeconds(_rotateCooldown);
        _isInCooldown = false;
    }
}