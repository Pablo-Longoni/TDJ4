using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 _startPosition;
    public Rigidbody _rb;
    public int _limit;
    public PlayerGrab _playerGrab;
    [SerializeField] public PlayerMovement _playerMovement;
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
            _rb.linearVelocity = Vector3.zero;


            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;


            transform.position = _startPosition;

            if (col != null)
                col.enabled = true;

            _playerMovement.justRespawned = true;
            _playerMovement.Invoke("ResetRespawnFlag", 1f);  
        }
    }

}
