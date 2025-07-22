using UnityEngine;
using System.Collections;

using UnityEngine.InputSystem;
public class CubeMenu : MonoBehaviour
{

    public float rotationSpeed = 90f; // grados por segundo
    public bool onStage = false;
    private Coroutine _rotationCoroutine;
    //cooldown
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
          /*  yield return RotateBy(Vector3.up);       // Rota 90° en Y
            yield return new WaitForSeconds(3f);*/

            yield return RotateBy(Vector3.forward);  // Rota 90° en Z
            yield return new WaitForSeconds(2f);

          /*  yield return RotateBy(Vector3.right);    // Rota 90° en X
            yield return new WaitForSeconds(3f);*/
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

        // Corregir a exactamente 90° (por acumulación de flotantes)
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
          //  StopCoroutine(_rotationCoroutine);
            if (Input.GetKeyDown(KeyCode.A))
            {
               StartCoroutine( RotateBy(Vector3.back));
                StartCoroutine(RotationCooldown());
            }
            else if (Input.GetKeyDown(KeyCode.D))
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
