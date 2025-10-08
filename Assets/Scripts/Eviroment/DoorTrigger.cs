using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class DoorTrigger : MonoBehaviour
{
    private ChangeScene _changeScene;
    public CameraChange _cameraChange;
    [SerializeField] public PlayerMovement _player;
    [SerializeField] public CubeAnimation _cubeAnimation;
    public float moveSpeed = .7f;
   // private bool isMoving = false;
    public GameObject _target;
    [SerializeField] public AudioManager _audioManager;
    string _sceneName = string.Empty;
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private CameraZoom _cameraZoom;
    [SerializeField] private ParticleSystem _particleEnterPortal;
    void Start()
    {
        _changeScene = GameObject.FindGameObjectWithTag("GameController").GetComponent<ChangeScene>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _cameraChange._isIsometric/* && !other.GetComponent<PlayerMovement>().justRespawned*/)
        {
          //  _changeScene.NextLevel();
            _cameraZoom.ZoomIn(1500);
         //   _cameraShake.Shake(1, 1, 1);
            StartCoroutine(MovePlayerToDoor(_target.transform.position));
           AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._portal);

        

            Debug.Log("Jugador entró en la puerta");
        }
    }

    private IEnumerator MovePlayerToDoor(Vector3 targetPosition)
    {
        _player.enabled = false;
        //   isMoving = true;
        _cameraShake.Shake(2, 1, 1);
        float timeElapsed = 0f;
        Vector3 initialPosition = _player.transform.position;
        _cubeAnimation.EnterPortalAnim();
        Instantiate(_particleEnterPortal, targetPosition, Quaternion.identity);
        while (timeElapsed < 2f)
        {
            timeElapsed += Time.deltaTime * moveSpeed; 
            _player.transform.position = Vector3.Lerp(initialPosition, targetPosition, timeElapsed);
            yield return null;
        }
        _changeScene.NextLevel();
        Debug.Log("Jugador entró en la puerta");
        _player.enabled = true;
        // isMoving = false; 
    }
}