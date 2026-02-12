using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [SerializeField] private RectTransform blackBlock; // Sem vlož ten èierny Image
    [SerializeField] private float transitionSpeed = 1.5f;

    private float screenWidth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prechod prežije zmenu scény
            screenWidth = Screen.width + 100; // Malá rezerva
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Hneï pri štarte odhalíme svet (Zprava do¾ava)
        StartCoroutine(RevealScene());
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ExitScene(sceneName));
    }

    // Odhalenie sveta (èierny blok odchádza z 0 na -screenWidth)
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

    // Zakrytie sveta (èierny blok prichádza z screenWidth na 0)
    private IEnumerator ExitScene(string sceneName)
    {
        float timer = 0;
        Vector2 startPos = new Vector2(screenWidth, 0);
        Vector2 endPos = Vector2.zero;

        // Nastavíme poèiatoènú pozíciu pred zaèiatkom pohybu
        blackBlock.anchoredPosition = startPos;

        while (timer < 1f)
        {
            timer += Time.deltaTime * transitionSpeed;
            blackBlock.anchoredPosition = Vector2.Lerp(startPos, endPos, timer);
            yield return null;
        }
        blackBlock.anchoredPosition = endPos;

        // Naèítanie scény a následné odhalenie v novej scéne
        yield return SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(RevealScene());
    }
}