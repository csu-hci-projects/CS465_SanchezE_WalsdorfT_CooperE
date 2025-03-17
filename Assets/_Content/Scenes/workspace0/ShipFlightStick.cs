using UnityEngine;

public class ShipFlightStick : ControlMetaphor
{
    [Header("References")]
    [SerializeField] private ShipInputHandler _shipInputHandler;
    [Header("Physical Configuration")]
    [SerializeField] private float _Xlimit = 30f;
    [SerializeField] private float _Ylimit = 20f;
    [SerializeField] private float _Zlimit = 20f;
    [Header("Gizmos Configuration")]
    [SerializeField] private float _gizmoSize = 0.15f;
    [Header("Flight Stick Configuration")]
    [SerializeField] private float _LerpSpeed = 10f;

    private Vector3 _targetRotation;

    private void Awake()
    {
        _targetRotation = Vector3.zero;
        transform.localEulerAngles = _targetRotation;
    }

    private void Update()
    {
        if (IsGrabbed)
        {
            Quaternion localInputRotation = Quaternion.Inverse(transform.parent.rotation) * _inputs.Transform.rotation;

            Vector3 localInputEulerAngles = localInputRotation.eulerAngles;
            localInputEulerAngles.x = Mathf.Clamp(NormalizeAngle(localInputEulerAngles.x), -_Xlimit, _Xlimit);
            localInputEulerAngles.y = Mathf.Clamp(NormalizeAngle(localInputEulerAngles.y), -_Ylimit, _Ylimit);
            localInputEulerAngles.z = Mathf.Clamp(NormalizeAngle(localInputEulerAngles.z), -_Zlimit, _Zlimit);

            _targetRotation = localInputEulerAngles;
        }
        else
        {
            _targetRotation = Vector3.zero;
        }

        Vector3 normalizedLocalAngles = transform.localEulerAngles;
        normalizedLocalAngles.x = NormalizeAngle(normalizedLocalAngles.x);
        normalizedLocalAngles.y = NormalizeAngle(normalizedLocalAngles.y);
        normalizedLocalAngles.z = NormalizeAngle(normalizedLocalAngles.z);
        transform.localEulerAngles = Vector3.Lerp(normalizedLocalAngles, _targetRotation, Mathf.Clamp01(Time.deltaTime * _LerpSpeed));

        if (_shipInputHandler)
        {
            _shipInputHandler.Pitch = Mathf.InverseLerp(-_Xlimit, _Xlimit, normalizedLocalAngles.x) * 2 - 1;
            _shipInputHandler.Yaw = Mathf.InverseLerp(-_Ylimit, _Ylimit, normalizedLocalAngles.y) * 2 - 1;
            _shipInputHandler.Roll = Mathf.InverseLerp(-_Zlimit, _Zlimit, normalizedLocalAngles.z) * 2 - 1;
        }
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180)
        {
            angle -= 360;
        }
        while (angle < -180)
        {
            angle += 360;
        }
        return angle;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.up * _gizmoSize);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(transform.localEulerAngles.y, Vector3.up) * transform.parent.forward * _gizmoSize);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-_Xlimit, Vector3.right) * transform.parent.up * _gizmoSize);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(_Xlimit, Vector3.right) * transform.parent.up * _gizmoSize);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-_Ylimit, Vector3.up) * transform.parent.forward * _gizmoSize);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(_Ylimit, Vector3.up) * transform.parent.forward * _gizmoSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-_Zlimit, Vector3.forward) * transform.parent.up * _gizmoSize);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(_Zlimit, Vector3.forward) * transform.parent.up * _gizmoSize);
    }
}
