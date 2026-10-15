using UnityEngine;

public class DeactivateCollider : MonoBehaviour
{
    [SerializeField] private Collider _targetCollider;
    [SerializeField] private Collider _groundCollider;

    [SerializeField] private ParticleSystem _ripplePrefab;
    [SerializeField] private ParticleSystem _waterSplashPrefab;

    [SerializeField] private PlayerMovement _playerInput;
    [SerializeField] private CubeAnimation _playerAnimator;

    [SerializeField] private Material _originalMaterial;
    [SerializeField] private Material _newMaterial;
    [SerializeField] private Renderer _playerRenderer;

    [SerializeField] private PlayerShadow _fakeShadow;

    private bool _isTriggered;

    public bool IsTriggered => _isTriggered;

    private Quaternion _rotation = Quaternion.Euler(-90, 0, 0);

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{name}: entró en el trigger {other.name}");


        if (!other.CompareTag("Player") || _isTriggered)
            return;

        _isTriggered = true;

        // Bloquear el control y borrar el input.
        _playerInput._input = Vector3.zero;
        _playerInput._movementBlocked = true;

        // Detener inmediatamente el movimiento horizontal.
        Rigidbody rb = _playerInput.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }

        // Cambiar el material del jugador.
        if (_playerRenderer != null && _newMaterial != null)
        {
            _playerRenderer.material = _newMaterial;
        }

       /* if(_fakeShadow != null)
        {
            _fakeShadow.ForceDeactivateShadow();
        }*/

        // Sonido.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._splashWater);
        }

        // Desactivar las colisiones necesarias para que caiga.
        if (_targetCollider != null)
            _targetCollider.enabled = false;

        if (_groundCollider != null)
            _groundCollider.enabled = false;

        // Animación de caída.
        _playerAnimator.CallFalling();

        // Efectos.
        if (_ripplePrefab != null)
        {
            Instantiate(_ripplePrefab, _groundCollider.transform.position, Quaternion.identity);
        }

        if (_waterSplashPrefab != null)
        {
            Instantiate(_waterSplashPrefab, _groundCollider.transform.position, _rotation);
        }

        Debug.Log("Jugador atrapado por la sombra. Movimiento bloqueado.");
    }


   public void ResetShadowEffect()
{
    if (_playerRenderer != null && _originalMaterial != null)
        _playerRenderer.material = _originalMaterial;

    // Recuperar los colliders del jugador.
    if (_targetCollider != null)
        _targetCollider.enabled = true;

    if (_groundCollider != null)
        _groundCollider.enabled = true;

    if (_playerInput != null)
    {
        _playerInput._input = Vector3.zero;
        _playerInput._movementBlocked = false;
    }

    _isTriggered = false;
}

}
