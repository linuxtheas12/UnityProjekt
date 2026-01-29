
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON (Fallback)")]
    [Tooltip("Tento súbor sa spustí, ak by náhodou nebola vybratá žiadna téma v menu.")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        // Ak je hráè v dosahu a práve neprebieha iný dialóg
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);

            // Keï hráè stlaèí tlaèidlo pre interakciu (napr. E)
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
        TextAsset suborNaSpustenie = null;

        if (GameSession.vybranaTema != null)
        {
            // Tu priradíme súbor pod¾a aktuálneho èísla stageu (1-9)
            switch (GameSession.aktualnyStage)
            {
                case 1: suborNaSpustenie = GameSession.vybranaTema.stage1_Ink; break;
                case 2: suborNaSpustenie = GameSession.vybranaTema.stage2_Ink; break;
                case 3: suborNaSpustenie = GameSession.vybranaTema.stage3_Ink; break;
                case 4: suborNaSpustenie = GameSession.vybranaTema.stage4_Ink; break;
                case 5: suborNaSpustenie = GameSession.vybranaTema.stage5_Ink; break;
                case 6: suborNaSpustenie = GameSession.vybranaTema.stage6_Ink; break;
                case 7: suborNaSpustenie = GameSession.vybranaTema.stage7_Ink; break;
                case 8: suborNaSpustenie = GameSession.vybranaTema.stage8_Ink; break;
                case 9: suborNaSpustenie = GameSession.vybranaTema.stage9_Ink; break;
                default: suborNaSpustenie = GameSession.vybranaTema.stage1_Ink; break;
            }
        }

        // Ak by náhodou v téme nebol súbor, použi ten, èo je hodený priamo na strome (fallback)
        if (suborNaSpustenie == null)
        {
            suborNaSpustenie = inkJSON;
        }

        if (suborNaSpustenie != null)
        {
            DialogueManager.GetInstance().EnterDialogueMode(suborNaSpustenie);
        }
        else
        {
            Debug.LogError("Žiadny Ink súbor nebol nájdený!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}