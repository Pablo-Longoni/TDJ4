using UnityEngine;
using System.Collections;
public class CubeRotation : MonoBehaviour
{
    private Quaternion _targetRotation;
    private float rotationSpeed = 200f;
    private bool _shouldRotate = false;
    public float _rotationTurn = 90f;
    private bool _canRotate = true;
    public float _rotateCooldown = 2f;

    void Update()
    {
        if (_shouldRotate)
        {
          //  Debug.Log("Rotando cubo en Update");
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, _targetRotation) < 0.1f)
            {
                transform.rotation = _targetRotation;
                _shouldRotate = false;
            //    Debug.Log("Rotación completada");
            }
        }
    }

      public void RotateCube(Vector3 rotationAxis, Transform player)
      {
          if (!_shouldRotate && _canRotate)
          {

            _targetRotation = Quaternion.AngleAxis(_rotationTurn, rotationAxis) * transform.rotation;
            _shouldRotate = true;

            StartCoroutine(RotationCooldown());
        }
      }
      private IEnumerator RotationCooldown()
      {
       _canRotate = false;
        yield return new WaitForSeconds(_rotateCooldown);
       _canRotate = true;
      }

}