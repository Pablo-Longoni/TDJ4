using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedDebug : MonoBehaviour
{
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Debug.Log("Seleccionado: " + EventSystem.current.currentSelectedGameObject.name);
        }
    }
}