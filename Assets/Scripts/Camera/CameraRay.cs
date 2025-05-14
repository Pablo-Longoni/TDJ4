using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.UI.Image;
using System.Collections;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class CameraRay : MonoBehaviour
{
    //  public GameObject _player;
    public float maxDistance = 1000f;
    public LayerMask platformMask;
    public CinemachineCamera _cinemachineCamera;
    [SerializeField] private float edgeDetectionDistance = 2f;
    public PlayerMovement _player;
    //planos material
    [SerializeField] public GameObject[] _planes;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _alignedMaterial;

    void Start()
    {
        
    }

    private void Update()
    {
        CroosPlanes();
    }

    public void CroosPlanes()
    {
          Vector3 origin = _cinemachineCamera.transform.position;
          Vector3 direction = _cinemachineCamera.transform.forward;

          Ray ray = new Ray(origin, direction);
          RaycastHit[] hits = Physics.RaycastAll(ray, 100f, platformMask);

          Debug.DrawRay(origin, direction * 100f, Color.yellow);

          if (hits.Length >= 2)
          {
          /*  foreach (GameObject plane in _planes)
            {
                var renderer = plane.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = _alignedMaterial;
                    Debug.Log("Material AZUL");
                }
            }*/
            //Debug.Log("Se alinean los planos");
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));


              Vector3 firstPlatformPoint = hits[0].point;
              Vector3 secondPlatformPoint = hits[1].point;

              // Opcional: mostrarlos en la escena
              Debug.DrawLine(firstPlatformPoint, secondPlatformPoint, Color.green);
   

              playerOnEdge(secondPlatformPoint);
          }
        /*  else
          {
            //  no  alineados volver a material por defecto
            foreach (GameObject plane in _planes)
            {
                var renderer = plane.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = _defaultMaterial;
                 //   Debug.Log("Material NORMAL");
                }
            }
          }*/


        /*  Vector3 origin = _cinemachineCamera.transform.position;
           Vector3 direction = _cinemachineCamera.transform.forward;

           Ray ray = new Ray(origin, direction);
           RaycastHit[] hits = Physics.RaycastAll(ray, 100f, platformMask);

           Debug.DrawRay(origin, direction * 100f, Color.yellow);

           if (hits.Length >= 2)
           {
               System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance)); // Ordena de más cercano a más lejano

               Vector3 firstPlatformPoint = hits[0].point;
               Vector3 secondPlatformPoint = hits[1].point;

               Debug.DrawLine(firstPlatformPoint, secondPlatformPoint, Color.green);

               // Dirección de movimiento del jugador
               Vector3 moveDir = _player._input.normalized;

               // Check si está al borde
               Vector3 frontCheck = _player.transform.position + moveDir * edgeDetectionDistance;
               bool isEdge = !Physics.Raycast(frontCheck, Vector3.down, 2f, platformMask);

               if (isEdge && moveDir != Vector3.zero)
               {
                   Vector3 targetPoint;

                   float distToFirst = Vector3.Distance(_player.transform.position, firstPlatformPoint);
                   float distToSecond = Vector3.Distance(_player.transform.position, secondPlatformPoint);
                   float forwardOffset = 3f;
                   // Si está más cerca del segundo plano, y se mueve hacia atrás => volver
                   if (distToSecond < distToFirst)
                       targetPoint = firstPlatformPoint;
                   else
                       targetPoint = secondPlatformPoint;

                   Vector3 newPlayerPos = targetPoint + moveDir *  forwardOffset;
                   newPlayerPos.y += 1f;

                   StartCoroutine(MovePlayerSmoothly(newPlayerPos));
               }
           }*/
    }

    public void playerOnEdge(Vector3 targetPoint)
    {
        Vector3 direction = _player._input.normalized;

        Vector3 frontCheck = _player.transform.position + direction * edgeDetectionDistance;
        bool isEdge = !Physics.Raycast(frontCheck, Vector3.down, 2f, platformMask);

        if (isEdge)
        {
            float forwardOffset = 3.5f; 
            Vector3 newPlayerPos = targetPoint + direction * forwardOffset;
            newPlayerPos.y += 1f;


            StartCoroutine(MovePlayerSmoothly(newPlayerPos));
         //   _player.transform.position = newPlayerPos;
        }
    }

    IEnumerator MovePlayerSmoothly(Vector3 target)
    {
          float t = 0;
          Vector3 start = _player.transform.position;

          while (t < 1f)
          {
              t += Time.deltaTime * 3f; // velocidad
              _player.transform.position = Vector3.Lerp(start, target, t);
              yield return null;
          }
      
    }
}
