using UnityEngine;
using UnityEngine.SceneManagement;

public class jakurwaneviem : MonoBehaviour
{
    private float startPos, length;
    private float startPosY; // Pridané pre vertikálny štart
    public GameObject cam;
    public float parallaxEffect;
    private bool isTowerLevel;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main != null ? Camera.main.gameObject : GameObject.FindGameObjectWithTag("MainCamera");
        }

        if (cam == null) return;

        startPos = transform.position.x;
        startPosY = transform.position.y; // Uložíme si pôvodnú výšku

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            length = renderer.bounds.size.x;
        }

        if (SceneManager.GetActiveScene().name == "tower")
        {
            isTowerLevel = true;
        }
    }

    void FixedUpdate()
    {
        if (cam == null) return;

        if (isTowerLevel)
        {
            // VERTIKÁLNY PARALLAX (pre Tower)
            // Pozadie sa hýbe podľa Y pozície kamery
            float distanceY = cam.transform.position.y * parallaxEffect;
            transform.position = new Vector3(transform.position.x, startPosY + distanceY, transform.position.z);
        }
        else
        {
            // HORIZONTÁLNY PARALLAX (klasický level)
            float distance = cam.transform.position.x * parallaxEffect;
            float movement = cam.transform.position.x * (1 - parallaxEffect);

            transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

            if (movement > startPos + length) startPos += length;
            else if (movement < startPos - length) startPos -= length;
        }
    }
}