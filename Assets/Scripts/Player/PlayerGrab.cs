using UnityEngine;
using UnityEngine.UIElements;

public class PlayerGrab : MonoBehaviour
{
    public float _grabRange = 2f;
    public Transform _grabPoint;

    public GameObject _grabbedObject;
    private Rigidbody _grabbedRb;

    public bool isGrabbed = false;
    public bool _canGrab = false;

    private PlayerInputReader _input;
    private bool _grabButtonHandled = false;

    private void Awake()
    {
        _input = FindFirstObjectByType<PlayerInputReader>();

    }

    void Update()
    {
        if (_input == null) return;

        bool grabInput = _input.GrabPressed || Input.GetKeyDown(KeyCode.F);

        if (grabInput)
        {
            if (!_grabButtonHandled)
            {
                if (_grabbedObject == null)
                    TryGrab();
                else
                    Release();

                _grabButtonHandled = true;
            }
        }
        else
        {
            _grabButtonHandled = false;
        }

        if (_grabbedObject != null)
        {
            MoveGrabbedObject();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Movable"))
        {
            _canGrab = true;
            MeshRenderer _renderer = other.gameObject.GetComponent<MeshRenderer>();
            _renderer.material.color = Color.gray;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Movable"))
        {
            _canGrab = false;
            MeshRenderer _renderer = other.gameObject.GetComponent<MeshRenderer>();
            _renderer.material.color = Color.white;
        }
    }

    void TryGrab()
    {
        if (!_canGrab) return;
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
                isGrabbed = true;
                break;
            }
        }
    }

    void MoveGrabbedObject()
    {
        //  Debug.Log("Move object");
        MeshRenderer _renderer = _grabbedObject.gameObject.GetComponent<MeshRenderer>();
        _renderer.material.color = Color.grey;
        isGrabbed = true;
        Vector3 targetPos = _grabPoint.position;
        _grabbedObject.transform.position = Vector3.Lerp(_grabbedObject.transform.position, targetPos, Time.deltaTime * 10f);
    }

    void Release()
    {
        Debug.Log("Release object");
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
