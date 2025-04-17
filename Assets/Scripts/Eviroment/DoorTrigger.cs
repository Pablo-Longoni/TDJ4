using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private ChangeScene _changeScene;
    public CameraChange _cameraChange;
    void Start()
    {
        _changeScene = GameObject.FindGameObjectWithTag("GameController").GetComponent<ChangeScene>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _cameraChange._isIsometric)
        {
            _changeScene.NextLevel();
            Debug.Log("Jugador entró en la puerta");
        }
    }
}