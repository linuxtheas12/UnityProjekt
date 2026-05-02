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
    // Táto premenná teraz slúži ako globálny indikátor pre LevelExit
    private bool levelComplete = false;

    private int currentChoiceIndex = 0;
    private bool canMakeChoice = true;
    private bool canMoveChoice = true;
    private bool inputReleased = false;

    // Referencia na trigger, ktorý otvoril aktuálny dialóg
    private DialogueTrigger currentActiveTrigger;

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

        if (!inputReleased)
        {
            if (!Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Return))
                inputReleased = true;
            else
                return;
        }

        if (currentState == DialogueState.ShowingResult && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)))
        {
            StartCoroutine(ExitDialogueMode());
            return;
        }

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

    // --- LOGIKA PRE LEVEL EXIT ---
    public void SetLevelComplete(bool status) { levelComplete = status; }
    public bool CheckIfLevelIsComplete() { return levelComplete; }

    public void EnterDialogueMode(QuestionData questionData, string npcOverrideName, DialogueTrigger triggerSource)
    {
        currentQuestion = questionData;
        currentActiveTrigger = triggerSource;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        inputReleased = false;
        currentState = DialogueState.AskingQuestion;

        // VYMAŽ ALEBO ZAKOMENTUJ TENTO RIADOK:
        // canGoToNextLevel = false;  <-- Toto spôsobovalo problém

        if (!string.IsNullOrEmpty(npcOverrideName))
            dialogueText.text = $"{npcOverrideName}: {currentQuestion.questionText}";
        else
            dialogueText.text = currentQuestion.questionText;

        DisplayChoices();
    }

    // Pre spätnú kompatibilitu, ak by si niekde volal starú verziu bez triggeru
    public void EnterDialogueMode(QuestionData questionData, string npcOverrideName)
    {
        EnterDialogueMode(questionData, npcOverrideName, null);
    }

    private void MakeChoice(int index)
    {
        foreach (var choiceBtn in choices) choiceBtn.SetActive(false);

        bool isCorrect = (index == currentQuestion.correctAnswerIndex);

        if (isCorrect)
        {
            dialogueText.text = $"Barbas: {currentQuestion.correctResponse}";
            if (correctSoundSource != null) correctSoundSource.Play();
        }
        else
        {
            dialogueText.text = $"Barbas: {currentQuestion.wrongResponseHint}";
            if (wrongSoundSource != null) wrongSoundSource.Play();
        }

        // ODOVZDANIE VÝSLEDKU DO TRIGGERU (TU SA POČÍTAJÚ BODY A TRESTY)
        if (currentActiveTrigger != null)
        {
            currentActiveTrigger.SpracujVysledokOdpovede(isCorrect);
        }

        currentState = DialogueState.ShowingResult;
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        currentQuestion = null;
        currentActiveTrigger = null; // Vyčistíme referenciu
        inputReleased = false;
        canMakeChoice = true;
        canMoveChoice = true;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    // --- ZVYŠOK UI LOGIKY ---
    void UpdateChoiceUI()
    {
        for (int i = 0; i < currentQuestion.options.Length; i++)
        {
            choicesText[i].color = Color.white;
            UnityEngine.UI.Image btnImage = choices[i].GetComponent<UnityEngine.UI.Image>();
            if (btnImage != null)
                btnImage.color = (i == currentChoiceIndex) ? new Color(0f, 0f, 0f, 0.78f) : new Color(0f, 0f, 0f, 0f);
        }
    }

    void ChangeChoice(int dir)
    {
        currentChoiceIndex += dir;
        currentChoiceIndex = Mathf.Clamp(currentChoiceIndex, 0, currentQuestion.options.Length - 1);
        UpdateChoiceUI();
    }

    private void DisplayChoices()
    {
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
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(choices[0]);
        }
    }
}