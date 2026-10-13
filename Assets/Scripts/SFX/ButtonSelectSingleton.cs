using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSelectSingleton : MonoBehaviour
{
  public static ButtonSelectSingleton instance;
  
  [SerializeField] AudioSource buttonSound;

  void Awake()
  {
    if (instance == null)
    {
      Destroy(gameObject);
    }
    else
    {
      instance = this;
    }
    DontDestroyOnLoad(this.gameObject);
    
    buttonSound = GetComponent<AudioSource>();
  }
  public void NextScene()
  {
    SceneManager.LoadScene(1);
  }
}
