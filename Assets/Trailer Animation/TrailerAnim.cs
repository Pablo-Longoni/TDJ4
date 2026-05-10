using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
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

    [SerializeField] private Material _skybox;

    public Transform obj1;
    public Transform obj2;
    public Transform obj3;
    public Transform obj4;

    public Vector3 targetRot1 = new Vector3(0, 0, 0);
    public Vector3 targetRot2 = new Vector3(0, 0, 0);
    public Vector3 targetRot3 = new Vector3(0, 0, 0);
    public Vector3 targetRot4 = new Vector3(0, 0, -0);

    private void Start()
    {
        timerFigure = 3f;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.GetKeyDown(KeyCode.Space)) && !isPlaying)
        {
            isPlaying = true;
            StartCoroutine(PlayCinematic());
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        yield return new WaitForSeconds(.5f);

        _player.transform.SetParent(_cubeFinal.transform);

        yield return StartCoroutine(RotateTitle());
        Debug.Log("Cinemática finalizada");
        isPlaying = false;
    }

    private IEnumerator MoveUp(Transform cubeFinal)
    {
        Vector3 _startPosition = cubeFinal.position;
        Vector3 _finalPosition = new Vector3(-40.14f, 4.19f, 0);
        float duration = 1;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            cubeFinal.position = Vector3.Lerp(_startPosition, _finalPosition, normalized);
            yield return null;
        }
        cubeFinal.position = _finalPosition;
    }

    /*   private void SwithCamera()
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
       }*/

    private IEnumerator RotateTitle()
    {
        StartCoroutine(MoveUp(_cubeFinal));
        // Rotaciones iniciales locales
        Quaternion start1 = obj1.localRotation;
        Quaternion start2 = obj2.localRotation;
        Quaternion start3 = obj3.localRotation;
        Quaternion start4 = obj4.localRotation;

        // Rotaciones finales locales (estas las ponés vos en el inspector)
        Quaternion end1 = Quaternion.Euler(targetRot1);
        Quaternion end2 = Quaternion.Euler(targetRot2);
        Quaternion end3 = Quaternion.Euler(targetRot3);
        Quaternion end4 = Quaternion.Euler(targetRot4);

        // tiempos distintos para cada letra
        float dur1 = 1f;
        float dur2 = 1.4f;
        float dur3 = 1.8f;
        float dur4 = 2f;

        float elapsed1 = 0f;
        float elapsed2 = 0f;
        float elapsed3 = 0f;
        float elapsed4 = 0f;

        // Ejecutamos todas a la vez hasta que terminen
        while (elapsed1 < dur1 || elapsed2 < dur2 || elapsed3 < dur3 || elapsed4 < dur4)
        {
            if (elapsed1 < dur1)
            {
                elapsed1 += Time.deltaTime;
                float t1 = Mathf.Clamp01(elapsed1 / dur1);
                obj1.localRotation = Quaternion.Slerp(start1, end1, t1);
            }

            if (elapsed2 < dur2)
            {
                elapsed2 += Time.deltaTime;
                float t2 = Mathf.Clamp01(elapsed2 / dur2);
                obj2.localRotation = Quaternion.Slerp(start2, end2, t2);
            }

            if (elapsed3 < dur3)
            {
                elapsed3 += Time.deltaTime;
                float t3 = Mathf.Clamp01(elapsed3 / dur3);
                obj3.localRotation = Quaternion.Slerp(start3, end3, t3);
            }

            if (elapsed4 < dur4)
            {
                elapsed4 += Time.deltaTime;
                float t4 = Mathf.Clamp01(elapsed4 / dur4);
                obj4.localRotation = Quaternion.Slerp(start4, end4, t4);
            }

            yield return null;
        }

        // aseguramos que lleguen exacto
        obj1.localRotation = end1;
        obj2.localRotation = end2;
        obj3.localRotation = end3;
        obj4.localRotation = end4;
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