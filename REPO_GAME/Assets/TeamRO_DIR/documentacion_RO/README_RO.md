📖 Documentación del Sistema de Interactivos Unity
🌟 Descripción General
Este paquete contiene varios sistemas interactivos para Unity que incluyen:

Sistema de keypad con combinación numérica

Gestor de progreso de puzzles

Control de luces interactivas

Efecto de revelado con spotlight

Componentes base para puzzles

🛠️ Componentes Principales
🔢 Keypad System (NavKeypad/Keypad.cs)
Función: Teclado numérico interactivo con validación de combinación.

csharp
// Ejemplo de uso básico:
[SerializeField] private Keypad keypad;
void Start() {
    keypad.OnAccessGranted.AddListener(OpenDoor);
}
Características:

Código configurable (hasta 9 dígitos)

Retroalimentación visual (cambios de color)

Efectos de sonido integrados

Eventos para acceso concedido/denegado

🧩 Puzzle Manager (GameManager.cs)
Función: Control central de progreso del juego.

csharp
// Registrar puzzle resuelto:
GameManager.Instance.PuzzleSolved();
Características:

Sistema de conteo de puzzles

Transición automática entre escenas

Actualización de UI de progreso

Patrón Singleton

💡 Light Controller (ToggleLantern.cs)
Función: Control alternado de luces con teclado.

csharp
// Teclas por defecto:
// U - Luz UV
// I - Spotlight
Características:

Sistema de mutuo exclusivo (solo una luz activa)

Mensajes de depuración

Fácil configuración desde Inspector

✨ Reveal Effect (Reveal.cs)
Función: Efecto de revelado con spotlight.

csharp
// Requiere material con propiedades:
_LightPos (Vector3)
_LightDir (Vector3) 
_LightAngle (Float)
Características:

Funciona en modo Editor y Play

Sincronización automática con luz

Validación de tipo de luz

🧩 Puzzle Base (Puzzle.cs)
Función: Componente base para objetos interactivos.

csharp
// Para resolver puzzle:
GetComponent<Puzzle>().Solve();
Características:

Evento personalizado al resolver

Prevención de doble activación

Integración con GameManager

🖥️ Requisitos del Sistema
Unity 2019.4 o superior

Render Pipeline Standard o URP (con ajustes)

Input System de Unity

🛠️ Configuración
Importar el paquete a tu proyecto Unity

Configurar los prefabs según necesidades

Asignar referencias en el Inspector

Conectar eventos a otros sistemas

📝 Notas Adicionales
Todos los scripts están comentados internamente

Los materiales deben usar shaders compatibles

Se recomienda usar el Input System para controles

📄 Licencia
MIT License - Libre uso y modificación

Este README proporciona una visión general del sistema. Cada script contiene comentarios detallados sobre su implementación específica.