using System.Collections; // Potrebné pre IEnumerator (delay)
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Nastavenia")]
    [SerializeField] private string nextLevelName;
    [SerializeField] private float delayBeforeExit = 0.5f; // Pol sekundy delay

    [Header("Zvuk")]
    [SerializeField] private AudioSource exitSound; // Sem priradíš AudioSource

    private bool isExiting = false; // Ochrana, aby sa to nespustilo viackrát naraz

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExiting) return;

        if (collision.CompareTag("Player"))
        {
            if (DialogueManager.GetInstance().canGoToNextLevel)
            {
                GameSession.aktualnyStage++; // Zvıšime stage
                StartCoroutine(ExecuteExit());
            }
            else
            {
                Debug.Log("NPC a ešte nepustilo.");
                // Ak chceš zvuk aj pri zamknutıch dverách, môeš ho prida sem
            }
        }
    }

    private IEnumerator ExecuteExit()
    {
        isExiting = true;
        Debug.Log("Prístup povolenı. Hrám zvuk...");

        // 1. Spusti zvuk (ak je priradenı)
        if (exitSound != null)
        {
            exitSound.Play();
        }

        // 2. Poèkaj definovanı èas
        yield return new WaitForSeconds(delayBeforeExit);

        // 3. Naèítaj ïalší level
        SceneManager.LoadScene(nextLevelName);
    }
}