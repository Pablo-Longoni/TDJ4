using System.Collections;
using UnityEngine;
using TMPro;

public class CameraRotation : MonoBehaviour
{
    public float _rotationSpeed = 1000f;
    public CameraChange _cameraChange;
    public PlayerInputReader inputReader;

    [Header("Tutorial")]
    [SerializeField] private TMP_Text dragTutorial;
    [SerializeField] private float tutorialDuration = 2f;

    private bool rotationDetected = false;

    private void Start()
    {
        _rotationSpeed = PlayerPrefs.GetFloat("RotationSpeed", 360);
        Debug.Log("Rotation: " + _rotationSpeed);

        // El tutorial comienza visible
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

                transform.Rotate(
                    Vector3.up,
                    mouseX * _rotationSpeed * Time.deltaTime
                );
            }
        }

        if (Input.GetButton("RotateLeft"))
        {
            isRotating = true;

            transform.Rotate(
                Vector3.up,
                -_rotationSpeed * Time.deltaTime
            );
        }


        if (Input.GetButton("RotateRight"))
        {
            isRotating = true;

            transform.Rotate(
                Vector3.up,
                _rotationSpeed * Time.deltaTime
            );
        }


        Vector2 look = inputReader.LookInput;

        if (Mathf.Abs(look.x) > 0.1f)
        {
            isRotating = true;

            transform.Rotate(
                Vector3.up,
                look.x * _rotationSpeed * Time.deltaTime
            );
        }

       
        if (isRotating && !rotationDetected)
        {
            rotationDetected = true;

            StartCoroutine(OcultarDragTutorial());
        }
    }

    private IEnumerator OcultarDragTutorial()
    {
        // El tutorial permanece visible durante 2 segundos
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