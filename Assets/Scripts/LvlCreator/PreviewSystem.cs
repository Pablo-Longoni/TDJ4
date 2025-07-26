using System;
using UnityEditor.SpeedTree.Importer;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float _previewYOffset = 0.06f;

    [SerializeField] private GameObject _cellIndicator;
     private GameObject _previewObject;

    [SerializeField] private Material _previewMaterialPrefab;
     private Material _previewMaterialInstance;

    private Renderer _cellIndicatorRenderer;
    private void Start()
    {
        _previewMaterialInstance = new Material(_previewMaterialPrefab);
        _cellIndicator.SetActive(false);
        _cellIndicatorRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject _prefab, Vector2Int _size)
    {
        _previewObject = Instantiate(_prefab);
        PreparePreview(_previewObject);
        PrepareCursor(_size);
        _cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int _size)
    {
        if( _size.x > 0 || _size.y > 0)
        {
            _cellIndicator.transform.localScale = new Vector3 (_size.x, 1, _size.y);
            _cellIndicatorRenderer.material.mainTextureScale = _size;
        }
    }

    private void PreparePreview(GameObject _previewObject)
    {
        Renderer [] _renderers = _previewObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in _renderers)
        {
            Material[] materials = renderer.materials;
            for(int i = 0; i < materials.Length; i++)
            {
                materials[i] = _previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        _cellIndicator.SetActive(false);

        if( _previewObject != null ) 
        {
            Destroy(_previewObject);
        } 
    }

    public void UpdatePosition(Vector3 _position, bool _validity)
    {
        if (_previewObject != null) 
        {
            MovePreview(_position);
            ApplyFeedBackToPreview(_validity);
        } 

        MoveCursor(_position);    
        ApplyFeedBackToCursor(_validity);
    }

    private void ApplyFeedBackToPreview(bool _validity)
    {
       Color c = _validity ? Color.white : Color.red;
       
        c.a = 0.8f;
        _previewMaterialInstance.color = c;
    }

    private void ApplyFeedBackToCursor(bool _validity)
    {
        Color c = _validity ? Color.white : Color.red;

        c.a = 0.8f;
        _cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 _position)
    {
        _cellIndicator.transform.position = _position;
    }

    private void MovePreview(Vector3 _position)
    {
       _previewObject.transform.position = new Vector3(_position.x, _position.y + _previewYOffset, _position.z);
    }

    internal void StartShowingRemovePreview()
    {
        _cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedBackToCursor(false);
    }
}
