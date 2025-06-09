using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Telport : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    [SerializeField] private float cooldownTime = 0.5f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCooldown cooldown = other.GetComponent<PlayerCooldown>();
            if (cooldown != null && cooldown.canTeleport)
            {
                StartCoroutine(Teleport(other, cooldown));
            }
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
