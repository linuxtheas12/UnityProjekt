using UnityEngine;

public class PlatformDialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    
    private bool alreadyTriggered = false; // aby sa nesp˙öùal dookola

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
        TextAsset suborNaSpustenie = null;

        if (GameSession.vybranaTema != null)
        {
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
            Debug.LogError("éiadny Ink s˙bor nebol n·jden˝!");
        }
    }
}