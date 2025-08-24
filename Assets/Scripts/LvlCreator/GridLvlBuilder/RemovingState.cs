using System;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int _gameObjectIndex = -1;
    Grid _grid;
    PreviewSystem _previewSystem;
    GridData _figuresData;
    GridData _elementsData;
    ObjectPlacer _objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData figuresData, GridData elementsData, ObjectPlacer objectPlacer)
    {
        _grid = grid;
        _previewSystem = previewSystem;
        _figuresData = figuresData;
        _elementsData = elementsData;
        _objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
       _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int _gridPosition)
    {
        GridData _selectedData = null;

        if (_elementsData.CanPlaceObjectAt(_gridPosition, Vector2Int.one) == false)
        {
            _selectedData = _elementsData;
        }
        else if (_figuresData.CanPlaceObjectAt(_gridPosition,Vector2Int.one) == false)
        {
            _selectedData = _figuresData;
        }

        if (_selectedData == null)
        {
           //sound
        }
        else
        {
            _gameObjectIndex = _selectedData.GetRepresentationIndex(_gridPosition);    
            
            if(_gameObjectIndex == -1)
            {
                Debug.Log("Object not destoryed");
                return;
            }
            else
            {
                _selectedData.RemoveObjectAt(_gridPosition);
                _objectPlacer.RemoveObjectAt(_gameObjectIndex);
                Debug.Log("Object destroyed");
            }
        }
        Vector3 _cellPosition = _grid.CellToWorld(_gridPosition);
        _previewSystem.UpdatePosition(_cellPosition, CheckIfSelectionIsValid(_gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int _gridPosition)
    {
        return !(_elementsData.CanPlaceObjectAt(_gridPosition, Vector2Int.one) && _figuresData.CanPlaceObjectAt(_gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int _gridPosition)
    {
       bool _validity = CheckIfSelectionIsValid(_gridPosition);
       _previewSystem.UpdatePosition(_grid.CellToWorld(_gridPosition), _validity);
    }
}
