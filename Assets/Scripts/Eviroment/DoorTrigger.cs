using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class DoorTrigger : MonoBehaviour
{
    private ChangeScene _changeScene;
    public CameraChange _cameraChange;
    [SerializeField] public PlayerMovement _player;

    public float moveSpeed = 0.5f;
   // private bool isMoving = false;
    public GameObject _target;
    [SerializeField] public AudioManager _audioManager;
    string _sceneName = string.Empty;
    void Start()
    {
        _changeScene = GameObject.FindGameObjectWithTag("GameController").GetComponent<ChangeScene>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _cameraChange._isIsometric/* && !other.GetComponent<PlayerMovement>().justRespawned*/)
        {
            _changeScene.NextLevel();
            StartCoroutine(MovePlayerToDoor(_target.transform.position));
         //   _audioManager.playSound(_audioManager._portal);

           

            Debug.Log("Jugador entró en la puerta");
        }
    }

    private IEnumerator MovePlayerToDoor(Vector3 targetPosition)
    {
        _player.enabled = false;
        //   isMoving = true;
        _audioManager.playSound(_audioManager._portal);
        float timeElapsed = 0f;
        Vector3 initialPosition = _player.transform.position;

        while (timeElapsed < 2f)
        {
            timeElapsed += Time.deltaTime * moveSpeed; 
            _player.transform.position = Vector3.Lerp(initialPosition, targetPosition, timeElapsed);
            yield return null;
        }

        Debug.Log("Jugador entró en la puerta");
        _player.enabled = true;
        // isMoving = false; 
    }
}