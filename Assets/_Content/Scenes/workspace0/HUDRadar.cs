using UnityEngine;

public class HUDRadar : MonoBehaviour
{
    [SerializeField] private RadarTrigger _radarTrigger;
    private ObjectPool _blipPool;

    private void Awake()
    {
        _blipPool = GetComponent<ObjectPool>();
    }

    private void FixedUpdate()
    {
        foreach (Transform target in _radarTrigger.NewTargets)
        {
            RadarBlip blip = _blipPool.GetObject().GetComponent<RadarBlip>();
            blip.TrackedTransform = target;
            target.tag = "TrackedTarget";
            RadarTrackable info = target.GetComponent<RadarTrackable>();
            if (info != null)
            {
                blip.Set(info.Sprite, info.Color);
            }
            else
            {
                blip.Set(null, Color.white);
            }
        }
        _radarTrigger.NewTargets.Clear();
    }
}
