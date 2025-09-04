using UnityEngine;
using System.Collections;
/*using Unity.VisualScripting;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;*/
public class Telport : MonoBehaviour
{
    [SerializeField] public Transform _destination;
    [SerializeField] private float cooldownTime = 0.5f;
    [SerializeField] public PlayerGrab _playerGrab;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] public CubeAnimation _cubeAnimation;
    [SerializeField] public CameraShake _cameraShake;
    [SerializeField] public AudioManager _audioManager;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && _playerGrab.isGrabbed == false)
        {
            PlayerCooldown cooldown = other.GetComponent<PlayerCooldown>();
           if (cooldown != null && cooldown.canTeleport)
            {
                   StartCoroutine(Teleport(other, cooldown));
                _cameraShake.Shake(0.5f, 0.5f, 0.5f);
              //  StartCoroutine(MovePlayerToPortal(other.transform, cooldown));
            }
        }
        else
        {
          //  _cameraShake.Shake(0.5f, 0.5f, 0.5f);
          //  AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._portal);
            Debug.Log("soltaa el cubooo");
        }
    }

     private IEnumerator Teleport(Collider other, PlayerCooldown cooldown)
     {
         cooldown.canTeleport = false;
         other.transform.position = _destination.position;
         yield return new WaitForSeconds(cooldownTime);
         cooldown.canTeleport = true;
     }
 /*   private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCooldown cooldown = other.GetComponent<PlayerCooldown>();
            if (cooldown != null)
                cooldown.canTeleport = true;
        }
    }*/
    private IEnumerator MovePlayerToPortal(Transform playerTransform, PlayerCooldown cooldown)
    {
        _playerGrab.enabled = false;
        cooldown.canTeleport = false;

        // 1. Mover al jugador hacia el centro del portal (posición del collider)
        Vector3 startPos = playerTransform.position;
        Vector3 targetPos = transform.position;
        float timeElapsed = 0f;
        float moveDuration = 0.7f; // tiempo para llegar al portal

        _cubeAnimation.EnterPortalAnim();
        while (timeElapsed < moveDuration)
        { 
            timeElapsed += Time.deltaTime * _moveSpeed; 
            playerTransform.position = Vector3.Lerp(startPos, targetPos, timeElapsed / moveDuration);
            yield return null; 
        }
        // 2 Teletransportar
        playerTransform.position = _destination.position;
        _cubeAnimation.ExitPortalAnim();
        yield return new WaitForSeconds(cooldownTime);

        cooldown.canTeleport = true;
        _playerGrab.enabled = true;
    }
}