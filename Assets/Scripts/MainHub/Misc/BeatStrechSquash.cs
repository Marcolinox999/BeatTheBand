using UnityEngine;

public class BeatSquashStretch : MonoBehaviour
{
    [Header("Rhythm")]
    [SerializeField] private float bpm = 120f;

    [Header("Animation")]
    [SerializeField] private float stretchAmount = 0.15f;
    [SerializeField] private float squashAmount = 0.08f;
    [SerializeField] private float animationSharpness = 8f;
    
    [Header("Extra")]
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Sprite normalLevel;
    [SerializeField] private Sprite BWLevel;
    [SerializeField] private bool levelBeaten = false;

    private Vector3 baseScale;
    private float beatTimer;
     SpriteRenderer spriteRenderer;

    private void Start()
    {
        baseScale = transform.localScale;
        particles.Play(!levelBeaten);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalLevel;
        bpm = BeatManager.instance.BPM;
    }

    private void Update()
    {
        if (!levelBeaten)
            Bounce();
        else
        {
            spriteRenderer.sprite = BWLevel;
            transform.localScale = new Vector3(1, 1, 1);
            particles.Pause(levelBeaten);
        }
    }

    private void Bounce() //animación gassy
    {
        float beatDuration = 60f / bpm;

        beatTimer += Time.deltaTime;

        if (beatTimer > beatDuration)
            beatTimer -= beatDuration;

        float t = beatTimer / beatDuration;
        
        float pulse = Mathf.Sin(t * Mathf.PI);
        pulse = Mathf.Pow(pulse, animationSharpness);

        float scaleY = 1f + stretchAmount * pulse;
        float scaleX = 1f - squashAmount * pulse;

        transform.localScale = new Vector3(baseScale.x * scaleX, baseScale.y * scaleY, baseScale.z);
    }
}