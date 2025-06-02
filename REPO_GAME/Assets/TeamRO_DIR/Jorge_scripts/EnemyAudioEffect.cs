using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudioEffect : MonoBehaviour
{
    // Archivos de audio asignados directamente en el código
    private AudioClip[] audioClips = new AudioClip[3];
    private AudioSource audioSource;

    // Configuración de detección
    public float detectionRange = 15f;
    private int currentClipIndex = 0;
    private bool playerWasInRange = false;

    void Awake()
    {
        // 1. Configurar AudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f; // Audio 3D

        // 2. Cargar los archivos de audio directamente desde Resources
        LoadAudioClips();
    }

    void LoadAudioClips()
    {
        // Asignación directa de archivos (deben estar en Resources/Audio)
        audioClips[0] = Resources.Load<AudioClip>("Audio/efecto_enemy_1");
        audioClips[1] = Resources.Load<AudioClip>("Audio/efecto_enemy_2");
        audioClips[2] = Resources.Load<AudioClip>("Audio/efecto_enemy_3");

        // Verificar que se cargaron correctamente
        foreach (var clip in audioClips)
        {
            if (clip == null) Debug.LogError("¡No se encontró un archivo de audio!");
        }
    }

    void Update()
    {
        // Buscar jugadores con tag "Player" en el rango
        bool playerInRange = IsPlayerNearby();

        if (playerInRange)
        {
            if (!playerWasInRange || !audioSource.isPlaying)
            {
                // Reproducir siguiente clip cuando:
                // 1. Un jugador entra en rango, o
                // 2. Termina el clip anterior
                PlayNextClip();
            }
            playerWasInRange = true;
        }
        else
        {
            if (playerWasInRange)
            {
                // Detener si el jugador se aleja
                audioSource.Stop();
            }
            playerWasInRange = false;
        }
    }

    bool IsPlayerNearby()
    {
        // Encontrar todos los objetos con tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= detectionRange)
            {
                return true;
            }
        }
        return false;
    }

    void PlayNextClip()
    {
        if (audioClips.Length == 0) return;

        audioSource.clip = audioClips[currentClipIndex];
        audioSource.Play();

        // Avanzar al siguiente clip (circular)
        currentClipIndex = (currentClipIndex + 1) % audioClips.Length;
    }

    // Visualización del rango en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}