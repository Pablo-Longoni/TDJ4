using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
  //  [SerializeField] private GameObject _mouseIndicator;
  //  [SerializeField] private GameObject _cellIndicator;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Grid _grid;

    [SerializeField] ObjectsDataBase _dataBase;
   // private int selectedObjectIndex = -1;

    [SerializeField] private GameObject _gridVisualization;

    private GridData _figuresData, _elementsData;

    //  private Renderer _previewRenderer;

    [SerializeField] private ObjectPlacer _objectPlacer;

    [SerializeField] private PreviewSystem _previewSystem;
    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    IBuildingState _buildingState;
    private void Start()
    {
        StopPlacement();
        _figuresData = new ();
        _elementsData = new();
   //     _previewRenderer = _cellIndicator.GetComponentInChildren<Renderer> ();
    }

    public void StopPlacement()
    {
       // selectedObjectIndex = -1;
       if(_buildingState == null)
        {
            return;
        }

       _gridVisualization.SetActive(false);
        _buildingState.EndState();
        _inputManager.OnClicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
        _lastDetectedPosition = Vector3Int.zero;
        _buildingState = null;
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        _gridVisualization.SetActive(true);
       _buildingState = new PlacementState(ID, _grid, _previewSystem, _dataBase, _figuresData, _elementsData, _objectPlacer);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        _gridVisualization.SetActive(true);
        _buildingState = new RemovingState(_grid, _previewSystem, _elementsData, _figuresData, _objectPlacer);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
       if(_inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 _mousePostion = _inputManager.GetSelectedMapPosition();
        Vector3Int _gridPosition = _grid.WorldToCell(_mousePostion);

       _buildingState.OnAction(_gridPosition);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = _dataBase.objectData[selectedObjectIndex].ID == 0 ? _figuresData : _elementsData; 

    //    return selectedData.CanPlaceObjectAt(gridPosition, _dataBase.objectData[selectedObjectIndex].Size);
    //}

    void Update()
    {
        if(_buildingState == null)
            return;
        
        Vector3 _mousePostion = _inputManager.GetSelectedMapPosition();
        Vector3Int _gridPosition = _grid.WorldToCell(_mousePostion);

        if (_lastDetectedPosition != _gridPosition)
        {
            _buildingState.UpdateState(_gridPosition);
            _lastDetectedPosition = _gridPosition;
        }
      
    }
}
