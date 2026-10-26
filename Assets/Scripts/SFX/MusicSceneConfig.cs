using UnityEngine;

public class MusicSceneConfig : MonoBehaviour
{
    [Header("Configuración musical")]
    [SerializeField] private AudioClip music;
    [SerializeField, Range(30f, 240f)] private float bpm = 120f;

    [Header("Sincronización")]
    [SerializeField] private float firstBeatOffset = 0f;

    private void Start()
    {
        Debug.Log(1);
        AudioManager.instance.PlaySongSynced(music, bpm, firstBeatOffset);
    }
}