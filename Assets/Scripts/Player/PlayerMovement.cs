using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{

    public Vector3 _input;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private CameraChange _cameraChange;
    public CubeRotation _currentCube;

    public FollowEnviroment _minimapCameraFollow;
    private Transform _currentFigure;
    private string _minimapLayerName = "Enviroment";
    private string _defaultLayerName = "Default";
    private void Start()
    {
        _cameraChange = FindAnyObjectByType<CameraChange>();
    }


    void Update()
    {
        GatherInput();
        CheckCurrentCube();
        /*  if (_cameraChange._isIsometric)
          {
              Look();

          }*/
        if (!_cameraChange._isIsometric && _currentCube._canRotate == true)
        {
            Rotating();
        }
    }

    void FixedUpdate()
    {
        GatherInput();
        Move();

        if (_cameraChange._isIsometric)
        {
            Look();
        }
        else if (!_cameraChange._isIsometric && _currentCube._canRotate == true)
        {
            Rotating();
        }
    }
    void GatherInput()
    {
        //  _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (_cameraChange._isIsometric && Camera.main != null)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            // Aseguramos que estén en plano horizontal
            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            _input = (camForward * rawInput.z + camRight * rawInput.x).normalized;
        }
        else
        {
            _input = rawInput;
        }
    }

    void Look()
    {
        if (_input != Vector3.zero)
        {
            /* var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

             var skewdInput = matrix.MultiplyPoint3x4(_input);

             var relative = (transform.position + skewdInput) - transform.position;
             var rotation = Quaternion.LookRotation(relative, Vector3.up);

             transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _turnSpeed * Time.deltaTime);*/
            Quaternion rotation = Quaternion.LookRotation(_input, Vector3.up);
             transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _turnSpeed * Time.fixedDeltaTime);

        }
    }

    public void Move()
    {
        if (_cameraChange._isIsometric)
        {
          //  _rb.MovePosition(transform.position + _input * _speed * Time.deltaTime);
            _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _speed * Time.deltaTime);
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            //    Debug.Log("Movimiento isometrico");
        }
        else
        {

            _rb.constraints = RigidbodyConstraints.FreezeAll;
            //  Debug.Log("Movimiento cential");
        }
    }

    // Rotacion de la figura
    public void Rotating()
    {
        if (_input == Vector3.zero) return;

        //   Debug.Log("Rotating");
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitDown, groundCheckDistance);
        Vector3 rotationAxis = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W))
            rotationAxis = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.S))
            rotationAxis = Vector3.right;
        else if (Input.GetKeyDown(KeyCode.A))
            rotationAxis = Vector3.forward;
        else if (Input.GetKeyDown(KeyCode.D))
            rotationAxis = Vector3.back;


        //si detecta una figura actualiza _currentCube
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance + 0.5f))
        {
            CubeRotation detectedCube = hit.collider.GetComponent<CubeRotation>();
            if (detectedCube != null)
            {
                _currentCube = detectedCube;
                _currentCube.StartBlinking();
            }
        }

        // si hay una figura guardada, la rota
        if (_currentCube != null)
        {
            _currentCube.RotateCube(rotationAxis, transform);
        }
    }


    void CheckCurrentCube()
    {
        if (_input == Vector3.zero) return;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance + 0.5f))
        {
            CubeRotation detectedCube = hit.collider.GetComponent<CubeRotation>();
            if (detectedCube != null && detectedCube != _currentCube)
            {
             _currentCube.StopBlinking();
             _currentCube = detectedCube;
             _currentCube.StartBlinking();
            }

            Transform currentFigure = hit.collider.transform;

            if (_currentFigure != currentFigure)
            {
                if (_currentFigure != null)
                    _currentFigure.gameObject.layer = LayerMask.NameToLayer(_defaultLayerName);

                currentFigure.gameObject.layer = LayerMask.NameToLayer(_minimapLayerName);
                _currentFigure = currentFigure;
            }

        }

        //minmapa

        
    }
}
   
     
