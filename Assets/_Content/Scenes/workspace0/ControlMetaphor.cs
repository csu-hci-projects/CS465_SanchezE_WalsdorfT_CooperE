using UnityEngine;

public abstract class ControlMetaphor : MonoBehaviour
{
    public bool IsGrabbed => _inputs != null;

    [SerializeField] private Outline _outline;

    protected MotionControl.Inputs _inputs;

    public virtual void OnGrab(MotionControl.Inputs inputs)
    {
        _inputs = inputs;
        if (_outline != null)
        {
            _outline.OutlineColor = Color.cyan;
        }
    }

    public virtual void OnRelease()
    {
        _inputs = null;
        if (_outline != null)
        {
            _outline.OutlineColor = Color.white;
        }
    }

    public virtual void OnHoverStart()
    {
        if (_outline != null)
        {
            _outline.OutlineWidth = 3f;
        }
    }
    public virtual void OnHoverEnd()
    {
        if (_outline != null)
        {
            _outline.OutlineWidth = 1f;
        }
    }
}
