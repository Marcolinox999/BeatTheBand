using System.Collections;
using UnityEngine;

public class MenuStaticInterruptions : MonoBehaviour
{
    [System.Serializable]
    public class StaticLevel
    {
        public Vector2 timeBetweenBursts = new Vector2(8f, 15f);

        public Vector2 burstDuration = new Vector2(0.1f, 0.3f);

        [Range(0f, 1f)] public float staticVolume = 0.3f;

        [Range(0f, 1f)] public float musicDuck = 0.5f;
    }

    [Header("Audio")]
    private AudioSource musicSource;
    [SerializeField] private AudioSource staticSource;

    [Header("Intensity per beaten level")]
    [SerializeField] private StaticLevel[] levels = new StaticLevel[]
    {
        null,
        new StaticLevel { timeBetweenBursts = new Vector2(10f, 18f), burstDuration = new Vector2(0.1f, 0.25f), staticVolume = 0.25f, musicDuck = 0.6f },
        new StaticLevel { timeBetweenBursts = new Vector2(4f, 9f),   burstDuration = new Vector2(0.2f, 0.6f),  staticVolume = 0.5f,  musicDuck = 0.3f },
        new StaticLevel { timeBetweenBursts = new Vector2(1f, 3f),   burstDuration = new Vector2(0.4f, 1.2f),  staticVolume = 0.8f,  musicDuck = 0f }
    };

    private float baseMusicVolume;

    private void Awake()
    {
        musicSource = GameObject.FindGameObjectWithTag("musicSource").GetComponent<AudioSource>();
    }
    private void Start()
    {
        baseMusicVolume = musicSource.volume;
        staticSource.loop = true;
        staticSource.volume = 0f;
        staticSource.Play();

        StartCoroutine(StaticRoutine());
    }

    private int CountBeatenLevels()
    {
        int count = 0;
        foreach (bool beaten in GameManager.Instance.beatenLevels)
        {
            if (beaten) count++;
        }
        return count;
    }

    private IEnumerator StaticRoutine()
    {
        while (true)
        {
            int beatenCount = Mathf.Min(CountBeatenLevels(), levels.Length - 1);
            StaticLevel level = levels[beatenCount];

            if (level == null)
            {
                yield return new WaitForSecondsRealtime(1f);
                continue;
            }

            yield return new WaitForSecondsRealtime(
                Random.Range(level.timeBetweenBursts.x, level.timeBetweenBursts.y));

            staticSource.volume = level.staticVolume;
            musicSource.volume = baseMusicVolume * level.musicDuck;

            yield return new WaitForSecondsRealtime(
                Random.Range(level.burstDuration.x, level.burstDuration.y));
            
            staticSource.volume = 0f;
            musicSource.volume = baseMusicVolume;
        }
    }
}