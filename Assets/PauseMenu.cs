
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// Riadok s MediaTypeNames som vymazal, ten robil ten error

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("UI Panely")]
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Audio")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;


    void Start()
    {
        // Hneï na zaèiatku hry menu vypneme a èas spustíme
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC stlaèenı! Aktuálny stav isPaused: " + isPaused);
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f; // DÔLEITÉ: Musíme vráti èas na 1, inak menu zamrzne
        isPaused = false;
        SceneManager.LoadScene("Menu"); // Tu napíš presnı názov tvojej scény s menu
    }

    // --- AUDIO LOGIKA ---

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        // Ak je volume 0, nastavíme ve¾mi nízke decibely, aby nebol error v Log10
        if (volume <= 0.0001f) mainMixer.SetFloat("musicVol", -80f);
        else mainMixer.SetFloat("musicVol", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        if (volume <= 0.0001f) mainMixer.SetFloat("sfxVol", -80f);
        else mainMixer.SetFloat("sfxVol", Mathf.Log10(volume) * 20);
    }
}