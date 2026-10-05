using UnityEngine;
using UnityEngine.UI;

public class BeatMarker : MonoBehaviour
{
    private RectTransform rect;
    private Image image;
    private double targetBeat;
    private int beatsAhead;      
    private Vector2 startPosition;
    private Vector2 endPosition;

    public void Init(double targetBeat, int beatsAhead, Vector2 startPosition, Vector2 endPosition)
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        this.targetBeat = targetBeat;
        this.beatsAhead = beatsAhead;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        rect.anchoredPosition = startPosition;
    }

    private void Update()
    {
        double beatsLeft = targetBeat - BeatManager.instance.BeatPosition;
        float t = 1f - (float)(beatsLeft / beatsAhead);
        
        if (t >= 1f)
        {
            Destroy(gameObject);
            return;
        }
        
        rect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, Mathf.Max(0f, t));
        
        if (image != null)
        {
            Color c = image.color;
            c.a = Mathf.Clamp01(t * 2f);
            image.color = c;
        }
    }
}