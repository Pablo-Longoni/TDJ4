using UnityEngine;

public class RockReleaseTutorialTrigger : MonoBehaviour
{
    [SerializeField] private RockTutorial rockTutorial;
    [SerializeField] private PlayerGrab playerGrab;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!playerGrab.isGrabbed)
            return;

        rockTutorial.MostrarTutorialRelease();
    }
}