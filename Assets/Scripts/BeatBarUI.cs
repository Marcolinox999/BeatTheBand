using UnityEngine;

public class BeatBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BeatMarker markerPrefab;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform leftSpawn;
    [SerializeField] private RectTransform rightSpawn;
    [SerializeField] private RectTransform heart;

    [Header("Settings")]
    [SerializeField, Min(1)] private int beatsAhead = 4;

    private int nextTargetBeat;

    private void Start()
    {
        nextTargetBeat = beatsAhead;
    }

    private void Update()
    {
        double position = BeatManager.instance.BeatPosition;

        while (nextTargetBeat - beatsAhead <= position)
        {
            SpawnMarker(leftSpawn.anchoredPosition, nextTargetBeat);
            SpawnMarker(rightSpawn.anchoredPosition, nextTargetBeat);
            nextTargetBeat++;
        }
    }

    private void SpawnMarker(Vector2 startPosition, double targetBeat)
    {
        BeatMarker marker = Instantiate(markerPrefab, container);
        marker.Init(targetBeat, beatsAhead, startPosition, heart.anchoredPosition);
    }
}