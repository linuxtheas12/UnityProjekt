using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    [Header("Dialogue Sounds")]
    [SerializeField] private AudioSource correctSoundSource;
    [SerializeField] private AudioSource wrongSoundSource;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    public bool dialogueIsPlaying { get; private set; }

    [Header("Level Progress")]
    public bool canGoToNextLevel = false;

    private int currentChoiceIndex = 0;
    private bool canMakeChoice = true;
    private bool canMoveChoice = true;
    private bool inputReleased = false;

    // JSON Dátové premenné
    private QuestionData currentQuestion;
    private enum DialogueState { AskingQuestion, ShowingResult }
    private DialogueState currentState;

    private void Awake()
    {
        if (instance != null) Debug.LogWarning("Found more than one Dialogue Manager");
        instance = this;
    }

    public static DialogueManager GetInstance() { return instance; }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];

        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;

        // Ošetrenie vstupu
        if (!inputReleased)
        {
            if (!Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Return))
                inputReleased = true;
            else
                return;
        }

        // Ak sa zobrazuje výsledok (správna/nesprávna odpoveď) a hráč stlačí E/Enter, ukonči dialóg
        if (currentState == DialogueState.ShowingResult && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)))
        {
            StartCoroutine(ExitDialogueMode());
            return;
        }

        // Ak hráč vyberá z možností
        if (currentState == DialogueState.AskingQuestion)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && canMoveChoice)
            {
                StartCoroutine(ChoiceMoveCooldown());
                ChangeChoice(1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && canMoveChoice)
            {
                StartCoroutine(ChoiceMoveCooldown());
                ChangeChoice(-1);
            }
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && canMakeChoice)
            {
                StartCoroutine(ChoiceSelectCooldown());
                MakeChoice(currentChoiceIndex);
            }
        }
    }

    private IEnumerator ChoiceMoveCooldown() { canMoveChoice = false; yield return new WaitForSeconds(0.2f); canMoveChoice = true; }
    private IEnumerator ChoiceSelectCooldown() { canMakeChoice = false; yield return new WaitForSeconds(0.2f); canMakeChoice = true; }

    void UpdateChoiceUI()
    {
        for (int i = 0; i < currentQuestion.options.Length; i++)
        {
            choicesText[i].color = Color.white;
            UnityEngine.UI.Image btnImage = choices[i].GetComponent<UnityEngine.UI.Image>();
            if (btnImage != null)
            {
                btnImage.color = (i == currentChoiceIndex) ? new Color(0f, 0f, 0f, 0.78f) : new Color(0f, 0f, 0f, 0f);
            }
        }
    }

    void ChangeChoice(int dir)
    {
        currentChoiceIndex += dir;
        currentChoiceIndex = Mathf.Clamp(currentChoiceIndex, 0, currentQuestion.options.Length - 1);
        UpdateChoiceUI();
    }

    private void MakeChoice(int index)
    {
        foreach (var choiceBtn in choices) choiceBtn.SetActive(false);

        // Získame meno, ktoré je momentálne zobrazené v dialogueText (pred dvojbodkou)
        // Alebo jednoduchšie: premennú npcOverrideName si ulož ako private v DialogueManageri

        if (index == currentQuestion.correctAnswerIndex)
        {
            // Namiesto currentQuestion.evaluatorName môžeme použiť tón Barbas (ak je to vždy on) 
            // alebo meno NPC, ktoré sme práve použili.
            dialogueText.text = $"Barbas: {currentQuestion.correctResponse}";
            canGoToNextLevel = true;
            if (correctSoundSource != null) correctSoundSource.Play();
        }
        else
        {
            dialogueText.text = $"Barbas: {currentQuestion.wrongResponseHint}";
            canGoToNextLevel = false;
            if (wrongSoundSource != null) wrongSoundSource.Play();
        }

        currentState = DialogueState.ShowingResult;
    }

    public void EnterDialogueMode(QuestionData questionData, string npcOverrideName)
    {
        currentQuestion = questionData;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        inputReleased = false;
        canGoToNextLevel = false;
        currentState = DialogueState.AskingQuestion;

        // Kontrola, či je meno zadané (aby sme sa vyhli prázdnej dvojbodke ":")
        if (!string.IsNullOrEmpty(npcOverrideName))
        {
            dialogueText.text = $"{npcOverrideName}: {currentQuestion.questionText}";
        }
        else
        {
            dialogueText.text = currentQuestion.questionText;
        }

        DisplayChoices();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        currentQuestion = null;
    }

    private void DisplayChoices()
    {
        if (currentQuestion.options.Length > choices.Length) Debug.LogError("Otázka má viac možností ako je pripravených UI tlačidiel!");

        int index = 0;
        foreach (string option in currentQuestion.options)
        {
            choices[index].SetActive(true);
            choicesText[index].text = option;
            index++;
        }
        for (int i = index; i < choices.Length; i++) choices[i].SetActive(false);

        currentChoiceIndex = 0;
        UpdateChoiceUI();
        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }
}