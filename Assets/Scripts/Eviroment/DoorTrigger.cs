using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class DoorTrigger : MonoBehaviour
{
    private ChangeScene _changeScene;
    public CameraChange _cameraChange;
    [SerializeField] public PlayerMovement _player;
    public float moveSpeed = 2f; 
   // private bool isMoving = false;

    [SerializeField] public AudioManager _audioManager;
    string _sceneName = string.Empty;
    void Start()
    {
        _changeScene = GameObject.FindGameObjectWithTag("GameController").GetComponent<ChangeScene>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _cameraChange._isIsometric && !other.GetComponent<PlayerMovement>().justRespawned)
        {
            _audioManager.playSound(_audioManager._portal);

                _changeScene.NextLevel();

            Debug.Log("Jugador entró en la puerta");
        }
    }

   /* private IEnumerator MovePlayerToDoor(Vector3 targetPosition)
    {
    //   isMoving = true;
        _audioManager.playSound(_audioManager._portal);
        float timeElapsed = 0f;
        Vector3 initialPosition = _player.transform.position;


        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * moveSpeed; 
            _player.transform.position = Vector3.Lerp(initialPosition, targetPosition, timeElapsed);
            _changeScene.NextLevel();
            yield return null;
        }


        _changeScene.NextLevel();
        Debug.Log("Jugador entró en la puerta");

    //    isMoving = false; 
    }*/
}