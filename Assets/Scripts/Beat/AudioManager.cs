using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource buttonSceneClip;
    [SerializeField] private AudioSource buttonStartGameClip;
    
    SceneManager sceneManager;
    
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //Its not going to be destroyed
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    } 
    public void PlaySFX(AudioClip clipToPlay)
    {
        sfxSource.PlayOneShot(clipToPlay);
    }

    public void PlayMusic(AudioClip musicToPlay)
    {
        musicSource.Stop();
        musicSource.clip = musicToPlay;
        musicSource.Play();
    }

    public void PlayScheduledBeat(AudioClip clipToPlay, double time)
    {
        sfxSource.clip = clipToPlay;
        sfxSource.PlayScheduled(time);
    }

    public void OnClickButtonToScene(string sceneToLoad)
    {
        buttonSceneClip.Play();
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnCLickButtonToStart(string sceneToLoad)
    {
        buttonStartGameClip.Play();
        SceneManager.LoadScene(sceneToLoad);
    }
}
