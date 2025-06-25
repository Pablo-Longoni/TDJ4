using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Telport : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    [SerializeField] private float cooldownTime = 0.5f;
    [SerializeField] private PlayerGrab _playerGrab;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _playerGrab.isGrabbed == false)
        {
            PlayerCooldown cooldown = other.GetComponent<PlayerCooldown>();
            if (cooldown != null && cooldown.canTeleport)
            {
                StartCoroutine(Teleport(other, cooldown));
            }
        }
        else
        {
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
}
