using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static ShipController Instance;

    // Debug
    public Vector3 Velocity => Rigidbody.linearVelocity;

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
    public float AngularRecoverySpeed = 40f;
    public float ForwardAcceleration = 45f;
    public float ReverseAcceleration = 30f;
    public float VerticalAcceleration = 30f;
    public float LateralAcceleration = 30f;
    public float BoostDuration = 2f;
    public float BoostAccelerationMultiplier = 4f;
    public float BoostRechargeDelay = 4f;
    public AnimationCurve HandlingCurve;

    // References
    public Rigidbody Rigidbody;
    public ShipInputHandler InputHandler;

    public bool FlightAssist = true;

    private float _lastBoostTime;
    private bool _isBoosting => Time.time - _lastBoostTime < BoostDuration;

    private void Awake()
    {
        Instance = this;

        Rigidbody = GetComponent<Rigidbody>();
        InputHandler = GetComponent<ShipInputHandler>();

        Rigidbody.mass = Mass;
    }

    private void FixedUpdate()
    {
        if (Rigidbody == null) return;
        if (InputHandler == null) return;

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
        Vector3 thrustInput = new Vector3(InputHandler.HorizontalThrust, InputHandler.VerticalThrust, InputHandler.Throttle);
        thrustInput = Vector3.ClampMagnitude(thrustInput, 1f);

        Vector3 currentLocalVelocity = transform.InverseTransformDirection(Rigidbody.linearVelocity);
        Vector3 targetLocalVelocity = new Vector3(
            thrustInput.x * TopSpeed * LateralSpeedMultiplier,
            thrustInput.y * TopSpeed * VerticalSpeedMultiplier,
            thrustInput.z > 0 ? thrustInput.z * TopSpeed : thrustInput.z * TopSpeed * ReverseSpeedMultiplier
         );

        Vector3 localAcceleration = (targetLocalVelocity - currentLocalVelocity).normalized;
        localAcceleration.Scale(new Vector3(LateralAcceleration, VerticalAcceleration, localAcceleration.z > 0 ? ForwardAcceleration : ReverseAcceleration));

        // Triple desceleration
        if (Mathf.Sign(localAcceleration.x) != Mathf.Sign(currentLocalVelocity.x))
            localAcceleration.x *= 3f;
        if (Mathf.Sign(localAcceleration.y) != Mathf.Sign(currentLocalVelocity.y))
            localAcceleration.y *= 3f;
        if (Mathf.Sign(localAcceleration.z) != Mathf.Sign(currentLocalVelocity.z))
            localAcceleration.z *= 3f;


        Vector3 worldAcceleration = transform.TransformDirection(localAcceleration);

        Rigidbody.AddForce(worldAcceleration, ForceMode.Acceleration);

        // Angular Movement
        Vector3 torqueInput = new Vector3(InputHandler.Pitch, InputHandler.Yaw, InputHandler.Roll);

        Vector3 targetLocalAngularVelocity = new Vector3(
            torqueInput.x * PitchSpeed,
            torqueInput.y * YawSpeed,
            torqueInput.z * RollSpeed
        );

        if (thrustInput.z > 0)
        {
            targetLocalAngularVelocity *= HandlingCurve.Evaluate(thrustInput.z);
        }

        Vector3 currentLocalAngularVelocity = transform.InverseTransformDirection(Rigidbody.angularVelocity);

        currentLocalAngularVelocity = Vector3.MoveTowards(currentLocalAngularVelocity, targetLocalAngularVelocity * Mathf.Deg2Rad, AngularRecoverySpeed * Mathf.Deg2Rad * Time.fixedDeltaTime);

        Rigidbody.angularVelocity = transform.TransformDirection(currentLocalAngularVelocity);
    }

    private void FreeFlightUpdate()
    {

    }
}
