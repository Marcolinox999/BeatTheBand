using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [Serializable]
    public class SceneButton
    {
        public Button button;
        public string sceneName;
    }

    [SerializeField] private SceneButton[] sceneButtons;

    private void Awake()
    {
        foreach (SceneButton entry in sceneButtons)
        {
            if (entry.button == null || string.IsNullOrEmpty(entry.sceneName)) continue;
            string sceneName = entry.sceneName;
            entry.button.onClick.AddListener(() => LoadScene(sceneName));
        }
    }
    public void LoadScene(string sceneName)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}