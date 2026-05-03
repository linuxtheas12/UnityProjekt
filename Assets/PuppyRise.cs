using UnityEngine;
using UnityEngine.UI; // Nezabudni na toto!

public class PuppyRise : MonoBehaviour
{
    public float riseSpeed = 400f;
    private Vector2 explosionVelocity;
    private bool isExploding = true;
    private float explosionDrag = 0.97f;

    private RectTransform rt;
    private Image img; // Zmenené zo SpriteRenderer na Image

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>(); // Hľadáme UI Image

        // Náhodná rotácia a veľkosť
        rt.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        float scale = Random.Range(0.5f, 1.2f);
        rt.localScale = new Vector3(scale, scale, 1f);

        // Náhodná priehľadnosť
        if (img != null)
        {
            Color c = img.color;
            c.a = Random.Range(0.6f, 1f);
            img.color = c;
        }

        // Výbuchová sila
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float power = Random.Range(5f, 15f);
        explosionVelocity = new Vector2(Mathf.Cos(angle) * power, Mathf.Sin(angle) * power);

        Destroy(gameObject, 10f);
    }

    void Update()
    {
        if (isExploding)
        {
            rt.anchoredPosition += explosionVelocity;
            explosionVelocity *= explosionDrag;
            if (explosionVelocity.magnitude < 0.1f) isExploding = false;
        }

        rt.anchoredPosition += Vector2.up * riseSpeed * Time.deltaTime;
    }

    public void SetRiseSpeed(float speed) => riseSpeed = speed;
}