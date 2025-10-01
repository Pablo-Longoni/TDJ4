using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    private int currentIndex = 0;

    private void Start()
    {

        if (!levelButtons[currentIndex].interactable)
            Next();
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


        if (levelButtons[currentIndex].interactable)
            levelButtons[currentIndex].onClick.Invoke();
        else
            Debug.Log("Nivel bloqueado");
    }

    private void Next()
    {
        int startIndex = currentIndex;
        do
        {
            currentIndex = (currentIndex + 1) % levelButtons.Length;
        }
        while (!levelButtons[currentIndex].interactable && currentIndex != startIndex);

        HighlightButton();
    }

    private void Previous()
    {
        int startIndex = currentIndex;
        do
        {
            currentIndex = (currentIndex - 1 + levelButtons.Length) % levelButtons.Length;
        }
        while (!levelButtons[currentIndex].interactable && currentIndex != startIndex);

        HighlightButton();
    }

    private void HighlightButton()
    {
        levelButtons[currentIndex].Select();
    }
}
