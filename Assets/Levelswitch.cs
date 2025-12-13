

using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene loading

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private string levelToLoad; // Name of the scene to load
    [SerializeField] private DialogueManager dialogueManager;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the object entering is the player
        if (collider.gameObject.tag == "Player")
        {
            if (dialogueManager.correctChoiceMade)
            {
                Debug.Log("Player entered trigger! Loading scene: " + levelToLoad);
                SceneManager.LoadScene(levelToLoad);

            }
        }
    }
}
