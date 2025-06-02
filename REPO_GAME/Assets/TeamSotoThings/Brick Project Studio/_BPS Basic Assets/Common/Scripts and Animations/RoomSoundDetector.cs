using System.Collections;
using UnityEngine;

public class RoomSoundDetector : MonoBehaviour
{
    public GameObject burger;

    private BurgerLogic bl;
    private float timer = 2f;
    private bool canBeep = false;

    void Start()
    {
        // Verificar si burger es nulo antes de intentar acceder a BurgerLogic
        if (burger != null)
        {
            bl = burger.GetComponent<BurgerLogic>();
        }
        else
        {
            Debug.LogError("No se ha asignado un objeto burger en el inspector.");
        }
    }

    void Update()
    {
        // Verificar si bl es nulo antes de usarlo
        if (bl != null && timer < 0 && canBeep)
        {
            bl.beep();
            timer = 2f;
        }

        timer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canBeep = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canBeep = false;
        }
    }
}
