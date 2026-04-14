using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Tieto triedy definujú štruktúru JSONu
[System.Serializable]
public class QuestionData
{
    public string theme;
    public string npcName;
    public string evaluatorName;
    public string questionText;
    public string[] options;
    public int correctAnswerIndex;
    public string correctResponse;
    public string wrongResponseHint;
}

[System.Serializable]
public class QuestionDatabase
{
    public List<QuestionData> questions;
}

public class QuestionManager : MonoBehaviour
{
    private static QuestionManager instance;

    [Header("JSON Databáza")]
    public TextAsset jsonFile;

    private QuestionDatabase database;

    // Pomocná premenná na sledovanie poslednej otázky (aby sa neopakovala)
    private int lastQuestionIndex = -1;

    private void Awake()
    {
        if (instance != null) Debug.LogWarning("Našlo sa viacero Question Managerov!");
        instance = this;
        LoadDatabase();
    }

    public static QuestionManager GetInstance() { return instance; }

    private void LoadDatabase()
    {
        if (jsonFile != null)
        {
            database = JsonUtility.FromJson<QuestionDatabase>(jsonFile.text);
        }
        else
        {
            Debug.LogError("JSON súbor s otázkami nie je priradený v QuestionManager!");
        }
    }

    public QuestionData GetRandomQuestionByTheme(string theme)
    {
        if (database == null || database.questions == null) return null;

        // 1. Získame otázky pre danú tému aj s ich pôvodnými indexmi v databáze
        // Toto robíme preto, aby sme vedeli presne identifikovať konkrétnu otázku
        var filteredWithIndices = database.questions
            .Select((q, index) => new { Question = q, Index = index })
            .Where(x => x.Question.theme == theme)
            .ToList();

        if (filteredWithIndices.Count == 0)
        {
            Debug.LogWarning($"Pre tému '{theme}' sa nenašli žiadne otázky!");
            return null;
        }

        int selectedEntryIndex = 0;

        // 2. Ak máme v téme viac ako jednu otázku, žrebujeme tak dlho, kým nedostaneme inú ako naposledy
        if (filteredWithIndices.Count > 1)
        {
            int safetyBreak = 0; // Poistka proti nekonečnému cyklu
            do
            {
                selectedEntryIndex = Random.Range(0, filteredWithIndices.Count);
                safetyBreak++;
            }
            while (filteredWithIndices[selectedEntryIndex].Index == lastQuestionIndex && safetyBreak < 100);
        }
        else
        {
            // Ak je v téme len jedna otázka, nemáme na výber a zopakujeme ju
            selectedEntryIndex = 0;
        }

        // 3. Uložíme si index vybranej otázky pre budúcu kontrolu
        lastQuestionIndex = filteredWithIndices[selectedEntryIndex].Index;

        // 4. Vrátime samotné dáta otázky
        return filteredWithIndices[selectedEntryIndex].Question;
    }
}