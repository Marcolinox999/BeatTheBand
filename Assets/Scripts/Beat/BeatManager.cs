using System;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
   [SerializeField, Range(30f, 240f)] private float BPM;
   [SerializeField] private AudioClip blip;
   private static BeatManager instance;
   private double nextTimeBeat;
   private double intervalTime;

   private void Awake()
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

   private void Start()
   {
      CalculateTheBeat();
      nextTimeBeat = AudioSettings.dspTime;
   }

   private void CalculateTheBeat()
   {
      intervalTime = 60 / BPM;
   }

   private void Update()
   {
      if (AudioSettings.dspTime >= nextTimeBeat)
      {
         Beat();
         nextTimeBeat += intervalTime; 
      }
   }

   private void Beat()
   {
      AudioManager.instance.PlayScheduledBeat(blip,intervalTime);
      Debug.Log(nextTimeBeat);
   }
}
