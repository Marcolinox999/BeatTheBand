using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [Header("Configuración")]
    public Slider slider;
    public float valorMaximo = 1500f;
    public float velocidad = 0.5f;

    private float valorActual = 0f;
    private float valorObjetivo = 0f;

    void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 0f;

        valorActual = 0f;

        float vidaRestante = GameManager.Instance.bossLeftHealth;

        // El progreso representa la vida perdida
        valorObjetivo = 1f - Mathf.Clamp01(vidaRestante / valorMaximo);

        Debug.Log("Vida restante: " + vidaRestante);
        Debug.Log("Progreso final: " + valorObjetivo);
    }

    void Update()
    {
        valorActual = Mathf.MoveTowards(
            valorActual,
            valorObjetivo,
            velocidad * Time.deltaTime
        );

        slider.value = valorActual;
    }
}