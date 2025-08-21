using System.Collections;
using UnityEngine;

public class DeactivateCollider : MonoBehaviour
{
    [SerializeField] private Collider _targetCollider;

    private bool _isTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isTriggered)
        {
            _targetCollider.enabled = false;
            _isTriggered = true;
            StartCoroutine(EnabledCollider());
            Debug.Log("Entro a la sombra");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _isTriggered)
        {
            _targetCollider.enabled = true;
            _isTriggered = false;
            Debug.Log("Salio de la sombra");
        }
    }

    private IEnumerator EnabledCollider()
    {
        yield return new WaitForSeconds(.5f);
        _targetCollider.enabled = true;
    }
}
