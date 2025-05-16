using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
using System.Collections;
public class Respawn : MonoBehaviour
{
    private Vector3 _startPosition;
    public Rigidbody _rb;
    public int _limit;

    [SerializeField] public BoxCollider _player;
    void Start()
    {
        _startPosition = transform.position;
        Debug.Log("Posicion inicial: " + _startPosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RespawnObject();
    }

    void RespawnObject()
    {
        if (transform.position.y <= _limit)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    IEnumerator RespawnRoutine()
    {
        // Desactivar f�sicas y colisiones
        _player.enabled = false;
        //   _rb.isKinematic = false;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // Reposicionar
        transform.position = _startPosition;
        Debug.Log("Posicion inicial en corrutina: " + _startPosition);
        // Esperar un frame para asegurar que las f�sicas se actualicen
        yield return null;

        // Reactivar f�sicas y colisiones
        _rb.isKinematic = false;
        _player.enabled = true;
    }

}
