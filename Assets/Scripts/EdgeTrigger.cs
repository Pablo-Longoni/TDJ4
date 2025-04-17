using UnityEngine;
using System.Collections;
public class EdgeTrigger : MonoBehaviour
{
    public CubeRotation cubeRotation; // Asignalo desde el Inspector
    private bool _alreadyTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyTriggered) return;

        if (other.CompareTag("Player"))
        {
            _alreadyTriggered = true;

            if (CompareTag("Rotacion180"))
            {
                cubeRotation._rotationTurn = 180f;
                Debug.Log("Trigger con etiqueta 'Rotacion180': rotación a 180°.");
            }
            else if (CompareTag("Rotacion90"))
            {
                cubeRotation._rotationTurn = 90f;
                Debug.Log("Trigger con etiqueta 'Rotacion90': rotación a 90°.");
            }
            StartCoroutine(ResetTrigger());
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            cubeRotation._rotationTurn = 90f;
            Debug.Log("Trigger con etiqueta 'Rotacion90': rotación a 90°.");
        }

    }

    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(1f); // o lo que dure tu rotación
        _alreadyTriggered = false;
    }
}

