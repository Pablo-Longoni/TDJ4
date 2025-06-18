using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInputReader inputReader; // Asignar en el Inspector

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 13;
    [SerializeField] private float _turnSpeed = 500;
    [SerializeField] private float groundCheckDistance = 0.2f;

    public Vector3 _input;

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
            _input = rawInput.normalized;
        }
    }
    
    void Move()
{
    if (_cameraChange._isIsometric)
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        Vector3 velocity = _rb.velocity;

        // Movimiento horizontal siempre permitido
        velocity.x = _input.x * _speed;
        velocity.z = _input.z * _speed;

        // Si no hay suelo, permitimos caída natural
        if (!isGrounded)
        {
            // Dejá que la gravedad actúe, no tocamos velocity.y
            // Pero si querés hacerla más fuerte, podés sumarle algo como:
            // velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
        }
        else
        {
            // Si está en el suelo y no te estás moviendo, frenamos
            if (_input == Vector3.zero)
            {
                velocity.x = 0;
                velocity.z = 0;
            }
        }

        _rb.velocity = velocity;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    else
    {
        _rb.velocity = Vector3.zero;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
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

    public void Rotating()
    {
        if (_input == Vector3.zero) return;

        Vector3 rotationAxis = Vector3.zero;

        if (_input.z > 0.5f)
            rotationAxis = Vector3.right;
        else if (_input.z < -0.5f)
            rotationAxis = Vector3.left;
        else if (_input.x < -0.5f)
            rotationAxis = Vector3.forward;
        else if (_input.x > 0.5f)
            rotationAxis = Vector3.back;

        // Verificar cubo debajo
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance + 0.5f))
        {
            CubeRotation detectedCube = hit.collider.GetComponent<CubeRotation>();
            if (detectedCube != null)
            {
                _currentCube = detectedCube;
                _currentCube.StartBlinking();
            }
        }

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

            Transform currentFigure = hit.collider.transform;

            if (_currentFigure != currentFigure)
            {
                if (_currentFigure != null)
                {
                    int defaultLayer = LayerMask.NameToLayer(_defaultLayerName);
                    foreach (Transform t in _currentFigure.GetComponentsInChildren<Transform>(true))
                        t.gameObject.layer = defaultLayer;
                }

                int minimapLayer = LayerMask.NameToLayer(_minimapLayerName);
                foreach (Transform t in currentFigure.GetComponentsInChildren<Transform>(true))
                    t.gameObject.layer = minimapLayer;

                _currentFigure = currentFigure;
            }
        }
    }
}
