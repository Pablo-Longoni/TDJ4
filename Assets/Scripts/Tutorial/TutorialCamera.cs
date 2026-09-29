using System.Collections;
using UnityEngine;

public class TutorialSequence : MonoBehaviour
{
    [Header("Tutorial GameObjects")]
    [SerializeField] private GameObject tutorialCamera;
    [SerializeField] private GameObject rotateFigure;
    [SerializeField] private GameObject tutorialCamera2;

    [Header("Referencias")]
    [SerializeField] private CameraChange cameraChange;

    [Header("Tiempo")]
    [SerializeField] private float tiempoRotateFigure = 2f;

    private bool camaraCambiada = false;
    private bool tutorialFinalMostrado = false;

    private void Start()
    {
        tutorialCamera.SetActive(true);
        rotateFigure.SetActive(false);
        tutorialCamera2.SetActive(false);
    }

    private void Update()
    {
        if (cameraChange == null)
            return;

        // -----------------------------------------
        // CAMBIO A CÁMARA CENITAL
        // -----------------------------------------

        if (!camaraCambiada && !cameraChange._isIsometric)
        {
            camaraCambiada = true;

            tutorialCamera.SetActive(false);
            rotateFigure.SetActive(true);

            StartCoroutine(CambiarAlTutorialCamera2());
        }

        // -----------------------------------------
        // VOLVER A CÁMARA ISOMÉTRICA
        // -----------------------------------------

        if (tutorialFinalMostrado && cameraChange._isIsometric)
        {
            tutorialCamera2.SetActive(false);

            Debug.Log("Tutorial terminado.");

            tutorialFinalMostrado = false;
        }
    }

    private IEnumerator CambiarAlTutorialCamera2()
    {
        yield return new WaitForSeconds(tiempoRotateFigure);

        rotateFigure.SetActive(false);
        tutorialCamera2.SetActive(true);

        tutorialFinalMostrado = true;

        Debug.Log("Mostrando tutorialCamera2.");
    }
}