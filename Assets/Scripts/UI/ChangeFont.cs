using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeFont : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_FontAsset defaultFont;
    public TMP_FontAsset hoverFont;
    public TextMeshProUGUI tmpText;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        if (textMesh != null && defaultFont != null)
            textMesh.font = defaultFont;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMesh != null && hoverFont != null)
            textMesh.font = hoverFont;
            tmpText.characterSpacing = 10f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMesh != null && defaultFont != null)
            textMesh.font = defaultFont;
            tmpText.characterSpacing = 0f;
    }
}
