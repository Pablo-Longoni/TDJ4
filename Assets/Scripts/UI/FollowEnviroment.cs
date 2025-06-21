using UnityEngine;

public class FollowEnviroment : MonoBehaviour
{
    public PlayerMovement _player;
    public Transform target;
   // public Vector3 offset = new Vector3(10, 20, 30);
    [SerializeField] private GameObject _miniMap;
    private bool _isMiniMap = false;
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

    public void MiniMapOn()
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
