using TMPro;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    [Header("WayPoints")]
    public MapPoint[] up;
    public MapPoint[] right, down, left;

    [Header("Scene Options")]
    [SerializeField] private int levelIndex = 0; // index of this level in GameManager.beatenLevels
    public string sceneToLoad;
    [TextArea(1, 2)] public string levelName;

    [Header("MapPoint Options")]
    public bool isLevel;
    public bool isCorner;
    public bool isWarpPoint;
    public bool isFinal;

    [Header("Warp Options")]
    public bool autoWarp;
    public bool hasWarped;
    public MapPoint warpPoint;

    [Header("Level UI Objects")]
    [SerializeField] private TextMeshProUGUI levelNameText = null;
    [SerializeField] private GameObject levelPanel = null;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (levelPanel != null)
        {
            levelPanel.SetActive(false);
        }
    }

    // Returns true when every level except this one has been beaten.
    // For the final point, call this before allowing the player to enter.
    public bool AreAllOtherLevelsBeaten()
    {
        bool[] beaten = GameManager.Instance.beatenLevels;

        for (int i = 0; i < beaten.Length; i++)
        {
            if (i == levelIndex) continue; // skip the final level itself
            if (!beaten[i]) return false;
        }
        return true;
    }

    // Single place to ask "can the player enter this point?"
    public bool CanBeEntered()
    {
        if (isFinal) return AreAllOtherLevelsBeaten();
        return true;
    }

    public void ActivateLevelUI()
    {
        if (levelPanel != null)
        {
            levelPanel.SetActive(true);
        }
        if (levelNameText != null)
        {
            levelNameText.text = levelName;
        }
    }

    public void DeactivateLevelUI()
    {
        if (levelPanel != null)
        {
            levelPanel.SetActive(false);
        }
        if (levelNameText != null)
        {
            levelNameText.text = "";
        }
    }
}