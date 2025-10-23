using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

using UnityEngine.SceneManagement; 
public class FinalAnimation : MonoBehaviour
{
    //player
    [SerializeField] private PlayerMovement _player, _input, _input2;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private ParticleSystem _particleRipple;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Respawn _respawn;

    [SerializeField] private float _growDuration = 5f;
    [SerializeField] private float _targetScale = 50f;

    //material
    [SerializeField] private Renderer _playerRenderer;
    [SerializeField] private Material _targetMaterial;
    [SerializeField] private float _blendDuration = 3f;

    //camera
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private CinemachineCamera _menuCamera;
    [SerializeField] private CameraShake _cameraShake;
    private CinemachineBrain _cameraBrain;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CameraZoom _cameraZoom;
    [SerializeField] private CameraRotation _cameraRotation;
    [SerializeField] private GameObject _cameraParent;

    //canvas
    [SerializeField] private Canvas _menuCanvas;
    [SerializeField] private CanvasGroup _fadeCanvas;
    [SerializeField] private Canvas _gameplayCanvas;
    [SerializeField] private GameObject _title;
    //effects
    private bool isBlinking = false;

    //figures
    [SerializeField] private GameObject _figure;
    [SerializeField] private GameObject[] _allFigures;
    [SerializeField] private Vector3 _finalPosition;
    private void Start()
    {
        _cameraBrain = _mainCamera.GetComponent<CinemachineBrain>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FinalAnimationSequence());
            Debug.Log("Jugador entró al portal final");
        }
    }

    private IEnumerator FinalAnimationSequence()
    {
        _camera.Target.TrackingTarget = null;
        _cameraRotation.enabled = false;
        _cameraZoom.enabled = false;
        _respawn.enabled = false;
        _player.enabled = false;
        _input.enabled = false;
        _input2.enabled = false;
        yield return LevitatePlayer();
        yield return ChangeColor();
      //  yield return CameraRotationAndZoomOut();
    }

    private IEnumerator LevitatePlayer()
    {
        Time.timeScale = 0.1f; // ralentizamos el tiempo
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        Instantiate(_particleRipple, _playerTransform.position, Quaternion.identity);

        // pequeño retardo dramático
        yield return new WaitForSecondsRealtime(1f);
    }

    private IEnumerator Blinking()
    {
        Debug.Log("Blinking start");
        isBlinking = true;
        HideAllFigures();
        if (_fadeCanvas == null)
        {
            Debug.LogWarning("No hay CanvasGroup asignado para el parpadeo");
            yield break;
        }

        while (isBlinking)
        {
            // parpadeo de la pantalla
            float blinkTime = 0.2f;
            _fadeCanvas.alpha = 0.9f;
            yield return new WaitForSeconds(blinkTime);
            _fadeCanvas.alpha = 0;
            yield return new WaitForSeconds(blinkTime);
        }

    }
    private IEnumerator ChangePlayerSize()
    {
        Debug.Log("ChangePlayerSize start");
        Time.timeScale = 0.7f;
        Time.fixedDeltaTime = 0.02f;

        // StartCoroutine(Blinking());
        HideAllFigures();
        _cameraShake.Shake(3f, 0.3f, 2f);

        _gameplayCanvas.gameObject.SetActive(false);
        _title.SetActive(false);

        _finalPosition = new Vector3(1000,1000,1000);
        _figure.transform.position = _finalPosition;

        Vector3 originalScale = _playerTransform.localScale;
        Vector3 finalScale = Vector3.one * _targetScale;

        float originalOrthoSize = _camera.Lens.OrthographicSize;
        float targetOrthoSize = originalOrthoSize * 2.5f;

        _player.transform.localRotation = Quaternion.Euler(0, 72, 0);
        StartCoroutine(CameraRotationAndZoomOut());
        float elapsed = 0f;
        while (elapsed < _growDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _growDuration;
            _playerTransform.localScale = Vector3.Lerp(originalScale, finalScale, t);
            _camera.Lens.OrthographicSize = Mathf.Lerp(originalOrthoSize, targetOrthoSize, t);
            yield return null;
        }
        _playerTransform.localScale = finalScale;
        _camera.Lens.OrthographicSize = targetOrthoSize;
        isBlinking = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ChangeColor()
    {
        StartCoroutine(ChangePlayerSize());

        Material startMat = _playerRenderer.material;
        Material tempMat = new Material(startMat); // copia temporal
        _playerRenderer.material = tempMat;

        float elapsed = 0f;

        // Nos aseguramos que ambos materiales tengan el mismo shader con _Color o _BaseColor
        Color startColor = startMat.color;
        Color endColor = _targetMaterial.color;

        while (elapsed < _blendDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _blendDuration;

            tempMat.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        _playerRenderer.material = _targetMaterial;
    }

    private void HideAllFigures()
    {
        foreach (GameObject i in _allFigures)
        {
            i.SetActive(false);
        }
    }
    private IEnumerator CameraRotationAndZoomOut()
    {
        _cameraParent.transform.localRotation = Quaternion.Euler(0,0,0);
        _cameraBrain.DefaultBlend.Time = 6;
        _menuCanvas.gameObject.SetActive(true);
        _camera.Priority = 0;
        _menuCamera.Priority = 5;

        Camera cam = Camera.main;

        // Cambiar gradualmente a color sólido
        cam.clearFlags = CameraClearFlags.SolidColor;

        Color startColor = cam.backgroundColor;
        Color endColor = Color.white; // o el que quieras

        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cam.backgroundColor = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }

        cam.backgroundColor = endColor;

        _player.transform.localRotation = Quaternion.Euler(0, 72, 0);
        // Mostrar el menú final
        Debug.Log("Canvas final mostrado");
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Credits");
    }

}
