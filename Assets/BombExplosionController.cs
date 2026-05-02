using UnityEngine;

public class BombExplosionController : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private Transform canvasTransform;

    [Header("Dogs")]
    [SerializeField] private GameObject dogPrefab;
    [SerializeField] private int dogCount = 50;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void Explode()
    {
        //  vizuálny efekt
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        //  zvuk
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        //  zaplnenie obrazovky
        SpawnDogsOnScreen();
    }



    private void SpawnDogsOnScreen()
    {
        if (dogPrefab == null || canvasTransform == null) return;

        RectTransform canvasRect = canvasTransform.GetComponent<RectTransform>();

        for (int i = 0; i < dogCount; i++)
        {
            GameObject dog = Instantiate(dogPrefab, canvasTransform);

            RectTransform rt = dog.GetComponent<RectTransform>();

            float x = Random.Range(0f, canvasRect.rect.width);
            float y = Random.Range(0f, canvasRect.rect.height);

            rt.anchoredPosition = new Vector2(x, y);
        }
    }

    private Vector3 GetRandomScreenPosition()
    {
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);

        Vector3 viewportPos = new Vector3(x, y, 0);
        Vector3 worldPos = mainCamera.ViewportToWorldPoint(viewportPos);

        worldPos.z = 0f;

        return worldPos;
    }
}