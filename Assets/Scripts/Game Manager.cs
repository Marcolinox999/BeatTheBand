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
    public bool[] unlockedWeapons = new bool[4];
    public bool[] beatenLevels = new bool[3];
    [SerializeField] private GameObject playerMap;
    [SerializeField] private string _map;

    
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
        unlockedWeapons[0] = true;
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

    public void UnlockWeapon(int weaponID)
    {
        unlockedWeapons[weaponID] = true;
    }
}