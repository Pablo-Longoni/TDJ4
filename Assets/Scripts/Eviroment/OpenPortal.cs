using TMPro;
using UnityEngine;

public class OpenPortal : MonoBehaviour
{
    [SerializeField] public GameObject _portal;
    private int _objectsInside = 0;
 //   [SerializeField] private ParticleSystem _particles;

    Vector3 _position;

    private void Start()
    {
        _position = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Movable"))
        {
            Debug.Log("Entró: " + other.name);
            _objectsInside++;
            _portal.SetActive(true);
            MeshRenderer _renderer = GetComponent<MeshRenderer>();
            _renderer.material.color = Color.black;
        //    Instantiate(_particles, transform.position, Quaternion.identity);
            AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._portal);
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
                MeshRenderer _renderer = GetComponent<MeshRenderer>();
                _renderer.material.color = Color.white;
            }
        }
    }
}
