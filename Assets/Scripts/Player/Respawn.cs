using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 _startPosition;
    public Rigidbody _rb;
    public int _limit;
    public PlayerGrab _playerGrab;
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RespawnObject();
    }

    void RespawnObject()
    {
        if (transform.position.y <= _limit)
        {
           // _playerGrab._canGrab = false;
            _rb.linearVelocity = Vector3.zero;
            transform.position = _startPosition;
        }
    }
}
