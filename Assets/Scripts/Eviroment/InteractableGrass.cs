using UnityEngine;

[ExecuteAlways]
public class InteractableGrass : MonoBehaviour
{
   [SerializeField] private Transform _player;

    [SerializeField] private Material _material;

    private void Update()
    {
        _material?.SetVector("_Character", _player.position);
    }
}
