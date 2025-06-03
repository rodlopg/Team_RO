using UnityEngine;
using UnityEngine.Events;

// Define un tipo de evento especializado para puzzles resueltos
[System.Serializable]
public class PuzzleSolvedEvent : UnityEvent { }

public class Puzzle : MonoBehaviour
{
    // Evento que se dispara cuando el puzzle es resuelto
    public PuzzleSolvedEvent onPuzzleSolved = new PuzzleSolvedEvent();

    // Estado interno que indica si el puzzle ya fue resuelto
    private bool isSolved = false;

    // Método público para marcar el puzzle como resuelto
    public void Solve()
    {
        // Verificar que el puzzle no esté ya resuelto
        if (!isSolved)
        {
            isSolved = true; // Marcar como resuelto
            Debug.Log("Puzzle solved: " + gameObject.name); // Log informativo
            onPuzzleSolved.Invoke(); // Disparar el evento
        }
    }
}