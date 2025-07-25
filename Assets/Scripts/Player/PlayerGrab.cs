using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public float _grabRange = 2f;
    public Transform _grabPoint;

    private GameObject _grabbedObject;
    private Rigidbody _grabbedRb;

    public bool isGrabbed = false;
    private bool _canGrab = false;

    private PlayerInputReader _input;

    private void Awake()
    {
        _input = FindObjectOfType<PlayerInputReader>();
    }

    void Update()
    {
        if (_input == null) return;

        if (_input.GrabPressed)
        {
            if (_grabbedObject == null)
            {
                TryGrab();
            }
            else
            {
                Release();
            }
        }

        if (_grabbedObject != null)
        {
            MoveGrabbedObject();
        }

        if (!_canGrab && _grabbedObject != null)
        {
            Release();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Movable"))
        {
            _canGrab = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Movable"))
        {
            _canGrab = false;
            if (_grabbedObject != null)
            {
                Release();
            }
        }
    }

    void TryGrab()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _grabRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Movable"))
            {
                _grabbedObject = hit.gameObject;
                _grabbedRb = _grabbedObject.GetComponent<Rigidbody>();
                Physics.IgnoreCollision(GetComponent<Collider>(), _grabbedObject.GetComponent<Collider>(), true);

                if (_grabbedRb != null)
                {
                    _grabbedRb.isKinematic = true;
                }

                break;
            }
        }
    }

    void MoveGrabbedObject()
    {
        isGrabbed = true;
        Vector3 targetPos = _grabPoint.position;
        _grabbedObject.transform.position = Vector3.Lerp(_grabbedObject.transform.position, targetPos, Time.deltaTime * 10f);
    }

    void Release()
    {
        isGrabbed = false;
        Physics.IgnoreCollision(GetComponent<Collider>(), _grabbedObject.GetComponent<Collider>(), false);
        if (_grabbedRb != null)
        {
            _grabbedRb.isKinematic = false;
        }

        _grabbedObject = null;
        _grabbedRb = null;
    }
}
