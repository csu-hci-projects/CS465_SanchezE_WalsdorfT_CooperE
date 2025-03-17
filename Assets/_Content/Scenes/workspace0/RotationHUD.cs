using UnityEngine;

public class RotationHUD : MonoBehaviour
{
    [SerializeField] private ShipController _shipController;
    [SerializeField] private ShipInputHandler _shipInputHandler;
    [SerializeField] private Transform _hologram;

    [SerializeField] private float _lerpSpeed = 10f;
    [SerializeField] private float _angleScale = 0.2f;

    private Vector3 _rotation;
    private Vector3 _targetRotation;

    private void Update()
    {
        _targetRotation = new Vector3(_shipInputHandler.Pitch * _shipController.PitchSpeed,
            _shipInputHandler.Yaw * _shipController.YawSpeed,
            _shipInputHandler.Roll * _shipController.RollSpeed);
        _targetRotation *= _angleScale;

        _rotation = Vector3.Lerp(_rotation, _targetRotation, Time.deltaTime * _lerpSpeed);

        _hologram.localRotation = Quaternion.Euler(_rotation);
    }
}
