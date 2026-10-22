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
    public BeatManager.Score score;
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

    public void PauseGameIResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            CurrentState = GameState.Playing;
            Time.timeScale = 1f;
        }
        else if (CurrentState == GameState.Playing)
        {
            CurrentState = GameState.Paused;
            Time.timeScale = 0f;
        }
    }
    public void ReturnToMap(string mapName)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(mapName); //te lo he cambiado para que se pueda elegir si volver al mapa o al menu
        playerMap.transform.position = lastPosition;
    }

    public void ReloadScene()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        Instance.CurrentState = GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void CompleteLevel(int levelID)
    {
        if (levelID < 0 || levelID >= beatenLevels.Length) return;
        beatenLevels[levelID] = true;
    }

    public void UnlockWeapon(int weaponID)
    {
        if (weaponID < 0 || weaponID >= unlockedWeapons.Length) return;
        unlockedWeapons[weaponID] = true;
    }

    public void UpdateScore(int score)
    {
        
    }
}