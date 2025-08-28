using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CubeMenu : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public bool onStage = false;
    private Coroutine _rotationCoroutine;

    // cooldown
    private float _rotateCooldown = 1f;
    private bool _isInCooldown = false;

    private void Start()
    {
        _rotationCoroutine = StartCoroutine(RotateCycle());
    }

    IEnumerator RotateCycle()
    {
        while (true)
        {
            yield return RotateBy(Vector3.forward);
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator RotateBy(Vector3 axis)
    {
        float rotated = 0f;
        while (rotated < 90f)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(axis, step);
            rotated += step;
            yield return null;
        }


        transform.Rotate(axis, 90f - rotated);
    }

    public void StopRotation()
    {
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = null;
        }
    }

    private void Update()
    {
        if (onStage && !_isInCooldown)
        {

            if (Input.GetKeyDown(KeyCode.A) ||
               (Gamepad.current != null && Gamepad.current.leftShoulder.wasPressedThisFrame))
            {
                StartCoroutine(RotateBy(Vector3.back));
                StartCoroutine(RotationCooldown());
            }


            else if (Input.GetKeyDown(KeyCode.D) ||
                    (Gamepad.current != null && Gamepad.current.rightShoulder.wasPressedThisFrame))
            {
                StartCoroutine(RotateBy(Vector3.forward));
                StartCoroutine(RotationCooldown());
            }
        }
    }

    private IEnumerator RotationCooldown()
    {
        _isInCooldown = true;
        yield return new WaitForSeconds(_rotateCooldown);
        _isInCooldown = false;
    }
}