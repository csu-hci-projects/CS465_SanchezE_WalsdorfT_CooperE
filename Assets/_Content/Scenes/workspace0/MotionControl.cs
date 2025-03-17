using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class MotionControl : MonoBehaviour
{
    public Hand ControllerHand;

    public HapticImpulsePlayer HapticImpulsePlayer { get; private set; }
    public TrackedPoseDriver TrackedPoseDriver { get; private set; }
    public Transform GripCenter { get; private set; }


    public Inputs InputValues = new Inputs();
    public Collider CurrentHoveredGrabCollider { get; private set; }
    public Collider CurrentGrabbedCollider { get; private set; }

    private List<Collider> _overlappedGrabColliders = new List<Collider>();

    private void Awake()
    {
        HapticImpulsePlayer = GetComponent<HapticImpulsePlayer>();
        TrackedPoseDriver = GetComponent<TrackedPoseDriver>();
        GripCenter = transform.Find("GripCenter");
        InputValues.Transform = transform;
        InputValues.GripCenterTransform = GripCenter;
    }

    private void Update()
    {
        HandleHover();
        HandleGrabbing();
    }

    private void LateUpdate()
    {
        InputValues.ClearFrameFlags();
    }

    private void OnTriggerEnter(Collider other)
    {
        _overlappedGrabColliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _overlappedGrabColliders.Remove(other);
    }

    private void HandleHover()
    {
        if (CurrentGrabbedCollider != null)
        {
            return;
        }

        Collider closestCollider = null;
        float closestDistance = float.MaxValue;
        foreach (var collider in _overlappedGrabColliders)
        {
            float distance = Vector3.Distance(collider.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = collider;
            }
        }

        if (closestCollider != null)
        {
            if (CurrentHoveredGrabCollider != closestCollider)
            {
                if (CurrentHoveredGrabCollider != null)
                {
                    CurrentHoveredGrabCollider.GetComponent<ControlMetaphor>()?.OnHoverEnd();
                }
                closestCollider.GetComponent<ControlMetaphor>()?.OnHoverStart();
                CurrentHoveredGrabCollider = closestCollider;
            }
        }
        else
        {
            if (CurrentHoveredGrabCollider != null)
            {
                CurrentHoveredGrabCollider.GetComponent<ControlMetaphor>()?.OnHoverEnd();
                CurrentHoveredGrabCollider = null;
            }
        }
    }

    private void HandleGrabbing()
    {
        if (InputValues.GripButtonPressedThisFrame)
        {
            if (CurrentHoveredGrabCollider != null)
            {
                CurrentHoveredGrabCollider.GetComponentInParent<ControlMetaphor>()?.OnGrab(InputValues);
                CurrentGrabbedCollider = CurrentHoveredGrabCollider;
            }
        }

        if (InputValues.GripButtonReleasedThisFrame)
        {
            if (CurrentGrabbedCollider != null)
            {
                CurrentGrabbedCollider.GetComponentInParent<ControlMetaphor>()?.OnRelease();
                CurrentGrabbedCollider = null;
            }
        }
    }

    #region Input Events

    private void OnThumbstick(InputValue value) { InputValues.Thumbstick = value.Get<Vector2>(); }
    private void OnGrip(InputValue value) { InputValues.Grip = value.Get<float>(); }
    private void OnGripButton(InputValue value)
    {
        InputValues.GripButton = value.isPressed;
        if (value.isPressed)
        {
            InputValues.GripButtonPressedThisFrame = true;
        }
        else
        {
            InputValues.GripButtonReleasedThisFrame = true;
        }
    }
    private void OnMenuButton(InputValue value)
    {
        InputValues.MenuButton = value.isPressed;
        if (value.isPressed)
        {
            InputValues.MenuButtonPressedThisFrame = true;
        }
        else
        {
            InputValues.MenuButtonReleasedThisFrame = true;
        }
    }
    private void OnPrimaryButton(InputValue value)
    {
        InputValues.PrimaryButton = value.isPressed;
        if (value.isPressed)
        {
            InputValues.PrimaryButtonPressedThisFrame = true;
        }
        else
        {
            InputValues.PrimaryButtonReleasedThisFrame = true;
        }
    }
    private void OnPrimaryTouch(InputValue value)
    {
        InputValues.PrimaryTouch = value.isPressed;
        if (value.isPressed)
        {
            InputValues.PrimaryTouchPressedThisFrame = true;
        }
        else
        {
            InputValues.PrimaryTouchReleasedThisFrame = true;
        }
    }
    private void OnSecondaryButton(InputValue value)
    {
        InputValues.SecondaryButton = value.isPressed;
        if (value.isPressed)
        {
            InputValues.SecondaryButtonPressedThisFrame = true;
        }
        else
        {
            InputValues.SecondaryButtonReleasedThisFrame = true;
        }
    }
    private void OnSecondaryTouch(InputValue value)
    {
        InputValues.SecondaryTouch = value.isPressed;
        if (value.isPressed)
        {
            InputValues.SecondaryTouchPressedThisFrame = true;
        }
        else
        {
            InputValues.SecondaryTouchReleasedThisFrame = true;
        }
    }
    private void OnTrigger(InputValue value) { InputValues.Trigger = value.Get<float>(); }
    private void OnTriggerButton(InputValue value)
    {
        InputValues.TriggerButton = value.isPressed;
        if (value.isPressed)
        {
            InputValues.TriggerButtonPressedThisFrame = true;
        }
        else
        {
            InputValues.TriggerButtonReleasedThisFrame = true;
        }
    }
    private void OnTriggerTouch(InputValue value)
    {
        InputValues.TriggerTouch = value.isPressed;
        if (value.isPressed)
        {
            InputValues.TriggerTouchPressedThisFrame = true;
        }
        else
        {
            InputValues.TriggerTouchReleasedThisFrame = true;
        }
    }
    private void OnThumbstickClick(InputValue value)
    {
        InputValues.ThumbstickClick = value.isPressed;
        if (value.isPressed)
        {
            InputValues.ThumbstickClickPressedThisFrame = true;
        }
        else
        {
            InputValues.ThumbstickClickReleasedThisFrame = true;
        }
    }
    private void OnThumbstickTouch(InputValue value)
    {
        InputValues.ThumbstickTouch = value.isPressed;
        if (value.isPressed)
        {
            InputValues.ThumbstickTouchPressedThisFrame = true;
        }
        else
        {
            InputValues.ThumbstickTouchReleasedThisFrame = true;
        }
    }
    private void OnThumbrestTouch(InputValue value)
    {
        InputValues.ThumbrestTouch = value.isPressed;
        if (value.isPressed)
        {
            InputValues.ThumbrestTouchPressedThisFrame = true;
        }
        else
        {
            InputValues.ThumbrestTouchReleasedThisFrame = true;
        }
    }
    private void OnDeviceIsTracked(InputValue value) { InputValues.DeviceIsTracked = value.isPressed; }
    private void OnDeviceTrackingState(InputValue value) { InputValues.DeviceTrackingState = value.Get<int>(); }
    private void OnDevicePosition(InputValue value) { InputValues.DevicePosition = value.Get<Vector3>(); }
    private void OnDeviceRotation(InputValue value) { InputValues.DeviceRotation = value.Get<Quaternion>(); }
    private void OnDeviceVelocity(InputValue value) { InputValues.DeviceVelocity = value.Get<Vector3>(); }
    private void OnDeviceAngularVelocity(InputValue value) { InputValues.DeviceAngularVelocity = value.Get<Vector3>(); }
    private void OnPointerIsTracked(InputValue value) { InputValues.PointerIsTracked = value.isPressed; }
    private void OnPointerTrackingState(InputValue value) { InputValues.PointerTrackingState = value.Get<int>(); }
    private void OnPointerPosition(InputValue value) { InputValues.PointerPosition = value.Get<Vector3>(); }
    private void OnPointerRotation(InputValue value) { InputValues.PointerRotation = value.Get<Quaternion>(); }
    private void OnPointerVelocity(InputValue value) { InputValues.PointerVelocity = value.Get<Vector3>(); }
    private void OnPointerAngularVelocity(InputValue value) { InputValues.PointerAngularVelocity = value.Get<Vector3>(); }
    private void OnIsTracked(InputValue value) { InputValues.IsTracked = value.isPressed; }
    private void OnTrackingState(InputValue value) { InputValues.TrackingState = value.Get<int>(); }

    #endregion

    [System.Serializable]
    public class Inputs
    {
        public Transform Transform;
        public Transform GripCenterTransform;
        public Vector2 Thumbstick;
        public float Grip;
        public bool GripButton;
        public bool GripButtonPressedThisFrame;
        public bool GripButtonReleasedThisFrame;
        public bool MenuButton;
        public bool MenuButtonPressedThisFrame;
        public bool MenuButtonReleasedThisFrame;
        public bool PrimaryButton;
        public bool PrimaryButtonPressedThisFrame;
        public bool PrimaryButtonReleasedThisFrame;
        public bool PrimaryTouch;
        public bool PrimaryTouchPressedThisFrame;
        public bool PrimaryTouchReleasedThisFrame;
        public bool SecondaryButton;
        public bool SecondaryButtonPressedThisFrame;
        public bool SecondaryButtonReleasedThisFrame;
        public bool SecondaryTouch;
        public bool SecondaryTouchPressedThisFrame;
        public bool SecondaryTouchReleasedThisFrame;
        public float Trigger;
        public bool TriggerButton;
        public bool TriggerButtonPressedThisFrame;
        public bool TriggerButtonReleasedThisFrame;
        public bool TriggerTouch;
        public bool TriggerTouchPressedThisFrame;
        public bool TriggerTouchReleasedThisFrame;
        public bool ThumbstickClick;
        public bool ThumbstickClickPressedThisFrame;
        public bool ThumbstickClickReleasedThisFrame;
        public bool ThumbstickTouch;
        public bool ThumbstickTouchPressedThisFrame;
        public bool ThumbstickTouchReleasedThisFrame;
        public bool ThumbrestTouch;
        public bool ThumbrestTouchPressedThisFrame;
        public bool ThumbrestTouchReleasedThisFrame;
        public bool DeviceIsTracked;
        public int DeviceTrackingState;
        public Vector3 DevicePosition;
        public Quaternion DeviceRotation;
        public Vector3 DeviceVelocity;
        public Vector3 DeviceAngularVelocity;
        public bool PointerIsTracked;
        public int PointerTrackingState;
        public Vector3 PointerPosition;
        public Quaternion PointerRotation;
        public Vector3 PointerVelocity;
        public Vector3 PointerAngularVelocity;
        public bool IsTracked;
        public int TrackingState;

        public void ClearFrameFlags()
        {
            GripButtonPressedThisFrame = false;
            GripButtonReleasedThisFrame = false;
            MenuButtonPressedThisFrame = false;
            MenuButtonReleasedThisFrame = false;
            PrimaryButtonPressedThisFrame = false;
            PrimaryButtonReleasedThisFrame = false;
            PrimaryTouchPressedThisFrame = false;
            PrimaryTouchReleasedThisFrame = false;
            SecondaryButtonPressedThisFrame = false;
            SecondaryButtonReleasedThisFrame = false;
            SecondaryTouchPressedThisFrame = false;
            SecondaryTouchReleasedThisFrame = false;
            TriggerButtonPressedThisFrame = false;
            TriggerButtonReleasedThisFrame = false;
            TriggerTouchPressedThisFrame = false;
            TriggerTouchReleasedThisFrame = false;
            ThumbstickClickPressedThisFrame = false;
            ThumbstickClickReleasedThisFrame = false;
            ThumbstickTouchPressedThisFrame = false;
            ThumbstickTouchReleasedThisFrame = false;
            ThumbrestTouchPressedThisFrame = false;
            ThumbrestTouchReleasedThisFrame = false;
        }
    }

    public enum Hand
    {
        Left,
        Right
    }
}
