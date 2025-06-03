using UnityEngine;

public class NpcMusicPlayer : MonoBehaviour
{
    //variables para escoger una de las 4 canciones random
    public AudioClip[] musicOption;
    public int musicIndex=0;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (musicOption.Length == 0)
        {
            Debug.LogWarning("No muisic chico");
            return;
        }

        if (musicIndex < 0 || musicIndex >= musicOption.Length)
        {
            Debug.LogWarning("Selected index is out of bounds.");
            return;
        }

        audioSource.clip = musicOption[musicIndex];
        audioSource.volume = 0.2f;     // modifica el volumen
        audioSource.loop = false;        // Para que no se repita la cancion
        audioSource.spatialBlend = 1f;  // Convierte el sonido en 3D
        audioSource.minDistance = 1f;   // Distancia a la que se oye fuerte
        audioSource.maxDistance = 2f;  // Mas lejos = mas bajo
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        PlayRandomTrack();

    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    void PlayRandomTrack()
    {
        if (musicOption.Length == 0) return;

        int randomIndex = Random.Range(0, musicOption.Length);
        audioSource.clip = musicOption[randomIndex];
        audioSource.Play();
    }
}

