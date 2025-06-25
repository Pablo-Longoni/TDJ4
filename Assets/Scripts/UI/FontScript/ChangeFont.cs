using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeFont : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_FontAsset defaultFont;
    public TMP_FontAsset hoverFont;
    public TextMeshProUGUI [] tmpText;
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
        if (textMesh != null && hoverFont != null)
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

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMesh != null && hoverFont != null)
            
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
