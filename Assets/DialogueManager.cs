using System.Collections;
using System.Collections.Generic;

using Ink.Runtime;
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

    private bool canContinueToNextLine = false;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private List<Choice> currentChoices;

    public bool dialogueIsPlaying { get; private set; }

    [Header("Level Progress")]
    public bool canGoToNextLevel = false;

    private int currentChoiceIndex = 0;
    private bool canMakeChoice = true;
    private bool canMoveChoice = true;
    private bool inputReleased = false;

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

        // Ošetrenie vstupu, aby dialóg nepreskočil hneď pri zapnutí
        if (!inputReleased)
        {
            if (!Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Return))
                inputReleased = true;
            else
                return;
        }

        // Pokračovanie v texte (ak nie sú na výber možnosti)
        if (canContinueToNextLine && (currentChoices == null || currentChoices.Count == 0) && Input.GetKeyDown(KeyCode.E))
        {
            ContinueStory();
            return;
        }

        // Ovládanie výberu možností
        if (currentChoices != null && currentChoices.Count > 0)
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

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string cleanTag = tag.Trim().ToUpper();
            if (cleanTag == "CORRECT_CHOICE")
            {
                canGoToNextLevel = true; // Odomkne dvere (LevelExit.cs)
                if (correctSoundSource != null) correctSoundSource.Play();
            }
            else if (cleanTag == "WRONG_CHOICE")
            {
                canGoToNextLevel = false;
                if (wrongSoundSource != null) wrongSoundSource.Play();
            }
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            HandleTags(currentStory.currentTags);
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator ChoiceMoveCooldown() { canMoveChoice = false; yield return new WaitForSeconds(0.2f); canMoveChoice = true; }
    private IEnumerator ChoiceSelectCooldown() { canMakeChoice = false; yield return new WaitForSeconds(0.2f); canMakeChoice = true; }

    void UpdateChoiceUI()
    {
        for (int i = 0; i < choices.Length; i++)
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
        currentChoiceIndex = Mathf.Clamp(currentChoiceIndex, 0, currentChoices.Count - 1);
        UpdateChoiceUI();
    }

    private void MakeChoice(int index)
    {
        currentStory.ChooseChoiceIndex(index);
        currentChoiceIndex = 0;
        ContinueStory();
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        inputReleased = false;
        canGoToNextLevel = false; // Reset pri každom novom dialógu

        ContinueStory();
        StartCoroutine(EnableNextLine());
    }

    private IEnumerator EnableNextLine() { yield return new WaitForSeconds(0.5f); canContinueToNextLine = true; }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        canContinueToNextLine = false;
        dialogueText.text = "";
        currentChoices = null;
        // TU SME ODSTRÁNILI AUTOMATICKÝ PRECHOD - teraz čakáme na trigger hráča v LevelExit.cs
    }

    private void DisplayChoices()
    {
        currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > choices.Length) Debug.LogError("Príliš veľa možností v Inku!");

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
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