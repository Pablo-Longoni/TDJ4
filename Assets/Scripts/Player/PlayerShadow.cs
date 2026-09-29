using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private GameObject _fakeShadow;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _rayDistance = 300f;
    [SerializeField] private float _sphereRadius = 2f;
    [SerializeField] private float _shadowOffset = 0.01f;

    [SerializeField] private PlayerGroundDetection _groundDetection;

    private void Awake()
    {
        _fakeShadow.SetActive(true);
    }

    private void Update()
    {
        RelocateShadow();
    }

    private void RelocateShadow()
    {
        Vector3 rayOrigin = _playerTransform.position;

        Debug.DrawRay(
            rayOrigin,
            Vector3.down * _rayDistance,
            Color.red
        );

        bool hasGroundBelow = Physics.SphereCast(
            rayOrigin,
            _sphereRadius,
            Vector3.down,
            out RaycastHit hit,
            _rayDistance,
            _layerMask
        );

        // PLAYER ESTÁ EN EL SUELO
        if (_groundDetection.IsGrounded)
        {
            _fakeShadow.SetActive(true);

            Vector3 shadowPosition = _playerTransform.position;
            shadowPosition.y -= _shadowOffset;

            _fakeShadow.transform.position = shadowPosition;

            return;
        }

        // PLAYER ESTÁ EN EL AIRE Y HAY UNA FIGURA DEBAJO
        if (hasGroundBelow)
        {
            _fakeShadow.SetActive(true);

            Vector3 shadowPosition = hit.point;
            shadowPosition.y += _shadowOffset;

            _fakeShadow.transform.position = shadowPosition;

            Debug.DrawLine(
                rayOrigin,
                hit.point,
                Color.green
            );

            return;
        }

        // PLAYER ESTÁ EN EL AIRE Y NO HAY NADA DEBAJO
        _fakeShadow.SetActive(false);
    }
}