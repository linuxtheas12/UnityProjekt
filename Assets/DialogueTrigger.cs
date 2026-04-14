using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Fallback Theme (Ak nie je vybraná z menu)")]
    [SerializeField] private string fallbackTheme = "people and life";

    [Header("NPC Display Settings")]
    [SerializeField] private string npcNameForUI;


    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);

            if (InputManager.GetInstance().GetInteractPressed())
            {
                SpustiVybratyDialog();
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void SpustiVybratyDialog()
    {
        string temaNaSpustenie = fallbackTheme;

        if (GameSession.vybranaTema != null)
        {
            // OPRAVA: Premenná sa volá nazovTemy, nie temaID
            temaNaSpustenie = GameSession.vybranaTema.nazovTemy;
        }

        // Získanie náhodnej otázky z JSONu podľa témy
        QuestionData vybranaOtazka = QuestionManager.GetInstance().GetRandomQuestionByTheme(temaNaSpustenie);

        if (vybranaOtazka != null)
        {
            // Spustenie dialógu s menom NPC, ktoré si nastavíš v Inšpektore
            DialogueManager.GetInstance().EnterDialogueMode(vybranaOtazka, npcNameForUI);
        }
        else
        {
            Debug.LogError($"Žiadna otázka pre tému '{temaNaSpustenie}' v JSON-e neexistuje! Skontroluj, či sa názov v LevelData zhoduje s názvom v JSON súbore.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}