using Unity.VisualScripting;
using UnityEngine;

public class UpgradeTransforms : MonoBehaviour
{
    [SerializeField] private PlayerTransformation _playerTransformation;

    [SerializeField] private GameObject _item;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerTransformation.TransformUpgrade();
            _item.SetActive(false);
            Debug.Log("Flip upgrade");
        }
    }
}
