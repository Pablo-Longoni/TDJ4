using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ChangeFontSize : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{
    [SerializeField] private TextMeshProUGUI _txt;
    [SerializeField] private float _rumbleLow = 0.2f;
    [SerializeField] private float _rumbleHigh = 0.4f;
    [SerializeField] private float _rumbleDuration = 0.15f;

    private Vector3 _newScale;
    private Vector3 _normalScale;

    void Start()
    {
        _normalScale = _txt.transform.localScale;
        _newScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ApplySelectEffects();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _txt.transform.localScale = _normalScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        ApplySelectEffects();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _txt.transform.localScale = _normalScale;
    }

    private void ApplySelectEffects()
    {
        _txt.transform.localScale = _newScale;


        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(_rumbleLow, _rumbleHigh);
            CancelInvoke(nameof(StopRumble));
            Invoke(nameof(StopRumble), _rumbleDuration);
        }
    }

    private void StopRumble()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}
