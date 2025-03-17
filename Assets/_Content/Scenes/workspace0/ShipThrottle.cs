using UnityEngine;

public class ShipThrottle : ControlMetaphor
{
    [Header("References")]
    [SerializeField] private ShipInputHandler _shipInputHandler;
    [Header("Physical Configuration")]
    [SerializeField] private float _minZ = -0.2f;
    [SerializeField] private float _maxZ = 0.2f;
    [Header("Gizmos Configuration")]
    [SerializeField] private float _gizmoSize = 0.02f;
    [SerializeField] private Vector3 _gizmoOffset = Vector3.zero;
    [Header("Throttle Configuration")]
    [SerializeField] private float _zeroPosition = 0.1f;
    [SerializeField] private float _zeroSnapWidth = 0.01f;
    [SerializeField] private float _LerpSpeed = 10f;

    private Vector3 _targetPosition;

    private void Awake()
    {
        _targetPosition = new Vector3(0, 0, _zeroPosition);
        transform.localPosition = _targetPosition;
    }

    private void Update()
    {
        if (IsGrabbed)
        {
            _targetPosition = transform.parent.InverseTransformPoint(_inputs.GripCenterTransform.position);
            _targetPosition = new Vector3(0, 0, Mathf.Clamp(_targetPosition.z, _minZ, _maxZ));

            if (Mathf.Abs(_targetPosition.z - _zeroPosition) < _zeroSnapWidth)
            {
                _targetPosition = new Vector3(0, 0, _zeroPosition);
            }
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Mathf.Clamp01(Time.deltaTime * _LerpSpeed));

        if (_shipInputHandler)
        {
            float outputValue = 0f;
            if (transform.localPosition.z > _zeroPosition)
            {
                outputValue = Mathf.InverseLerp(_zeroPosition, _maxZ, transform.localPosition.z);
            }
            else
            {
                outputValue = -Mathf.InverseLerp(_zeroPosition, _minZ, transform.localPosition.z);
            }

            _shipInputHandler.Throttle = outputValue;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.parent.TransformPoint(new Vector3(0, 0, _maxZ) + _gizmoOffset), new Vector3(_gizmoSize, _gizmoSize, _gizmoSize));
        Gizmos.DrawLine(transform.parent.TransformPoint(new Vector3(0, 0, _minZ) + _gizmoOffset), transform.parent.TransformPoint(new Vector3(0, 0, _maxZ) + _gizmoOffset));
        Gizmos.DrawWireCube(transform.parent.TransformPoint(new Vector3(0, 0, _minZ) + _gizmoOffset), new Vector3(_gizmoSize, _gizmoSize, _gizmoSize));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.parent.TransformPoint(new Vector3(0, 0, _zeroPosition) + _gizmoOffset), new Vector3(_gizmoSize, _gizmoSize, _zeroSnapWidth));
    }

    private void OnValidate()
    {
        transform.localPosition = new Vector3(0, 0, _zeroPosition);
    }
}
