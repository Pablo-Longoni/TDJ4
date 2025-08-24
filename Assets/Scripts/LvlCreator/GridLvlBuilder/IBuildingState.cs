using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int _gridPosition);
    void UpdateState(Vector3Int _gridPosition);
}