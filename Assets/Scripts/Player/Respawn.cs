using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour
{
    private Vector3 _startPosition;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _limit = -10f;
    [SerializeField] private BoxCollider[] _collisions;

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private DeactivateCollider[] _shadowEffects;
    [SerializeField] private PlayerShadow _playerShadow;

    [SerializeField] private CubeAnimation _playerAnimation;
    private bool _isRespawning;
    private ChangeScene _changeScene;

    private void Start()
    {
        _startPosition = transform.position;

        _changeScene = GameObject.FindGameObjectWithTag("GameController").GetComponent<ChangeScene>();
    }

    private void Update()
    {
        if (!_isRespawning && _rb.position.y <= _limit)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        _playerAnimation.CallFalling();

        _isRespawning = true;

        // Bloquear al jugador durante el reinicio.
        if (_playerMovement != null)
        {
            _playerMovement._input = Vector3.zero;
        }

        // Detener la física.
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // Desactivar temporalmente las colisiones del jugador.
        foreach (Collider col in _collisions)
        {
            if (col != null)
                col.enabled = false;
        }

        // Volver al punto inicial.
        _rb.position = _startPosition;
        transform.position = _startPosition;

        _rb.isKinematic = false;
        _rb.useGravity = true;

        yield return new WaitForFixedUpdate();

        // Reactivar las colisiones del jugador.
        foreach (Collider col in _collisions)
        {
            if (col != null)
                col.enabled = true;
        }

        // Reiniciar TODAS las sombras.
        foreach (DeactivateCollider shadow in _shadowEffects)
        {
            if (shadow != null)
                shadow.ResetShadowEffect();
        }

        // Detener cualquier movimiento residual.
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _isRespawning = false;

      /*  if (_playerShadow != null)
        {
            _playerShadow.RestoreShadow();
        }*/
    }
}
