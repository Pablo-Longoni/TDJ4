using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
/*using Unity.VisualScripting;
using NUnit.Framework.Constraints;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;*/

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
    private float _blendTime;
    public enum SpaceBarState
    {
        Cinematic,
        Playing
    }

    public SpaceBarState currentState = SpaceBarState.Cinematic;
    public float _holdTimer = 0;
    public float _holdDuration = 0f;
    private PlayerInputReader _input;

    //Saber si rotó
    public CubeRotation [] _cubeRotation;

    private void Awake()
    {
        _input = FindObjectOfType<PlayerInputReader>();
        _cubeRotation = FindObjectsByType<CubeRotation>(FindObjectsSortMode.None);
    }

    void Start()
    {
        // _isometricCamera.Priority = 2;
        _overHeadCamera.Priority = 2;
        // _cameraData = _mainCamera.GetComponent<UniversalAdditionalCameraData>();
        _cameraBrain = _mainCamera.GetComponent<CinemachineBrain>();
        _isIsometric = true;
        _blendTime = _cameraBrain.DefaultBlend.Time;
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
        if (_input == null) return;

        switch (currentState)
        {
            case SpaceBarState.Cinematic:
                if (_input.CameraFlipTriggered)
                    SkipCinematic();
                break;

            case SpaceBarState.Playing:
                if (_input.CameraFlipTriggered && _canChange)
                    ChangeCamera();
                break;
        }

        if (_input.RestartKeyPressed)
        {
            _changeScene.RestartLevel();
        }

        _input.ResetFlags();
    }

    private void ChangeCamera()
    {
        Debug.Log("ChangeCamera");
        _isIsometric = !_isIsometric;
        _cameraBrain.DefaultBlend.Time = 1;
        if (!_isIsometric)
        {
           
            _cameraRotation.ResetRotation();
            _isometricCamera.Priority = 2;
            _overHeadCamera.Priority = 3;
            Debug.Log("Camara cenital" + _isIsometric);
        }
        else
        {
            foreach (var cube in _cubeRotation)
            {
                if (cube._didRotate)
                {
                    _playerTransformation.PlayerTransformed();
                    Debug.Log("Transformed + 1 en " + cube.name);
                    cube._didRotate = false; // reseteo solo el que rotó
                    break; // salgo del loop, ya no importa el resto
                }
            }
            _isometricCamera.Priority = 3;
            _overHeadCamera.Priority = 2;
            //   _playerTransformation.GetComponent<CubeAnimation>().IgnoreStretchAndSquash(0.3f);
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

        // Esperar a que ya no sea la cámara activa
        yield return new WaitUntil(() =>
            _cameraBrain.ActiveVirtualCamera.Name != _cinematicCamera.Name
        );

        // Esperar a que termine el blend (esto es lo que te faltaba)
        yield return new WaitUntil(() => !_cameraBrain.IsBlending);

        // Recién ahora cambia de estado
        currentState = SpaceBarState.Playing;
        _cameraBrain.DefaultBlend.Time = 1;
        Debug.Log("Cinematica terminada y blend finalizado");
    }

    void SkipCinematic()
    {
        currentState = SpaceBarState.Playing;
        _cinematicCamera.Target = _isometricCamera.Target;
    }
}
