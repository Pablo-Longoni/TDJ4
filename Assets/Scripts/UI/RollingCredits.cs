using UnityEngine;

public class RollingCredits : MonoBehaviour
{
    [SerializeField] private RectTransform creditsText;   // El texto (content)
    [SerializeField] private RectTransform viewport;      // El área visible (ej. un panel con Mask)
    [SerializeField] private float scrollSpeed = 150f;

    private float startY;
    private float endY;

    void Start()
    {

        startY = -creditsText.rect.height - 50f;


        endY = viewport.rect.height + 290f;

        creditsText.anchoredPosition = new Vector2(creditsText.anchoredPosition.x, startY);
    }

    void Update()
    {
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsText.anchoredPosition.y >= endY)
        {
            creditsText.anchoredPosition = new Vector2(creditsText.anchoredPosition.x, startY);
        }
    }
}
