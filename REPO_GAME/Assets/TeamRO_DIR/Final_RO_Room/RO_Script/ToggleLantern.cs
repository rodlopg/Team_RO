using UnityEngine;

public class ToggleLantern : MonoBehaviour
{
    // Referencia al objeto de luz UV
    [SerializeField] private GameObject UVLightObject;
    // Referencia al objeto de luz focal (spotlight)
    [SerializeField] private GameObject spotLightObject;

    private void Start()
    {
        // Inicializar ambas luces en estado apagado
        SetLightState(UVLightObject, false);
        SetLightState(spotLightObject, false);
    }

    private void Update()
    {
        // Verificar si se presionó la tecla U para alternar luz UV
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleLight(ref UVLightObject, ref spotLightObject, "UV light");
        }
        // Verificar si se presionó la tecla I para alternar spotlight
        else if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleLight(ref spotLightObject, ref UVLightObject, "Spotlight");
        }
    }

    // Método para alternar el estado de una luz y apagar la otra
    private void ToggleLight(ref GameObject lightToToggle, ref GameObject otherLight, string lightName)
    {
        if (lightToToggle != null)
        {
            // Invertir el estado actual de la luz
            bool newState = !lightToToggle.activeSelf;
            SetLightState(lightToToggle, newState);

            // Apagar la otra luz si está encendida
            if (otherLight != null && otherLight.activeSelf)
            {
                SetLightState(otherLight, false);
            }

            // Mostrar mensaje de depuración con el nuevo estado
            Debug.Log($"{lightName} toggled: {(newState ? "ON" : "OFF")}");
        }
        else
        {
            // Advertencia si no se asignó el objeto de luz
            Debug.LogWarning($"{lightName} object not assigned!");
        }
    }

    // Método auxiliar para cambiar el estado de un objeto de luz
    private void SetLightState(GameObject lightObject, bool state)
    {
        if (lightObject != null)
        {
            lightObject.SetActive(state);
        }
    }
}