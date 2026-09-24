using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPoint : MonoBehaviour
{
    [SerializeField] private string levelSceneName;

    private bool playerInside;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            LoadLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void LoadLevel()
    {
        GameManager.Instance.LoadLevel(levelSceneName);
    }
}