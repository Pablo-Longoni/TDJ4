using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInputReader inputReader; // Asignar en el Inspector

    public Vector3 _input;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private CameraChange _cameraChange;
    public CubeRotation _currentCube;

    private void Start()
    {
        _cameraChange = FindAnyObjectByType<CameraChange>();
    }

    void Update()
    {
        GatherInput();
        CheckCurrentCube();

        if (!_cameraChange._isIsometric && _currentCube._canRotate)
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
        else if (!_cameraChange._isIsometric && _currentCube._canRotate)
        {
            Rotating();
        }
    }

    void GatherInput()
    {
        Vector2 input2D = inputReader.MoveInput;
        Vector3 rawInput = new Vector3(input2D.x, 0, input2D.y);

        if (_cameraChange._isIsometric && Camera.main != null)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

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
            Quaternion rotation = Quaternion.LookRotation(_input, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _turnSpeed * Time.fixedDeltaTime);
        }
    }

    public void Move()
    {
        if (_cameraChange._isIsometric)
        {
            _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _speed * Time.deltaTime);
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void Rotating()
    {
        if (_input == Vector3.zero) return;

        Vector3 rotationAxis = Vector3.zero;

        // W o stick hacia adelante
        if (Input.GetKeyDown(KeyCode.W) || _input.z > 0.5f)
            rotationAxis = Vector3.left;

        // S o stick hacia atrás
        else if (Input.GetKeyDown(KeyCode.S) || _input.z < -0.5f)
            rotationAxis = Vector3.right;

        // A o stick hacia la izquierda
        else if (Input.GetKeyDown(KeyCode.A) || _input.x < -0.5f)
            rotationAxis = Vector3.forward;

        // D o stick hacia la derecha
        else if (Input.GetKeyDown(KeyCode.D) || _input.x > 0.5f)
            rotationAxis = Vector3.back;

        // Verificar si hay un cubo debajo
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance + 0.5f))
        {
            CubeRotation detectedCube = hit.collider.GetComponent<CubeRotation>();
            if (detectedCube != null)
            {
                _currentCube = detectedCube;
                _currentCube.StartBlinking();
            }
        }

        // Rotar el cubo si se detectó uno
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
                _currentCube?.StopBlinking();
                _currentCube = detectedCube;
                _currentCube.StartBlinking();
            }
        }
    }
}


