using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIreference : MonoBehaviour
{
    //Player Transformation script
    public TextMeshProUGUI transformationText;
    public CameraChange _cameraChange;
    public Button _cheatTrans;

    //Player Movement script
    public FollowEnviroment _minimapCameraFollow;

    //Teleport
    public PlayerGrab _playerGrab;
    public CubeAnimation _cubeAnimation;
    public CameraShake _cameraShake;

    //Pressed
    public GameObject _portal;

    //Shadow objects
    public Transform _lightTransform;
    public Collider _deactivateCollider;

    //Flip
    public PlayerTransformation _player;

    //Mirror

    public static UIreference Instance;

    private void Awake()
    {
        Instance = this;
    }
}
