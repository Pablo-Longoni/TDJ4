using System.Collections;
using TMPro;
using UnityEngine;

public class MovementTutorial : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private PlayerInputReader playerInputReader;

    [Header("Tiempos")]
    [SerializeField] private float tiempoParaMostrar = 1f;
    [SerializeField] private float tiempoParaOcultar = 2f;

    private bool tutorialMostrado = false;
    private bool movimientoDetectado = false;

    private void Start()
    {
        // Comienza oculto
        tutorialText.gameObject.SetActive(true);

        // Mostrar después de 1 segundo
        StartCoroutine(MostrarTutorial());
    }

    private IEnumerator MostrarTutorial()
    {
        yield return new WaitForSeconds(tiempoParaMostrar);

        tutorialText.gameObject.SetActive(true);
        tutorialMostrado = true;
    }

    private void Update()
    {
        // Si todavía no apareció o ya detectamos movimiento,
        // no hacemos nada.
        if (!tutorialMostrado || movimientoDetectado)
            return;

        // Detectar cualquier movimiento
        if (playerInputReader.MoveInput != Vector2.zero)
        {
            movimientoDetectado = true;

            // Esperar 2 segundos y ocultar
            StartCoroutine(OcultarTutorial());
        }
    }

    private IEnumerator OcultarTutorial()
    {
        yield return new WaitForSeconds(tiempoParaOcultar);

        tutorialText.gameObject.SetActive(false);
    }
}