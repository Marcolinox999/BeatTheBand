using UnityEngine;
using UnityEngine.UI;

public class FilmGrainOverlay : MonoBehaviour
{
    [SerializeField] private RawImage targetImage;
    [SerializeField] private Texture2D[] grainFrames;
    [SerializeField] private float framesPerSecond = 12f;
    [SerializeField, Range(0f, 1f)] private float opacity = 0.12f;
    [SerializeField] private float flickerAmount = 0.03f;

    private float timer;
    private int lastIndex = -1;

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        float interval = 1f / framesPerSecond;

        if (timer < interval) return;
        timer -= interval;

        NextFrame();
    }

    private void NextFrame()
    {
        int index;
        do
        {
            index = Random.Range(0, grainFrames.Length);
        } while (index == lastIndex && grainFrames.Length > 1);
        lastIndex = index;

        targetImage.texture = grainFrames[index];
        
        float flipX = Random.value > 0.5f ? 1f : -1f;
        float flipY = Random.value > 0.5f ? 1f : -1f;
        targetImage.uvRect = new Rect(Random.value, Random.value, flipX, flipY);
        
        float alpha = opacity + Random.Range(-flickerAmount, flickerAmount);
        Color color = targetImage.color;
        color.a = Mathf.Clamp01(alpha);
        targetImage.color = color;
    }
}