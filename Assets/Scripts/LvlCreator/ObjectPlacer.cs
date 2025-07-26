using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _placedGameObjects = new();

    public int PlaceObject(GameObject _prefab, Vector3 _position)
    {
        GameObject _newObject = Instantiate(_prefab);
        _newObject.transform.position = _position;
        _placedGameObjects.Add(_newObject);
        return _placedGameObjects.Count -1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
       if (_placedGameObjects.Count <= gameObjectIndex || _placedGameObjects[gameObjectIndex] == null )
        {
            return;
        }
        else
        {
            Destroy(_placedGameObjects[gameObjectIndex]);
            _placedGameObjects[gameObjectIndex] = null;
        }
    }
}
