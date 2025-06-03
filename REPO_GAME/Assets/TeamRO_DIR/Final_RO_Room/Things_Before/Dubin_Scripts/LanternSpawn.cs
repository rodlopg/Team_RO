using UnityEngine;

public class LanternSpawn : MonoBehaviour
{
    public GameObject lampPrefab; //se tiene que asignar un prefab de la lampara
    public Transform handTransform; //es un empty el cual se coloca en la mano, esto es para darle la posicion y rotacion al prefab 

    private bool playerInTrigger = false;// checa si esta dentro del boxCollider
    private Transform currentPlayerHand; //se le tiene que asignar al jugador si o si el empty llamado Hand
    private bool hasGivenLamp = false; // checa si ya se le dio una lampara al jugador

    private void OnTriggerEnter(Collider other)
    {
        //checa si el jugador tiene el empty de la mano y si esta asignado en el inspector 
        //busca el empty en el jugador llamado Hand
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador entro al trigger");
            playerInTrigger = true;

            if (handTransform == null)
            {
                Transform hand = other.transform.Find("Hand");
                if (hand != null)
                {
                    currentPlayerHand = hand;
                    Debug.Log("Se encontro la mano del jugador automaticamente.");
                }
                else
                {
                    Debug.LogWarning("No se encontro un objeto llamado 'Hand' en el jugador.");
                }
            }
            else
            {
                currentPlayerHand = handTransform;
                Debug.Log("Se uso la mano asignada desde el inspector.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador salió del trigger");
            playerInTrigger = false;
        }
    }

    private void Update()
    {
        //checa si el jugador esta dentro del trigger no tiene lampara y le pico a la tecla e
        if (playerInTrigger && !hasGivenLamp && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Tecla E presionada dentro del trigger");
            if (currentPlayerHand != null)
            {
                Instantiate(lampPrefab, currentPlayerHand.position, currentPlayerHand.rotation, currentPlayerHand);
                hasGivenLamp = true;
                Debug.Log("Lámpara instanciada en la mano del jugador.");
            }
            else
            {
                Debug.LogWarning("No se puede instanciar la lámpara porque no se encontró la mano.");
            }
        }
    }
}
