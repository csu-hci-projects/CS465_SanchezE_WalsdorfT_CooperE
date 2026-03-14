using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class HintedObject : MonoBehaviour
{
    public Image InfoIcon;
    public Image GlobalInfoIcon;
    public Transform Info;
    public Outline Outline;

    private bool _lookingAt;
    private Coroutine _popupRoutine;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();

        InfoIcon.gameObject.SetActive(false);
        GlobalInfoIcon.gameObject.SetActive(false);
        Info.gameObject.SetActive(false);
    }

    private void Start()
    {
        HintsSystem.Instance.HintedObjects.Add(this);
    }

    //private void Awake()
    //{
    //    if (!GetComponent<RotationConstraint>())
    //    {
    //        RotationConstraint constraint = gameObject.AddComponent<RotationConstraint>();
    //        constraint.AddSource(new ConstraintSource
    //        {
    //            sourceTransform = Camera.main.transform,
    //            weight = 1f
    //        });
    //        constraint.constraintActive = true;
    //        constraint.rotationOffset = new Vector3(0, 180, 0);
    //    }
    //}

    private IEnumerator PopupRoutine()
    {
        InfoIcon.gameObject.SetActive(true);
        InfoIcon.fillAmount = 0f;

        while (InfoIcon.fillAmount < 1f)
        {
            InfoIcon.fillAmount += Time.deltaTime;
            yield return null;
        }

        InfoIcon.fillAmount = 1f;
        InfoIcon.gameObject.SetActive(false);
        Info.gameObject.SetActive(true);
    }

    public void StartLookingAt(bool delay = false)
    {
        if (delay)
        {
            _popupRoutine = StartCoroutine(PopupRoutine());
        }
        else
        {
            InfoIcon.fillAmount = 1f;
            InfoIcon.gameObject.SetActive(false);
            Info.gameObject.SetActive(true);
        }
    }

    public void StopLookingAt()
    {
        if (_popupRoutine != null)
        {
            StopCoroutine(_popupRoutine);
            _popupRoutine = null;
        }
        InfoIcon.gameObject.SetActive(false);
        Info.gameObject.SetActive(false);
    }
}
