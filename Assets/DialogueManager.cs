using System.Collections;
using System.Collections.Generic;

using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

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

    private int currentChoiceIndex = 0;

    private bool canMakeChoice = true;
    private bool canMoveChoice = true;

    private bool inputReleased = false; // <<< DOPLNENÉ


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;

        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;

        // ===============================================
        // BLOKOVANIE INPUTU, KÝM HRÁČ NEPUSTÍ E/ENTER
        // ===============================================
        if (!inputReleased)
        {
            if (!Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Return))
                inputReleased = true;
            else
                return;
        }
        // ===============================================


        // === 1) Continue story (len ak nie sú možnosti)
        if (canContinueToNextLine &&
            (currentChoices == null || currentChoices.Count == 0) &&
            Input.GetKeyDown(KeyCode.E))
        {
            ContinueStory();
            return;
        }

        // === 2) Pohyb v menu
        if (currentChoices != null && currentChoices.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && canMoveChoice)
            {
                StartCoroutine(ChoiceMoveCooldown());
                ChangeChoice(1);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && canMoveChoice)
            {
                StartCoroutine(ChoiceMoveCooldown());
                ChangeChoice(-1);
            }

            // === 3) Výber možnosti
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && canMakeChoice)
            {
                StartCoroutine(ChoiceSelectCooldown());
                MakeChoice(currentChoiceIndex);
            }
        }
    }


    // === COOLDOWNY ===

    private IEnumerator ChoiceMoveCooldown()
    {
        canMoveChoice = false;
        yield return new WaitForSeconds(0.2f);
        canMoveChoice = true;
    }

    private IEnumerator ChoiceSelectCooldown()
    {
        canMakeChoice = false;
        yield return new WaitForSeconds(0.2f);
        canMakeChoice = true;
    }


    // === UI ============

    void UpdateChoiceUI()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i].color = (i == currentChoiceIndex) ? Color.yellow : Color.white;
        }
    }

    void ChangeChoice(int dir)
    {
        currentChoiceIndex += dir;
        currentChoiceIndex = Mathf.Clamp(currentChoiceIndex, 0, currentChoices.Count - 1);
        UpdateChoiceUI();
    }

    public void MakeChoice(int index)
    {
        currentStory.ChooseChoiceIndex(index);
        currentChoiceIndex = 0;
        ContinueStory();
    }


    // === MODE =========

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        inputReleased = false; // <<< BLOKUJ INPUT PO SPUSTENÍ

        ContinueStory();
        StartCoroutine(EnableNextLine());
    }

    private IEnumerator EnableNextLine()
    {
        yield return new WaitForSeconds(0.5f);
        canContinueToNextLine = true;
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        canContinueToNextLine = false;
        dialogueText.text = "";

        currentChoices = null;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // Get the next line of text
            string text = currentStory.Continue();

            // Store all tags for this line
            List<string> tags = currentStory.currentTags;

            // Display the text in the UI
            dialogueText.text = text;

            // Log the text
            Debug.Log("Line text: " + text);

            // Log all tags
            foreach (string tag in tags)
            {
                Debug.Log("Tag: " + tag);
            }

            // Show choices if any
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }





    /*
        private void ContinueStory()
        {
            if (currentStory.canContinue)
            {
                dialogueText.text = currentStory.Continue();
                DisplayChoices();
            }
            else
            {
                StartCoroutine(ExitDialogueMode());
            }
        }

        */
    // === CHOICES =========

    private void DisplayChoices()
    {
        currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("Je viac možností ako máme buttonov!");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }

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