using UnityEngine;

public class FollowEnviroment : MonoBehaviour
{
    public PlayerMovement _player;
    public Transform target;
   // public Vector3 offset = new Vector3(0, 20, 0);
    [SerializeField] private GameObject _miniMap;
    private bool _isMiniMap = true;
    void LateUpdate()
    {
        if (target != null)
            transform.position = target.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            MiniMapOn();
        }
    }

    private void MiniMapOn()
    {
        _isMiniMap = !_isMiniMap;
        if (_isMiniMap)
        {
            _miniMap.SetActive(true);
        }
        else
        {
            _miniMap.SetActive(false);
        }
    }
}
