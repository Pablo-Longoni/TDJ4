using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
     private Vector3 _newScale;
     private Vector3 _normalScale;
    void Start()
    {
        _normalScale = _button.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_button.interactable)
        {
            _newScale = new Vector3(1.2f, 1.2f, 1.2f);
            _button.transform.localScale = _newScale;
        }
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _button.transform.localScale = _normalScale;
    }
}
