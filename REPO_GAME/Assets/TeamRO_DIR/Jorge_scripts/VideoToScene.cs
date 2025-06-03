using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoToScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Asigna el VideoPlayer desde el Inspector
    public string nextSceneName = "NombreDeTuSiguienteEscena";  // Nombre de la escena a cargar

    void Start()
    {
        // Verifica si el VideoPlayer está asignado
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Suscribe el evento para detectar cuando el video termine
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Método llamado cuando el video termina
    void OnVideoEnd(VideoPlayer vp)
    {
        // Carga la siguiente escena
        SceneManager.LoadScene(nextSceneName);
    }

    // Opcional: Permitir saltar el video con una tecla
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}