using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
public class PlayButtonSounds : MonoBehaviour, IPointerEnterHandler,
    ISelectHandler,
    IPointerClickHandler,
    ISubmitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlayHoverSound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSound();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayClickSound();
    }

    private void PlayHoverSound()
    {
        if (AudioManager.Instance == null) return;


        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();

        if (button != null && !button.interactable) return;


        AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._hover);
    }

    private void PlayClickSound()
    {
        if (AudioManager.Instance == null) return;


        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();

        if (button != null && !button.interactable) return;

        AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._selected);
    }
}
