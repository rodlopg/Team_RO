using UnityEngine;

[ExecuteAlways] // Permite que el script se ejecute tanto en modo Play como en modo Edit
public class Reveal : MonoBehaviour
{
    [SerializeField] private Material mat; // Material que será afectado por el efecto de revelado
    [SerializeField] private GameObject spotLightObject; // Objeto que contiene la luz de tipo Spotlight

    private Light spotLight; // Componente Light de la spotlight

    private void OnEnable()
    {
        // Al activarse el componente, obtenemos la referencia al componente Light si existe
        if (spotLightObject != null)
            spotLight = spotLightObject.GetComponent<Light>();
    }

    private void Update()
    {
        // Validación de referencias necesarias
        if (mat == null || spotLightObject == null || spotLight == null)
            return;

        // Si la luz está apagada o desactivada en la jerarquía
        if (!spotLightObject.activeInHierarchy || !spotLight.enabled)
        {
            // Desactivamos el efecto de revelado en el material
            mat.SetFloat("_LightAngle", 0f); // Valor que apaga el efecto (podría ser negativo)
            return;
        }

        // Verificamos que la luz sea efectivamente un Spotlight
        if (spotLight.type != LightType.Spot)
        {
            Debug.LogWarning("La luz asignada no es un spotlight.");
            return;
        }

        // Pasamos los parámetros actualizados al shader del material:
        // 1. Posición de la luz en el mundo
        mat.SetVector("_LightPos", spotLight.transform.position);
        // 2. Dirección hacia donde apunta la luz (invertida porque el shader necesita la dirección hacia la luz)
        mat.SetVector("_LightDir", -spotLight.transform.forward);
        // 3. Ángulo del cono de luz
        mat.SetFloat("_LightAngle", spotLight.spotAngle);
    }
}