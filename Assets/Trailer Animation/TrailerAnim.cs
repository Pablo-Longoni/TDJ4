using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
//using Unity.VisualScripting;
//using static UnityEditor.Experimental.GraphView.GraphView;
public class TrailerAnim : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera; // Asegurate que este sea el VirtualCamera
    [SerializeField] private CinemachineCamera _cenitalCamera;
    [SerializeField] private float _rotationSpeedY = 20f;
 //   [SerializeField] private float _rotationSpeedZ = 20f;
    [SerializeField] private GameObject[] _figures;
    [SerializeField] private Transform _cameraRig; // Empty parent para rotar la cámara
    [SerializeField] private float yRotationLimit = 315f;
    [SerializeField] private float timerFigure = 0;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _cubeFinal;

  //  private float _currentCubeRotation = 0;
 //   private float _finalCubeLimit = 90;
    private float currentYRotation = 0f;

    private int currentFigureIndex = 0;
 //   private GameObject currentFigure = null;
    private bool isPlaying = false;
  //  [SerializeField] private Material _opaqueMaterial;
    [SerializeField] private Canvas _canvas;

    private void Start()
    {
        timerFigure = 3f;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPlaying)
        {
            isPlaying = true;
            StartCoroutine(PlayCinematic());
        }
    }

    private IEnumerator PlayCinematic()
    {
        // Rotar en eje Y
        while (currentYRotation < yRotationLimit)
        {
            float rotationStep = _rotationSpeedY * Time.deltaTime;
            currentYRotation += rotationStep;
            _cameraRig.Rotate(0f, rotationStep, 0f);
          
            timerFigure += Time.deltaTime;
            // Cambiar figuras cada 40°
            if (timerFigure >= 3.2)
            {
                ChangeFigures();
                timerFigure = 0;
                Debug.Log("Figura cambiada");
            }

            yield return null;
        }

    /*    MeshRenderer renderer = _cubeFinal.gameObject.GetComponent<MeshRenderer>();
        renderer.material = _opaqueMaterial;*/

        SwithCamera();

        yield return new WaitForSeconds(3.5f);

        // Rotar cubo en eje Y
        _player.transform.SetParent(_cubeFinal.transform);
        _canvas.gameObject.SetActive(true);
        yield return StartCoroutine(RotateCube(_cubeFinal, Vector3.left, 90f, 1.5f));

        // Esperar 1 segundo
        yield return new WaitForSeconds(3f);

        // Rotar cubo en eje X
     //   yield return StartCoroutine(RotateCube(_cubeFinal, Vector3.left, 90f, 2f));
        Debug.Log("Cinemática finalizada");
    }

    private void SwithCamera()
    {
        _camera.Priority = 1;
        _cenitalCamera.Priority = 2;
    }

    private IEnumerator RotateCube(Transform target, Vector3 axis, float angle, float duration)
    {
        Quaternion startRotation = target.rotation;
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(angle, axis);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            target.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        target.rotation = endRotation;
    }

    private void ChangeFigures()
    {
        // Desactivar todos
        foreach (var go in _figures)
              go.SetActive(false);

          // Activar uno por uno en orden
          if (_figures.Length > 0)
          {
              _figures[currentFigureIndex % _figures.Length].SetActive(true);
              currentFigureIndex++;
          }
      /*  if (_figures.Length == 0) return;

        GameObject nextFigure = _figures[currentFigureIndex % _figures.Length];
        StartCoroutine(FadeOutAndIn(currentFigure, nextFigure, 1f));
    //    StartCoroutine(ScaleTransition(currentFigure, nextFigure, 0.5f));
        currentFigure = nextFigure;
        currentFigureIndex++;*/

        /*   if (_figures.Length == 0) return;

           GameObject nextFigure = _figures[currentFigureIndex % _figures.Length];
           StartCoroutine(ScaleTransition(currentFigure, nextFigure, 0.5f));
           currentFigure = nextFigure;
           currentFigureIndex++;*/
    }

    IEnumerator FadeOutAndIn(GameObject current, GameObject next, float duration)
    {
        float t = 0f;

        MeshRenderer currentR = current != null ? current.GetComponentInChildren<MeshRenderer>() : null;
        MeshRenderer nextR = next != null ? next.GetComponentInChildren<MeshRenderer>() : null;

        // Obtenemos el color original (con alfa original también)
        Color currentColor = currentR != null ? currentR.material.color : Color.white;
        Color nextColor = nextR != null ? nextR.material.color : Color.white;

        // Activar el siguiente objeto y dejarlo transparente
        if (next != null) next.SetActive(true);
        if (nextR != null)
            nextR.material.color = new Color(nextColor.r, nextColor.g, nextColor.b, 0f);

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            if (currentR != null)
            {
                Color c = currentR.material.color;
                c.a = 1f - normalized;
                currentR.material.color = c;
            }

            if (nextR != null)
            {
                Color c = nextR.material.color;
                c.a = normalized;
                nextR.material.color = c;
            }

            yield return null;
        }

        // Al final, desactivar el objeto actual
        if (current != null)
            current.SetActive(false);
    }

    IEnumerator ScaleTransition(GameObject current, GameObject next, float duration)
    {
      //  float t = 0f;

        Vector3 originalScale = next.transform.localScale;
        if (current != null)
        {
            StartCoroutine(ScaleObject(current, Vector3.zero, duration));
        }

        next.transform.localScale = Vector3.zero;
        next.SetActive(true);

        yield return StartCoroutine(ScaleObject(next, originalScale, duration));

        if (current != null)
            current.SetActive(false);
    }

    IEnumerator ScaleObject(GameObject obj, Vector3 targetScale, float duration)
    {
        float t = 0f;
        Vector3 startScale = obj.transform.localScale;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, normalized);
            yield return null;
        }
    }
}