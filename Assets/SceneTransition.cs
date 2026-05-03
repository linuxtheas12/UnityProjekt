using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [SerializeField] private RectTransform blackBlock;
    [SerializeField] private float transitionSpeed = 1.5f;

    private float screenWidth;
    private Canvas myCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Nastavenie šírky bloku
            screenWidth = Screen.width + 100;

            myCanvas = GetComponent<Canvas>();
            // Priradíme kameru hneď pri prvom vytvorení
            UpdateCamera();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Táto funkcia sa spustí vždy po načítaní novej scény
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCamera();
    }

    // Samostatná funkcia na hľadanie kamery, ktorú môžeme volať kedykoľvek
    public void UpdateCamera()
    {
        if (myCanvas != null)
        {
            // Skúsime nájsť MainCamera cez Tag
            myCanvas.worldCamera = Camera.main;

            // Ak ju nenašlo cez tag, nájdeme proste prvú kameru v scéne
            if (myCanvas.worldCamera == null)
            {
                myCanvas.worldCamera = GameObject.FindObjectOfType<Camera>();
            }

            // Nastavíme Plane Distance na malú hodnotu, aby bol prechod blízko kamery
            myCanvas.planeDistance = 1;

            Debug.Log("SceneTransition: Kamera priradená -> " + (myCanvas.worldCamera != null ? myCanvas.worldCamera.name : "NENÁJDENÁ"));
        }
    }

    private void Start()
    {
        StartCoroutine(RevealScene());
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ExitScene(sceneName));
    }

    private IEnumerator RevealScene()
    {
        float timer = 0;
        Vector2 startPos = Vector2.zero;
        Vector2 endPos = new Vector2(-screenWidth, 0);

        while (timer < 1f)
        {
            timer += Time.deltaTime * transitionSpeed;
            blackBlock.anchoredPosition = Vector2.Lerp(startPos, endPos, timer);
            yield return null;
        }
        blackBlock.anchoredPosition = endPos;
    }

    private IEnumerator ExitScene(string sceneName)
    {
        float timer = 0;
        Vector2 startPos = new Vector2(screenWidth, 0);
        Vector2 endPos = Vector2.zero;

        blackBlock.anchoredPosition = startPos;

        while (timer < 1f)
        {
            timer += Time.deltaTime * transitionSpeed;
            blackBlock.anchoredPosition = Vector2.Lerp(startPos, endPos, timer);
            yield return null;
        }
        blackBlock.anchoredPosition = endPos;

        // Načítanie scény
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Po načítaní scény pre istotu znova skontrolujeme kameru
        UpdateCamera();

        StartCoroutine(RevealScene());
    }
}