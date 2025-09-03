using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuStart : MonoBehaviour
{
    public Button firstButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }
}