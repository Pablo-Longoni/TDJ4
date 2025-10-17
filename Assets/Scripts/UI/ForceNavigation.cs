using UnityEngine;
using UnityEngine.EventSystems;

public class ForceNavigationOnStart : MonoBehaviour
{
  void Start()
  {
    // Forzar la selección después de un frame
    StartCoroutine(SelectFirstButton());
  }

  private System.Collections.IEnumerator SelectFirstButton()
  {
    // Espera un frame para que todo se inicialice
    yield return null;

    // Fuerza la selección del objeto que ya está configurado en EventSystem
    if (EventSystem.current != null && EventSystem.current.firstSelectedGameObject != null)
    {
      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }
  }
}