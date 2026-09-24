using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Playing,
        Paused,
    }

    public GameState CurrentState { get; private set; }
    [SerializeField] private GameObject playerMap;
    [SerializeField] private string _map;
    //[SerializeField] private int
    
    private Vector2 lastPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentState = GameState.Playing;
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Paused;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
    }

    public void LoadLevel(string levelSceneName)
    {
        lastPosition = playerMap.transform.position;
        SceneManager.LoadScene(levelSceneName);
    }

    public void ReturnToMap()
    {
        SceneManager.LoadScene(_map);
        playerMap.transform.position = lastPosition;
    }
}