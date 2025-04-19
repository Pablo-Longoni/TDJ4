using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Vector3 _input;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    private CameraChange _cameraChange;
    public CubeRotation [] _cubeRotation;
    //  [SerializeField] private float edgeDetectionDistance = 1f;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private void Start()
    {
        _cameraChange = FindAnyObjectByType<CameraChange>();
    }
    void Update()
    {
        GatherInput();

        if (_cameraChange._isIsometric)
        {
            Look();

        }
        else
        {
            Rotating();
        }
    }

    void FixedUpdate()
    {
        Move();
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

        /*   if (_cameraChange._isIsometric && Camera.main != null)
           {
               _input = rawInput; // Cámara totalmente cenital, usar input sin proyectar
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
               _input = rawInput; // Cámara totalmente cenital, usar input sin proyectar
           }*/
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _turnSpeed * Time.deltaTime);
        }
    }

    public void Move()
    {
        if (_cameraChange._isIsometric)
        {
            _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _speed * Time.deltaTime);
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            //    Debug.Log("Movimiento isometrico");
        }
        else
        {
            /* _rb.MovePosition(_rb.position + _input * _speed * Time.fixedDeltaTime);
             _rb.transform.rotation = (Quaternion.Euler(0, 0, 0));
             _rb.constraints = RigidbodyConstraints.FreezePositionY;*/
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            //  Debug.Log("Movimiento cential");
        }
    }

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

      //  _cubeRotation.RotateCube(rotationAxis, transform);
          if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance + 0.5f))
          {
              CubeRotation currentPlatform = hit.collider.GetComponent<CubeRotation>();
              if (currentPlatform != null)
              {
                  currentPlatform.RotateCube(rotationAxis, transform);
               //   Debug.Log("Rotando");
              }
          }

    }
}
   
        /* private void CheckForEdge()
         {
             if (_input == Vector3.zero) return;

             Vector3 direction = _input.normalized;
             Vector3 checkPosFront = transform.position + direction * edgeDetectionDistance;

             // Línea 1: raycast abajo desde el centro actual
             bool isOnGround = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitDown, groundCheckDistance);

             // Línea 2: raycast abajo desde adelante (verifica si hay borde)
             bool isEdge = !Physics.Raycast(checkPosFront, Vector3.down, groundCheckDistance);

             // Línea 3: raycast (o spherecast) en dirección diagonal
             Vector3 diagonalDir = (direction + Vector3.down).normalized;
             bool foundAdjacentFace = Physics.Raycast(checkPosFront, diagonalDir, out RaycastHit hitDiagonal, groundCheckDistance * 4f);

           //  Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.green, 10f);
           //  Debug.DrawRay(checkPosFront, Vector3.down * groundCheckDistance, Color.red, 10f);
             Debug.DrawRay(checkPosFront, diagonalDir.normalized * groundCheckDistance * 4f, Color.red,5f);

             if (isOnGround && isEdge)
             {
                 Vector3 rotationAxis = Vector3.zero;

                 if (direction == Vector3.forward)
                     rotationAxis = Vector3.left;
                 else if (direction == Vector3.back)
                     rotationAxis = Vector3.right;
                 else if (direction == Vector3.right)
                     rotationAxis = Vector3.forward;
                 else if (direction == Vector3.left)
                     rotationAxis = Vector3.back;

                     CubeRotation currentPlatform = null;

                     if (foundAdjacentFace)
                     {
                         Debug.Log("Cara adyacente detectada: giro de 90°");
                         currentPlatform = hitDiagonal.collider.GetComponent<CubeRotation>();
                         if (currentPlatform != null)
                         {
                             currentPlatform._rotationTurn = 90;
                             currentPlatform.RotateCube(rotationAxis, transform);
                             Debug.Log("90°");
                         }
                     }
                     else
                     {
                         Debug.Log("Sin cara adyacente: giro de 180°");
                         currentPlatform = hitDown.collider?.GetComponent<CubeRotation>();
                         if (currentPlatform != null)
                         {
                             currentPlatform._rotationTurn = 180;
                             currentPlatform.RotateCube(rotationAxis, transform);
                             Debug.Log("180°");
                         }
                     }

               /*  RaycastHit hit;
                 if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance + 0.5f))
                 {
                     CubeRotation currentPlatform = hit.collider.GetComponent<CubeRotation>();
                     if (currentPlatform != null)
                     {
                         currentPlatform.RotateCube(rotationAxis, transform);
                     }
                 }
             }
         }*/
    
