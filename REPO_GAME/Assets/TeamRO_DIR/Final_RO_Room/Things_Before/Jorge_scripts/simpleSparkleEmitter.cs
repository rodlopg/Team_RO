using UnityEngine;

public class SimpleSparkEmitter : MonoBehaviour
{
    [Header("Configuración Simple")]
    public float cadaCuantosSegundos = 2f;
    public int cuantasChispas = 10;

    private ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();

        // Desactivar emisión automática
        var emission = particles.emission;
        emission.enabled = false;

        // Comenzar a emitir chispas cada X segundos
        InvokeRepeating("HacerChispas", 0f, cadaCuantosSegundos);
    }

    void HacerChispas()
    {
        particles.Emit(cuantasChispas);
        Debug.Log($"¡Chispas! ({cuantasChispas} partículas)");
    }

    // Para detener/reanudar
    public void PararChispas()
    {
        CancelInvoke("HacerChispas");
    }

    public void ReanudarChispas()
    {
        InvokeRepeating("HacerChispas", 0f, cadaCuantosSegundos);
    }
}