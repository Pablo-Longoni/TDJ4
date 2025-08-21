using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class InteractiveShadows : MonoBehaviour
{
    [SerializeField] private Transform _shadowTransform;
    [SerializeField] private Transform _lightTransform;
    private LightType _lightType;

    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private Vector3 _extrusionDirection = Vector3.zero;

    private Vector3[] _objectVertices;

    private Mesh _shadowColliderMesh;
    private MeshCollider _shadowCollider;

    private Vector3 _previousPosition;
    private Quaternion _previousRotation;
    private Vector3 _previousScale;

    private bool _canUpdateCollider = true;

    [SerializeField][Range(0.02f, 1f)] private float _shadowColliderUpdateTime = 0.08f;
    private void Awake()
    {
        InitializeShadowCollider();

        _lightType = _lightTransform.GetComponent<Light>().type;

        _objectVertices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();

        _shadowColliderMesh = new Mesh();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _shadowTransform.position = transform.position;
    }

    private void FixedUpdate()
    {
        if (TransformHasChanged() && _canUpdateCollider)
        {
           Invoke("UpdateShadowCollider", _shadowColliderUpdateTime);
           _canUpdateCollider = false; 
        }

        _previousPosition = transform.position;
        _previousRotation = transform.rotation;
        _previousScale = transform.localScale;
    }

    private void InitializeShadowCollider()
    {
        GameObject _shadowGameObject = _shadowTransform.gameObject;
     //   _shadowGameObject.hideFlags = HideFlags.HideInHierarchy;
        _shadowCollider = _shadowGameObject.AddComponent<MeshCollider>();
        _shadowCollider.convex = true;
        _shadowCollider.isTrigger = true;
    }

    private Vector3[] ComputeShadowColliderMeshVerticies()
    {
        Vector3[] _points = new Vector3[2 * _objectVertices.Length];

        Vector3 _rayCastDirection = _lightTransform.forward;

        int n = _objectVertices.Length;

        for ( int i = 0; i < n; i++ )
        {
            //original Vector3 _point = _objectVertices[i];
            Vector3 _point = transform.TransformPoint(_objectVertices[i]);

            if (_lightType != LightType.Directional)
            {
                _rayCastDirection = _point - _lightTransform.position;
            }

            _points[i] = ComputeIntersectionPoint(_point, _rayCastDirection);

            _points[n + i] = ComputeExtrusionPoint(_point, _points[i]);
        }
        return _points;
    }

    private Vector3 ComputeIntersectionPoint(Vector3 _fromPosition, Vector3 _direction)
    {
        RaycastHit _hit;

        if(Physics.Raycast(_fromPosition, _direction, out _hit, Mathf.Infinity, _targetLayerMask))
        {
         //   return _hit.point  - transform.position;
            return _shadowTransform.InverseTransformPoint(_hit.point);
        }

      //  return _fromPosition + 100 * _direction - transform.position;
        return _shadowTransform.InverseTransformPoint(_fromPosition + 6 * _direction);
    }

    private Vector3 ComputeExtrusionPoint(Vector3 _objectVertexPosition, Vector3 _shadowPointPosition)
    {
        if(_extrusionDirection.sqrMagnitude == 0)
        {
          //  return _objectVertexPosition - transform.position;
            return _shadowTransform.InverseTransformPoint(_objectVertexPosition);
        }

        return _shadowPointPosition + _extrusionDirection;
    }

    private bool TransformHasChanged()
    {
        return _previousPosition != transform.position || _previousRotation != transform.rotation || _previousScale != transform.localScale;
    }

    private void UpdateShadowCollider()
    {
        _shadowColliderMesh.vertices = ComputeShadowColliderMeshVerticies();
     /*   _shadowColliderMesh.RecalculateBounds();
        _shadowColliderMesh.RecalculateNormals();*/
        _shadowCollider.sharedMesh = _shadowColliderMesh;
        _canUpdateCollider = true;
    }
}

 