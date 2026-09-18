using System;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
   [SerializeField, Range(30f, 240f)] private float BPM;
   [SerializeField] private AudioClip blip;
   [SerializeField, Range(0.01f, 1f)] private float windowOfInteractions;
   public static BeatManager instance;
   private double nextTimeBeat;
   private double intervalTime;

   [Header("Opcional (WIP")] 
   [SerializeField, Range(0.01f, 1f)] private float perfectWindow;
   [SerializeField, Range(0.01f, 1f)] private float okWindow;
   public enum Score
   {
      Perfect,
      Ok,
      Missed
   }

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
      //Por si queremos recalibrarlo no lo voy a quitar del todo que sino luego es un dolor
      //Debug.Log(nextTimeBeat);
   }
   //Recordadme que borre esto jajaaj pero es liada y por si lo tocamos en el futuro lo voy a ir documentando (terrible codigo)
   public bool CanAttack()
   {
      //los hago aqui los bools para no tener que guardarlos, y pillamos el tiempo que es y miramos cuanto tiempo ha pasado
      //desde que el ultimo beat ha sonao y hacemos cuanto tiempo de respuesta tiene antes y despues de cada beat
      double currentTime = AudioSettings.dspTime;
      double timeSinceTheLastBeat = intervalTime - (nextTimeBeat- currentTime);
      double window = intervalTime * windowOfInteractions;
      //esto es simplemente que si esta o detras o delante del beat lo detecta y no tiene porq ser solo por delante, aunq creo
      //que tamb funcionaria si los juntamos lo suficiente.
      return timeSinceTheLastBeat < window || timeSinceTheLastBeat >= intervalTime- window;
   }
   //Esto lo use yo para mi plataformer, dependiendo de que queramos nos va a dar un enum de un tipo si perfecto malo o reguleras.
   public Score PrecisionCheck()
   {
      //El codigo como tal es redundante, sin embargo esto al final no se si lo vais a querer dejar o no, es un poco lo
      //mismo pero en vez de ser si o no calcula una franja porcentual entre cada distancia del beat y te dice si lo has
      //hecho perfecto ok o reguleras, la linea del math.min te mira si esta mas cerca del anterior o del siguiente
      double currentTime = AudioSettings.dspTime;
      double timeSinceTheLastBeat = intervalTime - (nextTimeBeat - currentTime);
      double distanceFromBeat = Math.Min(timeSinceTheLastBeat, intervalTime - timeSinceTheLastBeat);
      
      double perfect = intervalTime * perfectWindow;
      double ok = intervalTime *  okWindow;
      if (distanceFromBeat <= perfect)
      {
         return Score.Perfect;
      }
      else if (distanceFromBeat <= ok)
      {
         return Score.Ok;
      }
      else
      {
         return Score.Missed;
      }
   }
}
