using System.Collections.Generic;
using UnityEngine;

public class RadarTrigger : MonoBehaviour
{
    public List<Transform> NewTargets = new List<Transform>();

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UntrackedTarget"))
        {
            NewTargets.Add(other.transform);
        }
    }
}
