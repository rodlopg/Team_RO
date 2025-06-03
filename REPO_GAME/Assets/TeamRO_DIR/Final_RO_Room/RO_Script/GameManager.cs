using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instancia singleton del GameManager
    public static GameManager Instance;

    [Header("Puzzle Tracking")]
    [SerializeField] private int totalPuzzles = 3; // Número total de puzzles en el nivel
    private int solvedPuzzles = 0; // Contador de puzzles resueltos

    [Header("UI")]
    [SerializeField] private TMP_Text puzzleStatusText; // Texto UI para mostrar progreso

    [Header("Next Scene")]
    [SerializeField] private string sceneToLoadWhenDone = "NextLevel"; // Escena a cargar al completar todos los puzzles

    private void Awake()
    {
        // Implementación del patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: persistir entre escenas
        }
        else
        {
            Destroy(gameObject); // Eliminar duplicados
        }
    }

    private void Start()
    {
        // Actualizar UI al inicio
        UpdatePuzzleUI();
    }

    // Método llamado cuando se resuelve un puzzle
    public void PuzzleSolved()
    {
        solvedPuzzles++; // Incrementar contador
        UpdatePuzzleUI(); // Actualizar UI

        // Verificar si se completaron todos los puzzles
        if (solvedPuzzles >= totalPuzzles)
        {
            LoadNextScene(); // Cargar siguiente escena
        }
    }

    // Actualiza el texto UI con el progreso actual
    private void UpdatePuzzleUI()
    {
        if (puzzleStatusText != null)
        {
            puzzleStatusText.text = $"Puzzles Solved: {solvedPuzzles} / {totalPuzzles}";
        }
    }

    // Carga la siguiente escena
    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoadWhenDone);
    }
}