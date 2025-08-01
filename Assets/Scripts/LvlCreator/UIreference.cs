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

    //Pressed
    public GameObject _portal;

    public static UIreference Instance;

    private void Awake()
    {
        Instance = this;
    }
}
