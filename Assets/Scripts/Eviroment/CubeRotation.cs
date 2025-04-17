using UnityEngine;
using System.Collections;
public class CubeRotation : MonoBehaviour
{
    private Quaternion _targetRotation;
    public float rotationSpeed = 5f;
    private bool _shouldRotate = false;
    public float _rotationTurn = 90f;
    private bool _canRotate = true;
    public float rotateCooldown = 2f;
    void Update()
    {
        if (_shouldRotate)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, _targetRotation) < 0.1f)
            {
                transform.rotation = _targetRotation;
                _shouldRotate = false;
            }
        }
    }

      public void RotateCube(Vector3 rotationAxis, Transform player)
      {
          if (!_shouldRotate && _canRotate)
          {
            Vector3 playerPos = player.position;

            // Calculamos offset desde el centro del cubo hasta el jugador
            Vector3 offsetToPlayer = playerPos - transform.position;
            // Guardar la posición relativa del jugador respecto al cubo antes de la rotación
            Vector3 localPlayerPos = transform.InverseTransformPoint(player.position);
            //  Debug.Log("localPos" + localPlayerPos);
              _targetRotation = Quaternion.AngleAxis(_rotationTurn, rotationAxis) * transform.rotation;
            _shouldRotate = true;
            StartCoroutine(RotationCooldown());


         //  StartCoroutine(AdjustPlayerPosition(player, offsetToPlayer));
        }
      }


    private IEnumerator AdjustPlayerPosition(Transform player, Vector3 originalOffset)
    {
        yield return new WaitUntil(() => !_shouldRotate);

        // Recalcular la dirección del offset después de la rotación
        Vector3 rotatedOffset = transform.rotation * Quaternion.Inverse(transform.rotation) * originalOffset;

        // Agregamos un pequeño offset hacia abajo (en la dirección opuesta a transform.up)
        Vector3 downwardOffset = -transform.up * 20f; // Ajustá el valor según el tamaño del cubo y del jugador

        // Nueva posición del cubo
        transform.position = player.position - rotatedOffset + downwardOffset;

        Debug.Log("Cube moved to stay under player with offset");
    }

    private IEnumerator RotationCooldown()
    {
        _canRotate = false;
        yield return new WaitForSeconds(rotateCooldown);
        _canRotate = true;
    }
}