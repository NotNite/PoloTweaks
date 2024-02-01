using UnityEngine;

namespace PoloTweaks.Components;

public class TrailSpeedChecker : MonoBehaviour {
    public float SpeedBarrier = 0f;
    private TrailRenderer trail = null!;
    private Vector3 lastPos;

    private void Awake() {
        trail = this.GetComponent<TrailRenderer>();
    }

    private void Update() {
        var pos = this.transform.position;
        var speed = (pos - this.lastPos).magnitude / Time.deltaTime;
        this.trail.emitting = speed > this.SpeedBarrier;
        this.lastPos = pos;
    }
}
