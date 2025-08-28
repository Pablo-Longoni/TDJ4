using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    public Button[] buttons;
    private int currentIndex = 0;

    void Start()
    {
        HighlightButton();
    }

    void Update()
    {
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
        buttons[currentIndex].Select();
    }
}