using UnityEngine;

public class BeatSquashStretch : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float stretchAmount = 0.15f;
    [SerializeField] private float squashAmount = 0.08f;
    [SerializeField] private float animationSharpness = 8f;

    [Header("Ground Anchor")]
    [SerializeField] private bool anchorToGround = true;

    [Header("Side Sway")]
    [SerializeField] private bool enableSideSway = false;
    [SerializeField] private float swayAmount = 0.1f;
    [SerializeField] private float swaySpeedMultiplier = 1f;

    [Header("Extra")]
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Sprite normalLevel;
    [SerializeField] private Sprite BWLevel;
    [SerializeField] private bool levelBeaten = false;

    private Vector3 baseScale;
    private Vector3 basePosition;

    private float beatTimer;
    private float bpm;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        baseScale = transform.localScale;
        basePosition = transform.position;

        particles.Play(!levelBeaten);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalLevel;

        bpm = BeatManager.instance.BPM;
    }

    private void Update()
    {
        if (!levelBeaten)
        {
            Bounce();
        }
        else
        {
            spriteRenderer.sprite = BWLevel;

            transform.localScale = baseScale;
            transform.position = basePosition;

            particles.Pause(levelBeaten);
        }
    }

    private void Bounce()
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

        Vector3 newScale = new Vector3(
            baseScale.x * scaleX,
            baseScale.y * scaleY,
            baseScale.z
        );

        transform.localScale = newScale;

        Vector3 newPosition = basePosition;

        // Mantener la base fija
        if (anchorToGround)
        {
            float addedHeight = newScale.y - baseScale.y;
            newPosition.y += addedHeight * 0.5f;
        }

        // Balanceo lateral opcional
        if (enableSideSway)
        {
            float sway =
                Mathf.Sin(
                    t * Mathf.PI * 2f * swaySpeedMultiplier
                ) * swayAmount * pulse;

            newPosition.x += sway;
        }

        transform.position = newPosition;
    }
}