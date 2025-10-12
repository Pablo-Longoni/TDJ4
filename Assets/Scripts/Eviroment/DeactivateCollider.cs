using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeactivateCollider : MonoBehaviour
{
    [SerializeField] public Collider _targetCollider;
    [SerializeField] public ParticleSystem _ripplePrefab;
    [SerializeField] public ParticleSystem _waterSplashPrefab;
    [SerializeField] public GameObject _playerInput;
    private bool _isTriggered;

    private Quaternion _rotation = Quaternion.Euler(-90,0,0);
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isTriggered)
        {
            _targetCollider.enabled = false;
            _isTriggered = true;

            _playerInput.SetActive(false);

            Instantiate(_ripplePrefab, _targetCollider.transform.position, Quaternion.identity);
            Instantiate(_waterSplashPrefab, _targetCollider.transform.position, _rotation);

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
        yield return new WaitForSeconds(.7f);

        _playerInput.SetActive(true);

        _targetCollider.enabled = true;
        _isTriggered = false;
       
    }
}
