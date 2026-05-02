using UnityEngine;
using UnityEngine.UI;

public class ScreenFillDog : MonoBehaviour
{
    private RectTransform rt;
    private Image img;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        // random rotácia
        rt.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        // random scale
        float scale = Random.Range(0.5f, 1.5f);
        rt.localScale = new Vector3(scale, scale, 1f);

        // random transparentnosť
        if (img != null)
        {
            Color c = img.color;
            c.a = Random.Range(0.7f, 1f);
            img.color = c;
        }
    }
}