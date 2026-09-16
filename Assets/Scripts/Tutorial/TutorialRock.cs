using System.Collections;
using UnityEngine;
using TMPro;

public class RockTutorial : MonoBehaviour
{
    [Header("Tutoriales")]
    [SerializeField] private TMP_Text tutorialRock;
    [SerializeField] private TMP_Text tutorialRock2;

    [Header("Player")]
    [SerializeField] private PlayerGrab playerGrab;

    [Header("Configuración")]
    [SerializeField] private float tiempoParaOcultar = 1f;
    [SerializeField] private float tiempoTutorialRelease = 2f;

    private bool grabDetectado = false;
    private Coroutine tutorialReleaseCoroutine;

    private void Start()
    {
        // Tutorial inicial
        if (tutorialRock != null)
            tutorialRock.gameObject.SetActive(true);

        // Tutorial de release comienza oculto
        if (tutorialRock2 != null)
            tutorialRock2.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerGrab == null)
            return;

        // Detectar que agarró la roca
        if (!grabDetectado && playerGrab.isGrabbed)
        {
            grabDetectado = true;

            StartCoroutine(OcultarTutorialRock());
        }
    }

    private IEnumerator OcultarTutorialRock()
    {
        yield return new WaitForSeconds(tiempoParaOcultar);

        if (tutorialRock != null)
            tutorialRock.gameObject.SetActive(false);
    }

    public void MostrarTutorialRelease()
    {
        // Solo mostrar si está llevando la roca
        if (!playerGrab.isGrabbed)
            return;

        // Si ya había una coroutine funcionando, detenerla
        if (tutorialReleaseCoroutine != null)
        {
            StopCoroutine(tutorialReleaseCoroutine);
        }

        // Mostrar tutorial nuevamente
        if (tutorialRock2 != null)
        {
            tutorialRock2.gameObject.SetActive(true);

            // Iniciar nuevamente los 2 segundos
            tutorialReleaseCoroutine = StartCoroutine(
                OcultarTutorialRelease()
            );
        }

        Debug.Log("TUTORIAL ROCK 2 MOSTRADO");
    }

    private IEnumerator OcultarTutorialRelease()
    {
        yield return new WaitForSeconds(tiempoTutorialRelease);

        if (tutorialRock2 != null)
        {
            tutorialRock2.gameObject.SetActive(false);
        }

        tutorialReleaseCoroutine = null;

        Debug.Log("TUTORIAL ROCK 2 OCULTO");
    }
}