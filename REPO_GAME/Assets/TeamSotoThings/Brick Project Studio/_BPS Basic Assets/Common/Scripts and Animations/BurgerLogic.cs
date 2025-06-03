using UnityEngine;

public class BurgerLogic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip beepSound;
    public AudioClip eatSound;
    public GameObject player;

    // Posiciones preestablecidas para que cambie de lugar la hamburguesa
    private Vector3[] positions = new Vector3[4]{ new Vector3(-1.55999994f, 1.57700002f, 9.52600002f), new Vector3(-0.293000013f, 1.57700002f, 5.52099991f), new Vector3(0.875f, 1.50100005f, 11.1780005f), new Vector3(-4.2670002f, 1.66499996f, 9.66499996f) };
    private float timer = 2f;

    void Update()
    {
        if (timer < 0) {
            // Cambiar de posicion aleatoriamente cada 2 segundos
            randPosition();
            timer = 2f;
        }

        timer -= Time.deltaTime;
    }

    // Si el jugador toca la hamburguesa, se destruye
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            audioSource.clip = eatSound;
            audioSource.Play();
            Destroy(gameObject);
        }
    }

    public void beep() {
        audioSource.clip = beepSound;
        audioSource.Play();
    }

    // Cambio de posicion aleatorio
    public void randPosition() {
        Vector3 randPos = positions[Random.Range(0, positions.Length)];

        // La posicion aleatoria tiene que ser diferente a la actual
        while (randPos == gameObject.transform.position) {
            randPos = positions[Random.Range(0, positions.Length)];
        }

        gameObject.transform.position = randPos;
    }
}
