using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeButton : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private Vector3 _scale = new Vector3(1.2f, 1.2f, 1.2f);

    private Vector3 _normalScale;

    void Start()
    {
        _normalScale = _button.transform.localScale;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_button.interactable)
            _button.transform.localScale = _scale;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        _button.transform.localScale = _normalScale;
    }


    public void OnSelect(BaseEventData eventData)
    {
        if (_button.interactable)
            _button.transform.localScale = _scale;
    }


    public void OnDeselect(BaseEventData eventData)
    {
        _button.transform.localScale = _normalScale;
    }
}
