using UnityEngine;

public abstract class ControlMetaphor : MonoBehaviour
{
    public bool IsGrabbed => _inputs != null;

    protected MotionControl.Inputs _inputs;

    public virtual void OnGrab(MotionControl.Inputs inputs)
    {
        _inputs = inputs;
    }

    public virtual void OnRelease()
    {
        _inputs = null;
    }

    public virtual void OnHoverStart() { }
    public virtual void OnHoverEnd() { }
}
