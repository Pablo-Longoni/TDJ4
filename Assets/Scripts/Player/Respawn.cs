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
    // Desactivar colisiones
    foreach (BoxCollider col in _collisions)
        col.enabled = false;

    // Resetear física
    _rb.velocity = Vector3.zero;
    _rb.angularVelocity = Vector3.zero;

    // Reposicionar el objeto
    transform.position = _startPosition;

    // Reactivar la física
    _rb.isKinematic = false;
    _rb.useGravity = true;

    // Pequeño impulso hacia abajo para asegurar movimiento
    StartCoroutine(ApplyInitialImpulse());

    // Reactivar colisiones después de un pequeño delay
    Invoke(nameof(EnableCollider), 0.5f);
}

private IEnumerator ApplyInitialImpulse()
{
    yield return new WaitForFixedUpdate();
    _rb.velocity = new Vector3(0, -2f, 0); 
}

    void EnableCollider()
    {
        foreach (BoxCollider col in _collisions)
        {
            col.enabled = true;
        }
    }
}
