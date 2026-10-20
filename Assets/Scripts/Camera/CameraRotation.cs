using System.Collections;
using UnityEngine;
using TMPro;

public class CameraRotation : MonoBehaviour
{
    [Header("Rotation settings")]
    public float _rotationSpeed = 360f;
    [SerializeField] private CameraChange _cameraChange;
    [SerializeField] private PlayerInputReader _inputReader;
    [Header("Mouse drag settings")]
    [SerializeField] private float mouseDragSensitivity = 5f;

    [Header("Tutorial drag")]
    [SerializeField] private TMP_Text dragTutorial;
    [SerializeField] private float tutorialDuration = 2f;

    private bool rotationDetected = false;

    private void Awake()
    {
        _inputReader = FindFirstObjectByType<PlayerInputReader>();
    }

    private void Start()
    {
        _rotationSpeed = PlayerPrefs.GetFloat("RotationSpeed", 360);

        if (dragTutorial != null)
        {
            dragTutorial.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (!_cameraChange._isIsometric)
            return;

        bool isRotating = false;


        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");

            if (Mathf.Abs(mouseX) > 0.01f)
            {
                isRotating = true;

                transform.Rotate(Vector3.up, mouseX * mouseDragSensitivity);
            }
        }

        if (Input.GetButton("RotateLeft"))
        {
            isRotating = true;

            transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime);
        }


        if (Input.GetButton("RotateRight"))
        {
            isRotating = true;

            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        }

        Vector2 look = _inputReader.LookInput;

        if (Mathf.Abs(look.x) > 0.1f)
        {
            isRotating = true;

            transform.Rotate(Vector3.up, look.x * _rotationSpeed * Time.deltaTime);
        }

        if (isRotating && !rotationDetected)
        {
            rotationDetected = true;

            StartCoroutine(HideDragTutorial());
        }
    }

    private IEnumerator HideDragTutorial()
    {
        yield return new WaitForSeconds(tutorialDuration);

        if (dragTutorial != null)
        {
            dragTutorial.gameObject.SetActive(false);
        }
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetRotationSpeed(float newSpeed)
    {
        _rotationSpeed = newSpeed;
    }
}