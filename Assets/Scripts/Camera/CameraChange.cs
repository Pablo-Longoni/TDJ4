using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
public class CameraChange : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _isometricCamera;
    [SerializeField ] private CinemachineCamera _overHeadCamera;
    [SerializeField] private CinemachineCamera _cinematicCamera;

    [SerializeField] private Camera _mainCamera;
    private UniversalAdditionalCameraData _cameraData;
    private CinemachineBrain _cameraBrain;

    public bool _isIsometric = true;
    public bool _canChange = true;
    public CameraRotation _cameraRotation;

    public PlayerGrab _playerGrab;
    public PlayerTransformation _playerTransformation;
    void Start()
    {
      //  _isometricCamera.Priority = 2;
        _overHeadCamera.Priority = 2;

        _cameraData = _mainCamera.GetComponent<UniversalAdditionalCameraData>();
        _cameraBrain = _mainCamera.GetComponent<CinemachineBrain>();
        _isIsometric = true;
        StartCoroutine(DelayedCinematicStart());
    }

    private IEnumerator DelayedCinematicStart()
    {
        // Esperar  que haya una cámara activa
        yield return new WaitUntil(() =>
            _cameraBrain != null &&
            _cameraBrain.ActiveVirtualCamera != null
        );

        CinematicCamera();
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
                _isometricCamera.Priority = 2;
                _overHeadCamera.Priority = 3;
                _cameraData.renderShadows = false;
                 Debug.Log("Camara cenital" + _isIsometric);
                _playerGrab._canGrab = false;
            }
            else
            {
                _isometricCamera.Priority = 3;
                _overHeadCamera.Priority = 2;
                _cameraData.renderShadows = true;
                 Debug.Log("Camara Isometrica");
                _playerGrab._canGrab = true;
            }
        }

    }

    public void CinematicCamera()
    {
        _isometricCamera.Priority = 3;
        _cinematicCamera.Priority = 1;
        Debug.Log("Cinematic terminada");
        StartCoroutine(WaitForCinematicEnd());
    }

    private IEnumerator WaitForCinematicEnd()
    {
        yield return new WaitUntil(() =>
        _cameraBrain != null &&
        _cameraBrain.ActiveVirtualCamera != null &&
        _cameraBrain.ActiveVirtualCamera.Name == _cinematicCamera.Name
    );

        // Esperar a que la cámara activa ya no sea la cinemática
        yield return new WaitUntil(() =>
            _cameraBrain.ActiveVirtualCamera.Name != _cinematicCamera.Name
        );

        _cameraBrain.DefaultBlend.Time = 1;
        //  _cameraBrain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseIn;
    }
}
