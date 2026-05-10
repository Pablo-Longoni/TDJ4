using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    public Button[] buttons;
    private int currentIndex = 0;
    private bool isActive = true; // NUEVO

    void Start()
    {
        HighlightButton();
    }

    void Update()
    {
        if (!isActive) return; // NUEVO: No ejecutar si está desactivado
        if (Gamepad.current == null) return;

        if (Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            currentIndex = (currentIndex + 1) % buttons.Length;
            HighlightButton();
        }

        if (Gamepad.current.dpad.up.wasPressedThisFrame)
        {
            currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
            HighlightButton();
        }

        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            buttons[currentIndex].onClick.Invoke();
        }
    }

    void HighlightButton()
    {
        if (!isActive) return; // NUEVO
        buttons[currentIndex].Select();
    }

    // NUEVO: Métodos públicos para activar/desactivar
    public void SetActive(bool active)
    {
        isActive = active;
        Debug.Log($"[MenuNavigator] Estado: {(active ? "ACTIVO" : "DESACTIVADO")}");
    }
}