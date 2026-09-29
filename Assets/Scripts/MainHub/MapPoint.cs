using System;
using TMPro;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    [Header("WayPoints")]
    [SerializeField]MapPoint[] up;
    [SerializeField]MapPoint[] right, down, left;

    [Header("Scene Options")] 
    [SerializeField] private int LevelIndex = 0;

    public string sceneToLoad;
    [TextArea(1, 2)] public string levelName;


    [Header("MapPoint Options")] public bool isLevel;
    public bool isCorner;
    public bool isWarpPoint;

    [Header("Warp Options")] public bool autoWarp;
    public bool hasWarped;
    public MapPoint warpPoint;

    [Header("Level UI Objects")] [SerializeField]
    private TextMeshProUGUI levelNameText = null;
    [SerializeField]GameObject levelPanel = null;
    
    SpriteRenderer _spriteRenderer;
    
    
    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (levelPanel != null)
        {
            levelPanel.SetActive(false);
        }
        
        //if NOT level or warp, set sprite image to null
        if (!isLevel && !isWarpPoint)
        {
            _spriteRenderer.sprite = null;
        }
        else
        {
            //this can also be edited for like saving data throught the levels
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (levelPanel != null)
            {
                levelPanel.SetActive(true);
            }
            if(levelNameText != null)
                {
                levelNameText.text = levelName;
                }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
