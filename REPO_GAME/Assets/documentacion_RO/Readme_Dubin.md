## Importante
Se tiene que asignar un empty acomodado en la orientacion en la que quieres la lampara el eje Y es el frente de la lampara
al empty se le tiene que dar el nombre obligatorio de Hand

## 🎮 Proyecto Unity: Modelos, Animaciones, Música y Sistema de Lámpara
Este documento detalla el proceso completo de desarrollo, desde la extracción de modelos hasta la implementación de animaciones, música y mecánicas interactivas en Unity.

## 🏗️ 1. Diseño del Escenario con ProBuilder
🔹 Creación de la Estructura
Utilicé ProBuilder (herramienta de modelado integrada en Unity) para diseñar:

Cuartos principales: Espacios cerrados con proporciones realistas.

Pasillo central: Conecta todos los cuartos con un diseño fluido.

Paredes, techos y pisos: Geometría básica con colisiones optimizadas.

🔹 Flujo de Trabajo
Formas básicas: Creé cubos y los ajusté para formar paredes y suelos.

Extrusión de puertas: Usé herramientas de corte (Vertex Editing) para abrir espacios.

Optimización:

🔹 Texturizado
Asigné materiales directamente desde ProBuilder:

Suelos: Texturas de madera/mármol.

Paredes: Colores planos o patrones simples.

Techos: Materiales blancos opacos.

## 📦 2. Extracción y Preparación de Modelos
🔹 Steam Workshop → Blender → Maximo
Descarga de modelos desde Steam Workshop (formato .gma, .vpk).

Descompresión con:

GMA Extractor (archivos .gma).

Crowbar (modelos .mdl y .vtx).

Importación a Blender usando SourceIO para ajustar mallas y texturas.

Exportación a OBJ para llevarlo a Maximo.

Rigging y animaciones en Maximo:

Se agregaron 4 animaciones de baile.

Exportación final en .fbx para Unity.

## 🎵 3. Sistema de Música Aleatoria
🔹 Código: NpcMusicPlayer.cs
Controla la reproducción aleatoria de canciones en un NPC, con configuración 3D y filtrado de fuentes de audio.
using UnityEngine;

```csharp
public class NpcMusicPlayer : MonoBehaviour
{
    public AudioClip[] musicOption; // Array de canciones disponibles
    public int musicIndex = 0;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Validaciones iniciales
        if (musicOption.Length == 0)
        {
            Debug.LogWarning("No music chico");
            return;
        }

        if (musicIndex < 0 || musicIndex >= musicOption.Length)
        {
            Debug.LogWarning("Selected index is out of bounds.");
            return;
        }

        // Configuración del AudioSource
        audioSource.clip = musicOption[musicIndex];
        audioSource.volume = 0.2f;          // Volumen bajo (20%)
        audioSource.loop = false;           // No repetir la misma canción
        audioSource.spatialBlend = 1f;      // Sonido 100% 3D
        audioSource.minDistance = 1f;       // Distancia mínima de audibilidad
        audioSource.maxDistance = 2f;       // Distancia máxima (atenúa linealmente)
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        PlayRandomTrack(); // Inicia la reproducción
    }

    void Update()
    {
        // Si no hay música reproduciéndose, elige una nueva canción
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

Exclusión de fuentes con tag `"ignorar":
if (source.CompareTag("ignorar")) {
    if (showDebug) Debug.Log("Ignoring music source: " + source.name);
    continue;
}
```

## 🕯️ 4. Sistema de Lámpara para el Jugador
🔹 Código: LanternSpawn.cs
Mecánica:

El jugador presiona E dentro de un trigger para recibir una lámpara.

La lámpara se instancia en un Empty llamado "Hand".

Flujo:

Detección del jugador (OnTriggerEnter).

Búsqueda automática del Transform de la mano (si no está asignado).

Instanciación de la lámpara al presionar E.

Prevención de duplicados con hasGivenLamp.
```csharp
private void Update() {
    if (playerInTrigger && !hasGivenLamp && Input.GetKeyDown(KeyCode.E)) {
        Instantiate(lampPrefab, currentPlayerHand.position, currentPlayerHand.rotation, currentPlayerHand);
        hasGivenLamp = true;
    }
}
 ```
## 🎨 5. Ambientación y Texturizado
Se aplicaron texturas a:

Piso.

Paredes.

Techos.

## 🔄 6. Animator Controller (Bucle Infinito)
4 animaciones de baile en ciclo continuo:
Anim1 → Anim2 → Anim3 → Anim4 → Anim1 (loop)
Transiciones automáticas sin condiciones.