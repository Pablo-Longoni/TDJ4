using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour
{
    private Vector3 _startPosition;
    public Rigidbody _rb;
    public int _limit;

    [SerializeField] public BoxCollider[] _collisions;

    private ChangeScene _changeScene;

    void Start()
    {
        _startPosition = transform.position;
        Debug.Log("Posición inicial: " + _startPosition);

        _changeScene = GameObject.FindGameObjectWithTag("GameController").GetComponent<ChangeScene>();
    }

    void Update()
    {
        if (transform.position.y <= _limit)
        {
            // _changeScene.RestartLevel();
            RespawnRoutine();
        }
    }

    public void RespawnRoutine()
    {
        // Desactiva colliders para evitar colisiones al moverlo
        foreach (BoxCollider col in _collisions)
        {
            col.enabled = false;
        }

        // Resetea posición
        transform.position = _startPosition;

        // Le da un pequeño impulso hacia abajo para que vuelva a caer
        _rb.linearVelocity = new Vector3(0, -0.1f, 0);
        _rb.angularVelocity = Vector3.zero;

        // Reactiva collider después de un pequeño delay
        Invoke(nameof(EnableCollider), 0.5f);
    }

    void EnableCollider()
    {
        foreach (BoxCollider col in _collisions)
        {
            col.enabled = true;
        }
    }
}
