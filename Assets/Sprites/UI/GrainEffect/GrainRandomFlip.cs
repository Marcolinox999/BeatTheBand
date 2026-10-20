using UnityEngine;

public class GrainRandomFlip : MonoBehaviour
{
    [SerializeField] private float flipsPerSecond = 12f;

    private float timer;

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer < 1f / flipsPerSecond) return;
        timer = 0f;
        
        float x = Random.value > 0.5f ? 1f : -1f;
        float y = Random.value > 0.5f ? 1f : -1f;
        transform.localScale = new Vector3(x, y, 1f);
    }
}