using System.Collections;
using System.Drawing;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private PlayerMovement _movement;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 40f;
    [SerializeField] private float dashDuration = 0.5f;

    private bool _dashing;
    private float _dashTimer;
    private Vector3 _dashDirection;

    [Header("Visual feedback")]
  //  [SerializeField] private TrailRenderer _dashTrail;
  //  [SerializeField] private ParticleSystem _dashParticles;
    [SerializeField] private GameObject _dashAfterImage;
    [SerializeField] private Material _dashMaterial;
    [SerializeField] private float _afterImageDuration = 0.5f;
    [SerializeField] private int _afterImageCount = 5;
    [SerializeField] private float _timeBetweenAfterImage = 0.05f;
    private MeshRenderer _meshRenderer;
    private Material _originaLMaterial;
    // STATIC: compartido globalmente, para que cualquier PlayerMovement
    // (puede haber más de uno controlando el mismo Rigidbody) sepa que
    // hay un dash en curso y no le pise la velocidad.
    public static bool IsAnyDashing { get; private set; }

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _originaLMaterial = _meshRenderer != null ? _meshRenderer.material : null;

    /*    if(_dashTrail != null)
            _dashTrail.emitting = false;*/
    }
    private void Update()
    {
        // Detectar y consumir el input acá, sincronizado con el mismo ciclo
        // en el que CameraChange.LateUpdate() resetea los flags.
        if (_inputReader.DashTriggered && !_dashing)
        {
            Vector3 dir = _movement._input != Vector3.zero
                ? _movement._input.normalized
                : transform.forward;

            _dashing = true;
            IsAnyDashing = true;
            _dashTimer = dashDuration;
            _dashDirection = dir;

            _inputReader.ConsumeDash();

            //visual effect
        /*    if (_dashTrail != null)
                _dashTrail.emitting = true;*/

            if (_dashAfterImage != null)
                StartCoroutine(CreateAfterImages());
        }
    }

    private void FixedUpdate()
    {
        // FixedUpdate solo aplica la física del dash ya iniciado.
        if (_dashing)
        {
            Vector3 dashVelocity = _dashDirection * dashSpeed;
            _rb.linearVelocity = new Vector3(dashVelocity.x, _rb.linearVelocity.y, dashVelocity.z);

            _dashTimer -= Time.fixedDeltaTime;

            if (_dashTimer <= 0f)
            {
                _dashing = false;
                IsAnyDashing = false;

                _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
            }
        }
    }


    private IEnumerator CreateAfterImages()
    {
        for (int i = 0; i < _afterImageCount; i++)
        {
            GameObject afterImage = Instantiate(_dashAfterImage, transform.position, transform.rotation);
            Destroy(afterImage, _afterImageDuration);
            yield return new WaitForSeconds(_timeBetweenAfterImage);
        }

    }
}