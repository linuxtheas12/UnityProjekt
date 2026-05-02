using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Nastavenia")]
    [SerializeField] private float delayBeforeSceneLoad = 0.5f;

    [Header("Zvuk")]
    [SerializeField] private AudioSource exitSound;

    private bool isExiting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExiting) return;

        if (collision.CompareTag("Player"))
        {
            // Kontrola cez DialogueManager, či hráč splnil podmienky (počet správnych odpovedí)
            if (DialogueManager.GetInstance().CheckIfLevelIsComplete())
            {
                StartCoroutine(ExecuteExit());
            }
            else
            {
                Debug.Log("Ešte si nesplnil úlohu u NPC (neodpovedal si správne dostatok krát)!");
                // Tu môžeš pridať napr. UI text: "Musíš najprv presvedčiť NPC!"
            }
        }
    }

    private IEnumerator ExecuteExit()
    {
        isExiting = true;

        if (exitSound != null)
        {
            exitSound.Play();
        }

        GameSession.aktualnyStage++;
        string nextScene = CalculateNextSceneName(GameSession.aktualnyStage);

        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.ChangeScene(nextScene);
        }
        else
        {
            yield return new WaitForSeconds(delayBeforeSceneLoad);
            SceneManager.LoadScene(nextScene);
        }
    }

    private string CalculateNextSceneName(int stage)
    {
        // Ponechal som tvoju logiku vetvenia levelov
        if (stage <= 3) return "LES_" + stage;
        else if (stage <= 6) return "PUST_" + (stage - 3);
        else if (stage <= 9) return "MESTO_" + (stage - 6);
        else return "TOWER_" + (stage - 9);
    }
}