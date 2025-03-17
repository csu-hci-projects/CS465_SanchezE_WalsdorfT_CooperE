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
    public float MinHandlingMultiplier = 0.5f;
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

    }

    private void FreeFlightUpdate()
    {

    }
}
