using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class DeactivateCollider : MonoBehaviour
{
    [SerializeField] public Collider _targetCollider;
    [SerializeField] public Collider _groundCollider;
    [SerializeField] public ParticleSystem _ripplePrefab;
    [SerializeField] public ParticleSystem _waterSplashPrefab;
    [SerializeField] public GameObject _playerInput, _Input;
    [SerializeField] private PlayerAnimationController _playerAnimator;
    private bool _isTriggered;

    private Quaternion _rotation = Quaternion.Euler(-90,0,0);
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isTriggered)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._splashWater);
            }
       
            _targetCollider.enabled = false;
            _groundCollider.enabled = false;
            _isTriggered = true;

            _playerInput.SetActive(false);
            _Input.SetActive(false);

            _playerAnimator.PlayAnimation("Fall");

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
            _groundCollider.enabled = true;
            _isTriggered = false;
            Debug.Log("Salio de la sombra");
        }
    }

    private IEnumerator EnabledCollider()
    {
        yield return new WaitForSeconds(.7f);
        _playerInput.SetActive(true);
        _Input.SetActive(true);
        _targetCollider.enabled = true;
        _groundCollider.enabled = true;
        _isTriggered = false;
    }
}
