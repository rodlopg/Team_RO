using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


//Script that handles the name and input of the player
public class NameScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextMeshPro;
    private string name;
    private void OnLevelWasLoaded(int level)
    {
        Debug.Log(name);
    }
    //Method to be called in the OnChange Event of the input field
    //It uses a textmeshpro to display the current name
    public void SetName(string name)
    {
        this.name = name;
        if(m_TextMeshPro != null) m_TextMeshPro.text = name;
    }

    //Method to handle the confirmation of the name. Normally I would think this closes the naming screen, but I have no context of how that is going to be in the final game. I will change it later on.
    //Right now, has temporal behavior for testing
    public void ConfirmName()
    {
        SceneManager.LoadScene(0);
    }
}
