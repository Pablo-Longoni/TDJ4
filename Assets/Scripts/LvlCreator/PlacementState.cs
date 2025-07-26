using UnityEngine;

public class PlacementState : IBuildingState
{
    private int _selectedObjectIndex = -1;
    int ID;
    Grid _grid;
    PreviewSystem _previewSystem;
    ObjectsDataBase _dataBase;
    GridData _figuresData;
    GridData _elementsData;
    ObjectPlacer _objectPlacer;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ObjectsDataBase dataBase, GridData figuresData, GridData elementsData, ObjectPlacer objectPlacer)
    {
        ID = iD;
        _grid = grid;
        _previewSystem = previewSystem;
        _dataBase = dataBase;
        _figuresData = figuresData;
        _elementsData = elementsData;
        _objectPlacer = objectPlacer;

        _selectedObjectIndex = _dataBase.objectData.FindIndex(data => data.ID == ID);
        if (_selectedObjectIndex > -1)
        {
            _previewSystem.StartShowingPlacementPreview(_dataBase.objectData[_selectedObjectIndex].Prefab, _dataBase.objectData[_selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with id{iD}");
        }
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int _gridPosition)
    {
        bool _placementValidity = CheckPlacementValidity(_gridPosition, _selectedObjectIndex);
        if (_placementValidity == false)
        {
            return;
        }

        int index = _objectPlacer.PlaceObject(_dataBase.objectData[_selectedObjectIndex].Prefab, _grid.CellToWorld(_gridPosition));


        GridData selectedData = _dataBase.objectData[_selectedObjectIndex].ID == 0 ? _figuresData : _elementsData;

        selectedData.AddObjectAt(_gridPosition, _dataBase.objectData[_selectedObjectIndex].Size,
            _dataBase.objectData[_selectedObjectIndex].ID, index);
        _previewSystem.UpdatePosition(_grid.CellToWorld(_gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = _dataBase.objectData[selectedObjectIndex].ID == 0 ? _figuresData : _elementsData;

        return selectedData.CanPlaceObjectAt(gridPosition, _dataBase.objectData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int _gridPosition)
    {
        bool _placementValidity = CheckPlacementValidity(_gridPosition, _selectedObjectIndex);

        _previewSystem.UpdatePosition(_grid.CellToWorld(_gridPosition), _placementValidity);
    }
}
