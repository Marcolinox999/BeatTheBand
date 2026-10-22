using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGManagerLink : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject pauseMenu;


    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            pauseMenu.SetActive(false);
        }
        else if  (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void ResumeIPauseMenu()
    {
        GameManager.Instance.PauseGameIResumeGame();
    }
    
    public void Return()
    {
        GameManager.Instance.ReturnToMap(sceneName);
    }

    public void Reload()
    {
        GameManager.Instance.ReloadScene();
    }
}
