using UnityEngine;

public class GrassCovering : MonoBehaviour
{
    [Header("Grass Prefabs")]
    [SerializeField] private GameObject _grass1;
    [SerializeField] private GameObject _grass2;

    [Header("Generation")]
    [SerializeField] private int _grassAmount = 100;
    [SerializeField] private float _distanceBetweenGrass = 0.5f;

    [Header("Rotation")]
    [SerializeField] private Vector3 _rotation = Vector3.zero;
    [SerializeField] private bool _randomRotation = true;

    [Header("Area")]
    [SerializeField] private Vector2 _areaSize = new Vector2(10, 10);

    private Transform _grassParent;

    public void GenerateGrass()
    {
        ClearGrass();

        _grassParent = new GameObject("Generated Grass").transform;
        _grassParent.SetParent(transform);

        for (int i = 0; i < _grassAmount; i++)
        {
            Vector3 position = GetRandomPosition();

            GameObject prefab = GetRandomGrassPrefab();

            if (prefab == null)
                continue;

            Quaternion grassRotation = GetGrassRotation();

            GameObject grass = Instantiate(
                prefab,
                position,
                grassRotation,
                _grassParent
            );

            grass.name = $"Grass_{i}";
        }
    }

    public void ClearGrass()
    {
        Transform existingGrass = transform.Find("Generated Grass");

        if (existingGrass == null)
            return;

        DestroyImmediate(existingGrass.gameObject);
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(
            -_areaSize.x / 2f,
            _areaSize.x / 2f
        );

        float z = Random.Range(
            -_areaSize.y / 2f,
            _areaSize.y / 2f
        );

        return transform.TransformPoint( new Vector3(x, 0, z));
    }

    private GameObject GetRandomGrassPrefab()
    {
        if (_grass1 == null && _grass2 == null)
            return null;

        if (_grass1 != null && _grass2 != null)
            return Random.value > 0.5f ? _grass1 : _grass2;

        return _grass1 != null ? _grass1 : _grass2;
    }

    private Quaternion GetGrassRotation()
    {
        if (_randomRotation)
        {
            float randomY = Random.Range(0f, 360f);

            Vector3 rotation = _rotation;
            rotation.y += randomY;

            return Quaternion.Euler(rotation);
        }

        return Quaternion.Euler(_rotation);
    }
}