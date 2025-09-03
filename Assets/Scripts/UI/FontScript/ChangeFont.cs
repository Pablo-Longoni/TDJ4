using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeFont : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TMP_FontAsset defaultFont;
    public TMP_FontAsset hoverFont;
    public TextMeshProUGUI[] tmpText;
    private TextMeshProUGUI textMesh;

    public bool isLocked = false;

    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI text in tmpText)
        {
            if (text != null && defaultFont != null)
                text.font = defaultFont;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ApplyHoverStyle();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ApplyDefaultStyle();
    }


    public void OnSelect(BaseEventData eventData)
    {
        ApplyHoverStyle();
    }


    public void OnDeselect(BaseEventData eventData)
    {
        ApplyDefaultStyle();
    }

    private void ApplyHoverStyle()
    {
        if (isLocked) return;

        foreach (TextMeshProUGUI text in tmpText)
        {
            if (text != null)
            {
                if (hoverFont != null) text.font = hoverFont;
                text.characterSpacing = 10f;
            }
        }
    }

    private void ApplyDefaultStyle()
    {
        foreach (TextMeshProUGUI text in tmpText)
        {
            if (text != null)
            {
                if (defaultFont != null) text.font = defaultFont;
                text.characterSpacing = 0f;
            }
        }
    }
}
