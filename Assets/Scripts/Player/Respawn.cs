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

            // Desactivar collider para evitar triggers al mover
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            // Teletransportar
            transform.position = _startPosition;

            // Activar collider de nuevo
            if (col != null)
                col.enabled = true;

            // Marcar que acaba de respawnear para que puertas u otros no lo detecten
            _playerMovement.justRespawned = true;
            _playerMovement.Invoke("ResetRespawnFlag", 1f);  // desactiva la flag tras un pequeño delay
        }
    }

}
