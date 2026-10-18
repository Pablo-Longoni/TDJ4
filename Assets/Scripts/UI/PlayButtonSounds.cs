using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
public class PlayButtonSounds : MonoBehaviour, IPointerEnterHandler/*, ISelectHandler*/
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._selected);
    }

  /*  public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._hover);
    }*/
}
