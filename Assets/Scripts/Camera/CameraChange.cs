using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using Unity.VisualScripting;
using NUnit.Framework.Constraints;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _isometricCamera;
    [SerializeField] private CinemachineCamera _overHeadCamera;
    [SerializeField] private CinemachineCamera _cinematicCamera;
    [SerializeField] private Camera _mainCamera;

    // private UniversalAdditionalCameraData _cameraData;
    private CinemachineBrain _cameraBrain;

    public bool _isIsometric;
    public bool _canChange = false;

    public CameraRotation _cameraRotation;
    public PlayerTransformation _playerTransformation;

    // Funciones del espacio
    public ChangeScene _changeScene;

    public enum SpaceBarState
    {
        Cinematic,
        Playing
    }

    public SpaceBarState currentState = SpaceBarState.Cinematic;
    public float _holdTimer = 0;
    public float _holdDuration = 0.5f;

    void Start()
    {
        // _isometricCamera.Priority = 2;
        _overHeadCamera.Priority = 2;
        // _cameraData = _mainCamera.GetComponent<UniversalAdditionalCameraData>();
        _cameraBrain = _mainCamera.GetComponent<CinemachineBrain>();
        _isIsometric = true;
        StartCoroutine(DelayedCinematicStart());
    }

    private IEnumerator DelayedCinematicStart()
    {
        yield return new WaitUntil(() =>
            _cameraBrain != null &&
            _cameraBrain.ActiveVirtualCamera != null
        );

        CinematicCamera();
    }

    void Update()
    {
        switch (currentState)
        {
            case SpaceBarState.Cinematic:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SkipCinematic();
                }
                break;

            case SpaceBarState.Playing:
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton3)) && _canChange)
                {
                    ChangeCamera();
                }
                break;
        }

        if (Input.GetKey(KeyCode.R))
        {
            _holdTimer += Time.deltaTime;
            if (_holdTimer >= _holdDuration)
            {
                currentState = SpaceBarState.Cinematic;
                _changeScene.RestartLevel();
                _holdTimer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            _holdTimer = 0;
        }
    }

    private void ChangeCamera()
    {
        Debug.Log("ChangeCamera");
        _isIsometric = !_isIsometric;
        _playerTransformation.PlayerTransformed();

        if (!_isIsometric)
        {
            _cameraRotation.ResetRotation();
            _isometricCamera.Priority = 2;
            _overHeadCamera.Priority = 3;
            // _cameraData.renderShadows = true;
            Debug.Log("Camara cenital" + _isIsometric);
        }
        else
        {
            _isometricCamera.Priority = 3;
            _overHeadCamera.Priority = 2;
            // _cameraData.renderShadows = true;
            Debug.Log("Camara Isometrica");
        }
    }

    public void CinematicCamera()
    {
        _isometricCamera.Priority = 3;
        _cinematicCamera.Priority = 1;
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
        Debug.Log("Cinematic terminada");
    }

    void SkipCinematic()
    {
        currentState = SpaceBarState.Playing;
        _cinematicCamera.Target = _isometricCamera.Target;
    }
}
