using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private GameObject _fakeShadow;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _rayDistance = 300f;
    [SerializeField] private float _sphereRadius = 0.2f;
    [SerializeField] private float _shadowOffset = 0.01f;
    [SerializeField] private float _rayOriginHeight = 3f;

    private void Update()
    {
        RelocateShadow();
    }

    private void RelocateShadow()
    {
        Vector3 rayOrigin =
            _playerTransform.position +
            Vector3.up * _rayOriginHeight;

        Debug.DrawRay(
            rayOrigin,
            Vector3.down * _rayDistance,
            Color.red
        );

        if (Physics.SphereCast(
            rayOrigin,
            _sphereRadius,
            Vector3.down,
            out RaycastHit hit,
            _rayDistance,
            _layerMask))
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
        }
        else
        {
            _fakeShadow.SetActive(false);
        }
    }
}