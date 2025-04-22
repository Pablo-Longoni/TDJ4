using UnityEngine;
using System.Collections;
public class CubeMenu : MonoBehaviour
{

    public float rotationSpeed = 90f; // grados por segundo

    private void Start()
    {
        StartCoroutine(RotateCycle());
    }

    IEnumerator RotateCycle()
    {
        while (true)
        {
            yield return RotateBy(Vector3.up);       // Rota 90° en Y
            yield return new WaitForSeconds(3f);

            yield return RotateBy(Vector3.forward);  // Rota 90° en Z
            yield return new WaitForSeconds(3f);

            yield return RotateBy(Vector3.right);    // Rota 90° en X
            yield return new WaitForSeconds(3f);
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
}
