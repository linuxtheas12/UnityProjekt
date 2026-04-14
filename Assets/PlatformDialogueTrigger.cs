using UnityEngine;

public class PlatformDialogueTrigger : MonoBehaviour
{
    [Header("Otázka")]
    [SerializeField] private string tema; // ← napíšeš tému napr. "idioms"
    [SerializeField] private string npcName = "Barbas";

    private bool alreadyTriggered = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !alreadyTriggered)
        {
            if (!DialogueManager.GetInstance().dialogueIsPlaying)
            {
                alreadyTriggered = true;
                SpustiDialog();
            }
        }
    }

    private void SpustiDialog()
    {
        QuestionData otazka = QuestionManager.GetInstance().GetRandomQuestionByTheme(tema);

        if (otazka != null)
        {
            DialogueManager.GetInstance().EnterDialogueMode(otazka, npcName);
        }
        else
        {
            Debug.LogError($"Žiadna otázka pre tému: {tema}");
        }
    }
}