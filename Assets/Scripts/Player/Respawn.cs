using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
using System.Collections;
public class Respawn : MonoBehaviour
{
    private Vector3 _startPosition;
    public Rigidbody _rb;
    public int _limit;

   [SerializeField] public BoxCollider [] _collisions;

    void Start()
    {
        _startPosition = transform.position;
        Debug.Log("Posicion inicial: " + _startPosition);
    }


    void Update()
    {
        if (transform.position.y <= _limit)
        {
           RespawnRoutine();
        }
    }

   /* void RespawnObject()
    {
        if (transform.position.y <= _limit)
        {
            StartCoroutine(RespawnRoutine());
        }
    }*/

    public void RespawnRoutine()
    {
        // Desactiva collider para evitar colisiones al moverlo
        foreach (BoxCollider col in _collisions)
        {
            col.enabled = false;
        }

        // Resetea posición y velocidad
        transform.position = _startPosition;
        _rb.linearVelocity = Vector3.zero;
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
