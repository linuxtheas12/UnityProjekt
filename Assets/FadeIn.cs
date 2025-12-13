using System.Collections;

using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 2f; // trvanie fade-out efektu

    private SpriteRenderer spriteRenderer;
    private Color initialColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("FadeOutSquare: SpriteRenderer not found!");
            return;
        }

        // uložíme pôvodnú farbu
        initialColor = spriteRenderer.color;
        // zaèíname úplne vidite¾ní
        spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            // postupne znižujeme alfa
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f - t);

            yield return null;
        }

        // upevníme koneènú alfa hodnotu na 0
        spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }
}