using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //Its not going to be destroyeda
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
}
