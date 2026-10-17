using UnityEngine;

public class PlayerDashAfterImage : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 0.5f;
    private MeshRenderer[] _renderers;
    private Material[] _materials;
    private float _startTime;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();

        _materials = new Material[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _materials[i] = _renderers[i].material;
        }

        _startTime = Time.time;
    }

    private void Update()
    {
        float t = (Time.time - _startTime) / _fadeDuration;
        Color color;

        for(int i = 0; i < _materials.Length; i++)
        {
            color = _materials[i].color;
            color.a = Mathf.Lerp(0.5f, 0, t);
            _materials[i].color = color;
        }
    }
}
