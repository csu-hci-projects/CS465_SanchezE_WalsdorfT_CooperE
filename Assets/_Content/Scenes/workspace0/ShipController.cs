using UnityEngine;

public class ShipController : MonoBehaviour
{
    // Debug
    public Vector3 Velocity => _rb.linearVelocity;

    // Ship-Specs
    public float Mass = 48000f;
    public float TopSpeed = 220f;
    public float BoostSpeed = 320f;
    public float ReverseSpeedMultiplier = 0.6f;
    public float VerticalSpeedMultiplier = 0.8f;
    public float LateralSpeedMultiplier = 0.8f;
    public float PitchSpeed = 42f;
    public float RollSpeed = 110f;
    public float YawSpeed = 16f;
    public float AngularRecoverySpeed = 120f;
    public float ForwardAcceleration = 45f;
    public float ReverseAcceleration = 30f;
    public float VerticalAcceleration = 30f;
    public float LateralAcceleration = 30f;
    public float BoostDuration = 2f;
    public float BoostAccelerationMultiplier = 4f;
    public float BoostRechargeDelay = 4f;
    public AnimationCurve HandlingCurve;

    // References
    private Rigidbody _rb;
    private ShipInputHandler _inputHandler;

    public bool FlightAssist = true;

    private float _lastBoostTime;
    private bool _isBoosting => Time.time - _lastBoostTime < BoostDuration;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<ShipInputHandler>();

        _rb.mass = Mass;
    }

    private void FixedUpdate()
    {
        if (_rb == null) return;
        if (_inputHandler == null) return;

        if (FlightAssist)
        {
            FlightAssistUpdate();
        }
        else
        {
            FreeFlightUpdate();
        }
    }

    private void FlightAssistUpdate()
    {
        // Linear Movement
        Vector3 thrustInput = new Vector3(_inputHandler.HorizontalThrust, _inputHandler.VerticalThrust, _inputHandler.Throttle);
        thrustInput = Vector3.ClampMagnitude(thrustInput, 1f);

        Vector3 currentLocalVelocity = transform.InverseTransformDirection(_rb.linearVelocity);
        Vector3 targetLocalVelocity = new Vector3(
            thrustInput.x * TopSpeed * LateralSpeedMultiplier,
            thrustInput.y * TopSpeed * VerticalSpeedMultiplier,
            thrustInput.z > 0 ? thrustInput.z * TopSpeed : thrustInput.z * TopSpeed * ReverseSpeedMultiplier
         );

        Vector3 localAcceleration = (targetLocalVelocity - currentLocalVelocity).normalized;
        localAcceleration.Scale(new Vector3(LateralAcceleration, VerticalAcceleration, localAcceleration.z > 0 ? ForwardAcceleration : ReverseAcceleration));

        Vector3 worldAcceleration = transform.TransformDirection(localAcceleration);

        _rb.AddForce(worldAcceleration, ForceMode.Acceleration);

        // Angular Movement
        Vector3 torqueInput = new Vector3(_inputHandler.Pitch, _inputHandler.Yaw, _inputHandler.Roll);

        Vector3 targetLocalAngularVelocity = new Vector3(
            torqueInput.x * PitchSpeed,
            torqueInput.y * YawSpeed,
            torqueInput.z * RollSpeed
        );

        if (thrustInput.z > 0)
        {
            targetLocalAngularVelocity *= HandlingCurve.Evaluate(thrustInput.z);
        }

        Vector3 currentLocalAngularVelocity = transform.InverseTransformDirection(_rb.angularVelocity);

        currentLocalAngularVelocity = Vector3.MoveTowards(currentLocalAngularVelocity, targetLocalAngularVelocity * Mathf.Deg2Rad, AngularRecoverySpeed * Mathf.Deg2Rad * Time.fixedDeltaTime);

        _rb.angularVelocity = transform.TransformDirection(currentLocalAngularVelocity);
    }

    private void FreeFlightUpdate()
    {

    }
}
