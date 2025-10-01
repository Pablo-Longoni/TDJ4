using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartUIFeedback : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _fixedIcon;   // la imagen fija (el cuadrado con ícono)
    [SerializeField] private Image _circleFill;  // la imagen circular con Fill

    [Header("Config")]
    [SerializeField] private float _holdTime = 3f;

    private float _pressStart = -1f;

    void Start()
    {
        if (_circleFill != null)
        {
            _circleFill.gameObject.SetActive(false);
            _circleFill.fillAmount = 0f;
        }
        if (_fixedIcon != null)
            _fixedIcon.gameObject.SetActive(true);
    }

    void Update()
    {
        if (_circleFill == null) return;

        bool keyboardPressed = Keyboard.current != null && Keyboard.current.rKey.isPressed;
        bool gamepadPressed = Gamepad.current != null && Gamepad.current.buttonEast.isPressed;
        bool anyPressed = keyboardPressed || gamepadPressed;

        // Cuando empieza a apretar
        if ((Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            _pressStart = Time.time;

            // Cambiar imágenes
            if (_fixedIcon != null) _fixedIcon.gameObject.SetActive(false);
            _circleFill.gameObject.SetActive(true);
            _circleFill.fillAmount = 0f;
        }

        // Cuando suelta antes de tiempo
        if ((Keyboard.current != null && Keyboard.current.rKey.wasReleasedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonEast.wasReleasedThisFrame))
        {
            _pressStart = -1f;

            // Volver a mostrar fija
            if (_fixedIcon != null) _fixedIcon.gameObject.SetActive(true);
            _circleFill.gameObject.SetActive(false);
        }

        // Mientras mantiene presionado
        if (_pressStart > 0f && anyPressed)
        {
            float progress = (Time.time - _pressStart) / _holdTime;
            _circleFill.fillAmount = Mathf.Clamp01(progress);

            if (progress >= 1f)
            {
                RestartLevel();
            }
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
