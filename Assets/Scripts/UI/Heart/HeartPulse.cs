using UnityEngine;

public class HeartPulse : MonoBehaviour
{
    [SerializeField] private float pulseScale = 1.3f;
    [SerializeField, Min(1f)] private float decay = 4f; 

    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    private void Update()
    {
        double position = BeatManager.instance.BeatPosition;
        if (position < 0) return;
        
        float fraction = (float)(position - System.Math.Floor(position));
        float t = Mathf.Clamp01(fraction * decay);

        transform.localScale = Vector3.Lerp(baseScale * pulseScale, baseScale, t);
    }
}