using UnityEngine;
using UnityEngine.SceneManagement; // Potrebné pre zistenie názvu scény
using System.Collections; // Potrebné pre Coroutinu

public class BombExplosionController : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;

    [Header("Dogs Settings")]
    [SerializeField] private GameObject dogPrefab;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private int dogCount = 150;
    [SerializeField] private float delayBeforeSceneChange = 2.5f; // Koľko sekúnd budú psy lietať, kým začne čierny prechod

    private Camera mainCamera;
    private bool exploded = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void Explode()
    {
        if (exploded) return;
        exploded = true;

        // 1. Zvuk a vizuálny efekt výbuchu bomby
        if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);
        if (audioSource != null && explosionSound != null) audioSource.PlayOneShot(explosionSound);

        // 2. OKAMŽITÝ ŠTART: Psy vyletia z bomby
        SpawnDogsFromBomb();

        // 3. OKAMŽITÝ ŠTART: Čierny prechod sa začne hýbať
        if (SceneTransition.Instance != null)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneTransition.Instance.ChangeScene(currentSceneName);
        }
    }

    private IEnumerator WaitAndRestart()
    {
        // Počkáme, kým sa psy trocha rozletia
        yield return new WaitForSeconds(delayBeforeSceneChange);

        // 4. Zavoláme tvoj SceneTransition
        if (SceneTransition.Instance != null)
        {
            // Načíta znova tú istú scénu (začiatok levelu)
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneTransition.Instance.ChangeScene(currentSceneName);
        }
        else
        {
            // Ak by si náhodou nemal SceneTransition v scéne, reštartne to hneď
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void SpawnDogsFromBomb()
    {
        if (dogPrefab == null || canvasRect == null) return;

        Vector2 screenPoint = mainCamera.WorldToScreenPoint(transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, mainCamera, out Vector2 localPoint);

        for (int i = 0; i < dogCount; i++)
        {
            GameObject dog = Instantiate(dogPrefab, canvasRect);
            RectTransform dogRT = dog.GetComponent<RectTransform>();

            dogRT.localPosition = new Vector3(dogRT.localPosition.x, dogRT.localPosition.y, 0);
            dogRT.localScale = Vector3.one;
            dogRT.anchoredPosition = localPoint + new Vector2(Random.Range(-40f, 40f), Random.Range(-40f, 40f));

            PuppyRise pr = dog.GetComponent<PuppyRise>();
            if (pr != null)
            {
                pr.SetRiseSpeed(Random.Range(400f, 800f));
            }
        }
    }
}