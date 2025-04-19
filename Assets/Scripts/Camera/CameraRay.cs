using Unity.Cinemachine;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    //  public GameObject _player;
    public float maxDistance = 100f;
    public LayerMask platformMask;
    public CinemachineCamera _cinemachineCamera;
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
            Debug.Log("Se alinean los planos");
        }
    }
}
