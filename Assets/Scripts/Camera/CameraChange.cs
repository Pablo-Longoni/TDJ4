using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class CameraChange : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _isometricCamera;
    [SerializeField ] private CinemachineCamera _overHeadCamera;

    [SerializeField] private Camera _mainCamera;
    private UniversalAdditionalCameraData _cameraData;

    public bool _isIsometric = true;
    public bool _canChange = true;
    public CameraRotation _cameraRotation;

    public PlayerGrab _playerGrab;
    public PlayerTransformation _playerTransformation;
    void Start()
    {
        _isometricCamera.Priority = 2;
        _overHeadCamera.Priority = 1;

        _cameraData = _mainCamera.GetComponent<UniversalAdditionalCameraData>();
        _isIsometric = true;
    }


void Update()
    {
        ChangeCamera();
    }

    private void ChangeCamera()
    {
       if (Input.GetKeyDown(KeyCode.Space) && _canChange)
        {
           
            _isIsometric = !_isIsometric;
            _playerTransformation.PlayerTransformed();

            if (!_isIsometric)
            {
                _cameraRotation.ResetRotation();
                _isometricCamera.Priority = 1;
                _overHeadCamera.Priority = 2;
                _cameraData.renderShadows = false;
                 Debug.Log("Camara cenital" + _isIsometric);
                _playerGrab._canGrab = false;
            }
            else
            {
                _isometricCamera.Priority = 2;
                _overHeadCamera.Priority = 1;
                _cameraData.renderShadows = true;
                 Debug.Log("Camara Isometrica");
                _playerGrab._canGrab = true;
            }
        }
    }
}
