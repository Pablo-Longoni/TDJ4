using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UIElements;
public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupay = CalculatePosistions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupay, ID, placedObjectIndex);
        foreach (var pos in positionToOccupay)
        {
            if(placedObjects.ContainsKey(pos))
            
                throw new Exception($"El diccionario ya contiene la posicion de esta celda{pos} ");
                placedObjects[pos] = data;
            
        }
    }

    private List<Vector3Int> CalculatePosistions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePosistions(gridPosition, objectSize);
        foreach ( var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
       if(placedObjects.ContainsKey(gridPosition) == false)
        {
            return -1;
        }
        else
        {
            return placedObjects[gridPosition].PlacedObjectIndex;
        }
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
       foreach ( var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;

    public int ID { get; private set; }

    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
