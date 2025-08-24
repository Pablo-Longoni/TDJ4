using System.Collections;
using UnityEngine;

public class MovingObstacles : MonoBehaviour
{
    [SerializeField] private Transform _obstacle;
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private float _speed;

    private Vector3 _startPos;
    private Vector3 _targetPos;

    void Start()
    {
        _startPos = _obstacle.position;
        _targetPos = _startPos + _offSet;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * _speed, 1f);
        _obstacle.position = Vector3.Lerp(_startPos, _targetPos, t);
    }
}

