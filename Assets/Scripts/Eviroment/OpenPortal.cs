using UnityEngine;

public class OpenPortal : MonoBehaviour
{
    [SerializeField] private GameObject _portal;
    private int _objectsInside = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Movable"))
        {
            Debug.Log("Entró: " + other.name);
            _objectsInside++;
            _portal.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Movable"))
        {
            _objectsInside--;

            // Evita valores negativos si algo sale mal
            _objectsInside = Mathf.Max(0, _objectsInside);

            if (_objectsInside == 0)
            {
                _portal.SetActive(false);
            }
        }
    }
}
