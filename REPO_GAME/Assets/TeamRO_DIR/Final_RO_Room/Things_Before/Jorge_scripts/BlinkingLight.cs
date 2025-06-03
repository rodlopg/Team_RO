using System.Collections;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    [Header("Configuración de Parpadeo")]
    public float tiempoEncendida = 0.5f;  // medio segundo
    public float tiempoApagada = 0.5f;    // medio segundo
    public bool iniciarAutomaticamente = true;

    private Light luzComponente;
    private bool estaParpeando = false;

    void Start()
    {
        // Obtener el componente Light
        luzComponente = GetComponent<Light>();

        if (luzComponente == null)
        {
            Debug.LogError("No se encontró un componente Light en este GameObject!");
            return;
        }

        // Iniciar el parpadeo automáticamente si está configurado
        if (iniciarAutomaticamente)
        {
            IniciarParpadeo();
        }
    }

    public void IniciarParpadeo()
    {
        if (!estaParpeando)
        {
            estaParpeando = true;
            StartCoroutine(CorrutinaParpadeo());
        }
    }

    public void DetenerParpadeo()
    {
        estaParpeando = false;
        StopAllCoroutines();
        luzComponente.enabled = true; // Dejar encendida por defecto
    }

    private IEnumerator CorrutinaParpadeo()
    {
        while (estaParpeando)
        {
            // Encender la luz
            luzComponente.enabled = true;
            yield return new WaitForSeconds(tiempoEncendida);

            // Apagar la luz
            luzComponente.enabled = false;
            yield return new WaitForSeconds(tiempoApagada);
        }
    }
}
