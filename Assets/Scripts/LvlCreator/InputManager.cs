using UnityEngine;
using System;
using UnityEngine.EventSystems;
public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private Vector3 _lastPosition;

    [SerializeField] private LayerMask _placementLayer;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }
    public bool IsPointerOverUI()   
        => EventSystem.current.IsPointerOverGameObject();
    
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 _mousePos = Input.mousePosition;
        _mousePos.z = _mainCamera.nearClipPlane;
        Ray _ray = _mainCamera.ScreenPointToRay(_mousePos);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, 100, _placementLayer))
        {
            _lastPosition = _hit.point;
        }
         return _lastPosition;  
    }
}
