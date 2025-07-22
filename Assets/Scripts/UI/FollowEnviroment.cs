using UnityEngine;
using UnityEngine.InputSystem;

public class FollowEnviroment : MonoBehaviour
{
    public PlayerMovement _player;
    public Transform target;
    [SerializeField] private GameObject _miniMap;
    private bool _isMiniMap = false;

    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();

        _controls.Camera.CameraHelp.performed += ctx => MiniMapOn();
    }

    private void OnEnable()
    {
        _controls.Camera.Enable();
    }

    private void OnDisable()
    {
        _controls.Camera.Disable();
    }

    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.position;
    }

    public void MiniMapOn()
    {
        _isMiniMap = !_isMiniMap;
        _miniMap.SetActive(_isMiniMap);
    }
}