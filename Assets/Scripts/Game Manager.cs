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
    private Animator animator;
    [SerializeField] private GameObject playerMap;
    [SerializeField] private string _map;
    public string desiredLevel;
    public float bossLeftHealth;
    
    private Vector2 lastPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        animator = GameObject.FindGameObjectsWithTag("Transition")[0].GetComponent<Animator>();
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
    
    public void LoadScene(string sceneName)
    {
        desiredLevel = sceneName;
        TransitionInLevel();
    }

    private void TransitionInLevel()
    {
        if (animator != null)
            animator.Play("Trans_Out");
    }

    public void TransitionOutLevel()
    {
        if (animator != null)
            animator.Play("Trans_In");
        SceneManager.LoadScene(desiredLevel);
        animator = GameObject.FindGameObjectsWithTag("Transition")[0].GetComponent<Animator>();
        
    }

    public void LoadScenebyInt(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
    
    public void ReturnToMap(string mapName)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        LoadScene(mapName); //te lo he cambiado para que se pueda elegir si volver al mapa o al menu
        if (playerMap!= null)
            playerMap.transform.position = lastPosition;
    }
    
}