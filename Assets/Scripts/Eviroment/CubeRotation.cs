using UnityEngine;
using System.Collections;

public class CubeRotation : MonoBehaviour
{
  private Quaternion _targetRotation;
  private float rotationSpeed = 200f;
  private bool _shouldRotate = false;
  public float _rotationTurn = 90f;
  public bool _canRotate = true;
  public float _rotateCooldown = 2f;

  [SerializeField] public AudioManager _audioManager;

  private bool _isInCooldown = false;

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
    if (_canRotate && !_shouldRotate && !_isInCooldown)
    {
      // Calcular la nueva rotación objetivo
      Quaternion newTargetRotation = Quaternion.AngleAxis(_rotationTurn, rotationAxis) * transform.rotation;

      // Solo rotar si hay una diferencia real
      if (Quaternion.Angle(transform.rotation, newTargetRotation) > 0.1f)
      {
        _targetRotation = newTargetRotation;
        _shouldRotate = true;

        // Reproducir sonido y comenzar cooldown
        _audioManager.playSound(_audioManager._turning);
        StartCoroutine(RotationCooldown());
      }
    }
  }

  private IEnumerator RotationCooldown()
  {
    _isInCooldown = true;
    _canRotate = false;

    yield return new WaitForSeconds(_rotateCooldown);

    _canRotate = true;
    _isInCooldown = false;
  }
}