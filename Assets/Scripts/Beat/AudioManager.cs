using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public bool musicChange;
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
            //It's not going to be destroyed
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

    public void PlayScheduledBeat(AudioClip clipToPlay, double dspTime)
    {
        if (clipToPlay == null)
            return;

        sfxSource.PlayOneShot(clipToPlay, 0f); // No uses esta línea.
    }

    public void OnClickButtonToScene(int sceneToLoad)
    {
        buttonSceneClip.Play();
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnCLickButtonToStart(int sceneToLoad)
    {
        buttonStartGameClip.Play();
        SceneManager.LoadScene(sceneToLoad);
    }
    public void PlaySongSynced(AudioClip song, float bpm, float firstBeatOffset = 0f)
    {
        if (song == null || bpm <= 0f)
            return;

        musicSource.Stop();
        musicSource.clip = song;
        //musicSource.loop = false;

        double startTime = AudioSettings.dspTime + 0.2;

        musicSource.PlayScheduled(startTime);

        BeatManager.instance.StartBeatSync(
            bpm,
            startTime + firstBeatOffset
        );
    }
    
}
