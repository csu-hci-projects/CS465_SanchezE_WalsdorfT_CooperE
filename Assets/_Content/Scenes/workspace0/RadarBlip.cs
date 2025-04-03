using UnityEngine;

public class RadarBlip : MonoBehaviour
{
    public const float DISPLAY_MAX = 0.5f;
    public const float RADAR_MAX = 3200f;

    public Transform TrackedTransform;

    [SerializeField] private AnimationCurve _blipScaleCurve;

    private SpriteRenderer _spriteRenderer;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Update()
    {
        if (TrackedTransform == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 relativePosition = ShipController.Instance.transform.InverseTransformPoint(TrackedTransform.position);

        float distance = relativePosition.magnitude;
        Vector3 direction = relativePosition.normalized;

        if (distance > RADAR_MAX)
        {
            TrackedTransform.tag = "UntrackedTarget";
            gameObject.SetActive(false);
            return;
        }

        distance = _blipScaleCurve.Evaluate(distance / RADAR_MAX) * DISPLAY_MAX;

        transform.localPosition = direction * distance;

        _lineRenderer.SetPosition(1, new Vector3(0, -transform.localPosition.y, 0));


        float alpha = 1 - (distance / DISPLAY_MAX);
        _spriteRenderer.color = new Color(1, 1, 1, alpha);
        _lineRenderer.endColor = new Color(1, 1, 1, alpha);
        _lineRenderer.startColor = new Color(1, 1, 1, alpha);
    }
}
