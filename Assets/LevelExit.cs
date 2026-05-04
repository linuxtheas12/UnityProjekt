using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Nastavenia")]
    [SerializeField] private bool requireLevelComplete = true;
    [SerializeField] private float delayBeforeSceneLoad = 0.5f;
    [SerializeField] private bool useCustomTargetScene = false;
    [SerializeField] private string customTargetScene = "Menu";

    [Header("Zvuk")]
    [SerializeField] private AudioSource exitSound;

    private bool isExiting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExiting) return;

        if (collision.CompareTag("Player"))
        {
            // Kontrola cez DialogueManager, či hráč splnil podmienky (počet správnych odpovedí)
            if (!requireLevelComplete || IsLevelComplete())
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

    private bool IsLevelComplete()
    {
        DialogueManager dialogueManager = DialogueManager.GetInstance();

        if (dialogueManager == null)
        {
            Debug.LogWarning($"{name}: DialogueManager nie je v scene, preto LevelExit nevie skontrolovat ulohu.");
            return false;
        }

        return dialogueManager.CheckIfLevelIsComplete();
    }

    private IEnumerator ExecuteExit()
    {
        isExiting = true;

        if (exitSound != null)
        {
            exitSound.Play();
        }

        GameSession.aktualnyStage++;
        string nextScene = GetTargetSceneName();

        if (string.IsNullOrWhiteSpace(nextScene))
        {
            Debug.LogWarning($"{name}: Nie je nastavena cielova scena pre LevelExit.");
            isExiting = false;
            yield break;
        }

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

    private string GetTargetSceneName()
    {
        if (useCustomTargetScene)
        {
            return customTargetScene;
        }

        return CalculateNextSceneName(GameSession.aktualnyStage);
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
