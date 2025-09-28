using UnityEngine;

public class CameraMirror : MonoBehaviour
{
   /* public float _rotationSpeed = 1000f;
    public PlayerInputReader inputReader;
    public Transform _mirrorCamera;
    public float minY = -45f;
    public float maxY = 45f;

    private float _currentYRotation;
    private float _fixedXRotation;
    private float _fixedZRotation;*/

  /*  private void Start()
    {
        // Guardar las inclinaciones iniciales de la cámara (X y Z no deberían cambiar)
        Vector3 startAngles = _mirrorCamera.localEulerAngles;
        _fixedXRotation = startAngles.x;
        _fixedZRotation = startAngles.z;

        // Solo acumulamos Y
        _currentYRotation = startAngles.y;
    }

    void Update()
    {
        float rotationInput = 0f;

        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            rotationInput += mouseX;
        }

        if (Input.GetButton("RotateLeft"))
            rotationInput -= 1f;

        if (Input.GetButton("RotateRight"))
            rotationInput += 1f;

        Vector2 look = inputReader.LookInput;
        if (Mathf.Abs(look.x) > 0.1f)
            rotationInput += look.x;

        // Acumular rotación en Y
        _currentYRotation += rotationInput * _rotationSpeed * Time.deltaTime;

        // Limitarla
        _currentYRotation = Mathf.Clamp(_currentYRotation, minY, maxY);

        // Aplicar manteniendo fijo X y Z
        _mirrorCamera.localEulerAngles = new Vector3(
            _fixedXRotation,
            _currentYRotation,
            _fixedZRotation
        );
        /*    public Transform _playerCamera;
            public Transform _mirrorCamera;

            private void Update()
            {
                Vector3 _posY = new Vector3(transform.position.x, _playerCamera.transform.position.y, transform.position.z);
                Vector3 _side1 = _playerCamera.transform.position - _posY;
                Vector3 _side2 = transform.forward;

                float angle = Vector3.SignedAngle(_side1, _side2, Vector3.up);
                _mirrorCamera.localEulerAngles = new Vector3(0, angle, 0);
            }
    }*/
}