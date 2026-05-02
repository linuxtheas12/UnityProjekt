using UnityEngine;

public class PuppyRise : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 10f)
        {
            Destroy(gameObject);
        }
    }
}