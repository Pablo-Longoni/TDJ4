using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    private int currentIndex = 0;

    private void Start()
    {
        HighlightButton();
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Vector2 input = context.ReadValue<Vector2>();

        if (input.x > 0.5f || input.y < -0.5f) Next();
        if (input.x < -0.5f || input.y > 0.5f) Previous();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        levelButtons[currentIndex].onClick.Invoke();
    }

    private void Next()
    {
        currentIndex = (currentIndex + 1) % levelButtons.Length;
        HighlightButton();
    }

    private void Previous()
    {
        currentIndex = (currentIndex - 1 + levelButtons.Length) % levelButtons.Length;
        HighlightButton();
    }

    private void HighlightButton()
    {
        levelButtons[currentIndex].Select();
    }
}
