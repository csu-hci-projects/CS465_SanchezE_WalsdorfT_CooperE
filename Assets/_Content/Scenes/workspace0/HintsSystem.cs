using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HintsSystem : MonoBehaviour
{
    public static HintsSystem Instance;

    public LayerMask HintsLayer;

    public HintsMode CurrentHintsMode = HintsMode.GazeOnly;

    public List<HintedObject> HintedObjects = new List<HintedObject>();

    [SerializeField] private TMPro.TextMeshPro _hintsText;
    [SerializeField] private GameObject _hintsButtonUI;

    private bool _hintsButtonPressed = false;
    private HintedObject _currentHintedObject;

    private void Awake()
    {
        Instance = this;
        _hintsText.text = CurrentHintsMode.ToString();
        if (CurrentHintsMode == HintsMode.GazeOnly)
        {
            _hintsButtonUI.SetActive(false);
        }
        else
        {
            _hintsButtonUI.SetActive(true);
        }
    }

    private void Update()
    {
        if (CurrentHintsMode == HintsMode.ButtonOnly)
        {
            return;
        }
        if (CurrentHintsMode == HintsMode.GazeAndButton && !_hintsButtonPressed)
        {
            _currentHintedObject?.StopLookingAt();
            _currentHintedObject = null;
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, HintsLayer))
        {
            if (hit.collider != null)
            {
                HintedObject hintedObject = hit.collider.GetComponent<HintedObject>();
                if (hintedObject != null && hintedObject != _currentHintedObject)
                {
                    if (_currentHintedObject != null)
                    {
                        _currentHintedObject.StopLookingAt();
                    }
                    _currentHintedObject = hintedObject;
                    _currentHintedObject.StartLookingAt(CurrentHintsMode == HintsMode.GazeOnly);
                }
            }
        }
        else
        {
            if (_currentHintedObject != null)
            {
                _currentHintedObject.StopLookingAt();
                _currentHintedObject = null;
            }
        }
    }

    public void OnSecondaryButton(InputValue value)
    {
        _hintsButtonPressed = value.isPressed;

        if (CurrentHintsMode == HintsMode.ButtonOnly)
        {
            foreach (HintedObject obj in HintedObjects)
            {
                if (value.isPressed)
                {
                    obj.StartLookingAt(false);
                }
                else
                {
                    obj.StopLookingAt();
                }
            }
        }
        else if (CurrentHintsMode == HintsMode.GazeAndButton)
        {
            foreach(HintedObject obj in HintedObjects)
            {
                obj.GlobalInfoIcon.gameObject.SetActive(value.isPressed);
            }
        }
    }

    public void OnMenuButton(InputValue value)
    {
        _currentHintedObject?.StopLookingAt();
        _currentHintedObject = null;

        if (value.isPressed)
        {
            if (CurrentHintsMode == HintsMode.GazeOnly)
            {
                CurrentHintsMode = HintsMode.ButtonOnly;
                _hintsButtonUI.SetActive(true);
            }
            else if (CurrentHintsMode == HintsMode.ButtonOnly)
            {
                CurrentHintsMode = HintsMode.GazeAndButton;
                _hintsButtonUI.SetActive(true);
            }
            else
            {
                CurrentHintsMode = HintsMode.GazeOnly;
                _hintsButtonUI.SetActive(false);
            }

            _hintsText.text = CurrentHintsMode.ToString();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10f);
    }

    public enum HintsMode
    {
        None,
        GazeOnly,
        ButtonOnly,
        GazeAndButton
    }
}
