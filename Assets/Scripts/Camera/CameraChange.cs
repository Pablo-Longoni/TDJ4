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
    [Header("Camera settings")]
    [SerializeField] private CinemachineCamera _isometricCamera;
    [SerializeField] private CinemachineCamera _overHeadCamera;
    [SerializeField] private CinemachineCamera _cinematicCamera;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CameraRotation _cameraRotation;
    private CinemachineBrain _cameraBrain;

    public bool _isIsometric;
    public bool _canChange = false;
    public bool _cinematicPlaying = true;

    [Header("Other settings")]
    [SerializeField] private PlayerTransformation _playerTransformation;
    [SerializeField] private ChangeScene _changeScene;
    private PlayerInputReader _input;
    private CubeRotation[] _cubeRotation;

    [Header("Domain settings")]
    [SerializeField] private GameObject _sphere;
    [SerializeField] private int _scaleAmount = 650;

    [Header("Cinematic settings")]

    [SerializeField] private float _holdTimer = 0;
    [SerializeField] private float _holdDuration = 0f;
    [SerializeField] private enum SpaceBarState {Cinematic, Playing}
    [SerializeField] private SpaceBarState currentState = SpaceBarState.Cinematic;

    private void Awake()
    {
        _input = FindFirstObjectByType<PlayerInputReader>();
        _cameraBrain = _mainCamera.GetComponent<CinemachineBrain>();
        _cubeRotation = FindObjectsByType<CubeRotation>(FindObjectsSortMode.None);
    }

    void Start()
    {
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
    }

    void LateUpdate()
    {
        // Se resetean los flags recién acá, después de que TODOS los
        // Update() de la escena ya los leyeron, para evitar que el orden
        // de ejecución entre scripts borre un flag antes de que se use.
        if (_input == null) return;

        _input.ResetFlags();
    }

    private void ChangeCamera()
    {
        StartCoroutine(ChangeCameraRoutine());
    }
    private IEnumerator ChangeCameraRoutine()
    {
        _canChange = false;

        if (_isIsometric)
        {
            _isIsometric = false;

            _cameraRotation.ResetRotation();

            _isometricCamera.Priority = 2;
            _overHeadCamera.Priority = 3;

            if (_sphere != null)
            {
                yield return StartCoroutine(ScaleSphere());
            }      
        }
        else
        {
            _isIsometric = true;

            foreach (var cube in _cubeRotation)
            {
                if (cube._didRotate)
                {
                    _playerTransformation.PlayerTransformed();

                    Debug.Log("Transformed + 1 en " + cube.name);

                    cube._didRotate = false;
                    break;
                }
            }

            _isometricCamera.Priority = 3;
            _overHeadCamera.Priority = 2;


            if (_sphere != null)
            {
                yield return StartCoroutine(UnscaleSphere());
            }
        }

        _canChange = true;
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
        _cinematicPlaying = false;
    }

    void SkipCinematic()
    {
        _cameraBrain.DefaultBlend.Time = 1;
        currentState = SpaceBarState.Playing;
        _cinematicCamera.Target = _isometricCamera.Target;
        _cinematicPlaying = false; 
    }


    private IEnumerator ScaleSphere()
    {
        float duration = 1;

        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * _scaleAmount;

        float timer = 0f;

        _sphere.transform.localScale = startScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            t = Mathf.SmoothStep(0f, 1f, t);

            _sphere.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        _sphere.transform.localScale = targetScale;
    }

    private IEnumerator UnscaleSphere()
    {
        float duration = 1;

        Vector3 startScale = Vector3.one * _scaleAmount;
        Vector3 targetScale = Vector3.zero;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            t = Mathf.SmoothStep(0f, 1f, t);

            _sphere.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        _sphere.transform.localScale = targetScale;
    }
}