using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeFontSize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _txt;
    private Vector3 _newScale;
    private Vector3 _normalScale;
    void Start()
    {
        _normalScale = _txt.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            _newScale = new Vector3(1.2f, 1.2f, 1.2f);
            _txt.transform.localScale = _newScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _txt.transform.localScale = _normalScale;
    }
}
