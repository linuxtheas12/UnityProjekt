using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    private enum CompletionAction
    {
        UnlockLevelExit,
        LoadSceneImmediately
    }

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Fallback Theme")]
    [SerializeField] private string fallbackTheme = "people and life";

    [Header("NPC Display Settings")]
    [SerializeField] private string npcNameForUI;

    [Header("Quest/Quiz Settings")]
    [SerializeField] private int cielovyPocetSpravnychOdpovedi = 3;

    [SerializeField] private bool vyzadovatZaSebou = false;

    [Header("Level Completion")]
    [SerializeField] private CompletionAction completionAction = CompletionAction.UnlockLevelExit;
    [SerializeField] private string sceneToLoadOnComplete = "Menu";

    [Header("Punishment Settings")]
    [SerializeField] private int maxPocetChyb = 1;

    [SerializeField] private bool resetProgressAfterPunishment = true;

    [SerializeField] private UnityEvent onPunishmentTriggered;


    //  AUDIO
    [Header("Wrong Answer Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip wrongAnswerSound;

    //  PUPPIES
    [SerializeField] private GameObject puppyPrefab;
    [SerializeField] private int pocetSteniatok = 10;
    [SerializeField] private float spawnWidth = 8f;
    [SerializeField] private float spawnY = -5f;

    private int aktualnyPocetSpravnychOdpovedi = 0;
    private int aktualnyPocetChyb = 0;
    private bool ulohaSplnena = false;
    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        ulohaSplnena = false;
        aktualnyPocetSpravnychOdpovedi = 0;
        aktualnyPocetChyb = 0;
        if (visualCue != null) visualCue.SetActive(false);
    }

    private void Start()
    {
        if (DialogueManager.GetInstance() != null)
        {
            DialogueManager.GetInstance().SetLevelComplete(false);
        }
    }

    private void Update()
    {
        if (ulohaSplnena)
        {
            if (visualCue != null) visualCue.SetActive(false);
            return;
        }

        if (DialogueManager.GetInstance() == null || InputManager.GetInstance() == null) return;

        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            if (visualCue != null) visualCue.SetActive(true);

            if (InputManager.GetInstance().GetInteractPressed())
            {
                SpustiVybratyDialog();
            }
        }
        else
        {
            if (visualCue != null) visualCue.SetActive(false);
        }
    }

    private void SpustiVybratyDialog()
    {
        string temaNaSpustenie = (GameSession.vybranaTema != null)
            ? GameSession.vybranaTema.nazovTemy
            : fallbackTheme;

        if (QuestionManager.GetInstance() == null) return;

        QuestionData vybranaOtazka =
            QuestionManager.GetInstance().GetRandomQuestionByTheme(temaNaSpustenie);

        if (vybranaOtazka != null)
        {
            DialogueManager.GetInstance().EnterDialogueMode(vybranaOtazka, npcNameForUI, this);
        }
    }

    public void SpracujVysledokOdpovede(bool odpovedBolaSpravna)
    {
        if (ulohaSplnena) return;

        if (odpovedBolaSpravna)
        {
            aktualnyPocetSpravnychOdpovedi++;
            Debug.Log($"{npcNameForUI}: Správne! ({aktualnyPocetSpravnychOdpovedi}/{cielovyPocetSpravnychOdpovedi})");

            if (aktualnyPocetSpravnychOdpovedi >= cielovyPocetSpravnychOdpovedi)
            {
                UlohaDokoncena();
            }
        }
        else
        {
            //  Zvuk
            if (audioSource != null && wrongAnswerSound != null)
            {
                audioSource.PlayOneShot(wrongAnswerSound);
            }

            //  Šteniatka
            SpawnPuppies();

            if (vyzadovatZaSebou)
            {
                aktualnyPocetSpravnychOdpovedi = 0;
                Debug.Log($"{npcNameForUI}: Chyba! Séria prerušená.");
            }

            aktualnyPocetChyb++;

            if (aktualnyPocetChyb >= maxPocetChyb)
            {
                AplikujTrest();
            }
        }
    }

    private void SpawnPuppies()
    {
        if (puppyPrefab == null) return;

        for (int i = 0; i < pocetSteniatok; i++)
        {
            float randomX = Random.Range(-spawnWidth, spawnWidth);
            Vector3 spawnPos = new Vector3(randomX, spawnY, 0);

            GameObject obj = Instantiate(puppyPrefab, spawnPos, Quaternion.identity);

            // náhodná veľkosť
            float scale = Random.Range(0.5f, 1.5f);
            obj.transform.localScale = new Vector3(scale, scale, 1f);

       
        }
    }

    private void AplikujTrest()
    {
        onPunishmentTriggered?.Invoke();

        if (resetProgressAfterPunishment)
        {
            aktualnyPocetSpravnychOdpovedi = 0;
        }

        aktualnyPocetChyb = 0;
    }

    private void UlohaDokoncena()
    {
        ulohaSplnena = true;
        DialogueManager.GetInstance().SetLevelComplete(true);
        Debug.Log($"NPC {npcNameForUI} spokojný. Cesta voľná!");

        if (completionAction == CompletionAction.LoadSceneImmediately)
        {
            LoadCompletedScene();
        }
    }

    private void LoadCompletedScene()
    {
        if (string.IsNullOrWhiteSpace(sceneToLoadOnComplete))
        {
            Debug.LogWarning($"{name}: Nie je nastavena scena po dokonceni ulohy.");
            return;
        }

        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.ChangeScene(sceneToLoadOnComplete);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoadOnComplete);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
            playerInRange = false;
    }
}
