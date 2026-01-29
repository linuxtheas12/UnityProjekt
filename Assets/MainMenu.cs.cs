using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panely")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject layer4;       // Pridan· premenn· pre tvoj extra objekt

    [Header("Nastavenia Dropdownu")]
    [SerializeField] private TMP_Dropdown temaDropdown;
    [SerializeField] private List<LevelData> vsetkyTemy;

    private LevelData vybranaTema;

    void Start()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);

        // ZabezpeËÌme, aby bola Layer 4 pri ötarte skryt· (ak to tak chceö)
        if (layer4 != null) layer4.SetActive(true);

        if (temaDropdown != null)
        {
            PripravDropdown();
        }
    }

    void PripravDropdown()
    {
        temaDropdown.ClearOptions();
        List<string> nazvy = new List<string>();

        foreach (LevelData tema in vsetkyTemy)
        {
            nazvy.Add(tema.nazovTemy);
        }

        temaDropdown.AddOptions(nazvy);
        if (vsetkyTemy.Count > 0) vybranaTema = vsetkyTemy[0];
    }

    public void ZmenaTemy(int index)
    {
        vybranaTema = vsetkyTemy[index];
    }

    public void OpenCredits()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (layer4 != null) layer4.SetActive(false); // Skryje Layer 4 pri otvorenÌ Credits
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        if (layer4 != null) layer4.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        // Ak chceö Layer 4 zapn˙ù sp‰ù pri zavretÌ credits, daj tu true
    }

    public void Play()
    {
        if (vybranaTema != null)
        {
            GameSession.vybranaTema = vybranaTema;
            GameSession.aktualnyStage = 1;
            SceneManager.LoadScene("LES_1");
        }
        else
        {
            Debug.LogError("Nezvolil si ûiadnu tÈmu!");
        }
    }

    public void Quit()
    {
        Debug.Log("VypÌnam hru...");
        UnityEngine.Application.Quit();
    }
}