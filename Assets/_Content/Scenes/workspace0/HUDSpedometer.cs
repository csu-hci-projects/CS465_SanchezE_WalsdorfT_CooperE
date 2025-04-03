using TMPro;
using UnityEngine;

public class HUDSpedometer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _SpeedGuage;
    [SerializeField] private Transform _ThrottleNeedle;
    [SerializeField] private ShipController _shipController;
    [SerializeField] private float _needleMaxAngle = 45f;
    [SerializeField] private float _needleDistance = 0.225f;
    [SerializeField] private TextMeshPro _speedText;

    private Material _speedGuageMaterial;
    private Color _needleStartingColor;
    private bool _reverse;

    private void Awake()
    {
        _speedGuageMaterial = _SpeedGuage.material;
        _needleStartingColor = _ThrottleNeedle.GetComponent<SpriteRenderer>().color;
    }

    private void FixedUpdate()
    {
        UpdateSpeedGuage();
        UpdateThrottleNeedle();
    }

    private void UpdateSpeedGuage()
    {
        float speed = _shipController.Rigidbody.linearVelocity.magnitude;
        _reverse = Vector3.Dot(_shipController.Rigidbody.linearVelocity, _shipController.transform.forward) < -1;
        float speed01 = Mathf.InverseLerp(0, _shipController.TopSpeed, speed);

        _speedGuageMaterial.SetFloat("_Value", speed01);
        _speedGuageMaterial.SetFloat("_Flip", _reverse ? 1 : 0);

        _speedText.text = $"{Mathf.Round(speed):0} m/s";
    }

    private void UpdateThrottleNeedle()
    {
        float throttle = _shipController.InputHandler.Throttle;
        if (throttle < 0)
            throttle *= _shipController.ReverseSpeedMultiplier;
        if (_reverse)
            throttle++;
        throttle = Mathf.Clamp01(throttle);

        float angle = Mathf.Lerp(-_needleMaxAngle, _needleMaxAngle, throttle);

        _ThrottleNeedle.localPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * _needleDistance, Mathf.Sin(angle * Mathf.Deg2Rad) * _needleDistance, 0);
    }
}
