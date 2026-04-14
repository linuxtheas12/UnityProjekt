using UnityEngine;
using System.Collections;

public class FallingPlatfrom : MonoBehaviour
{
    public float fallWait = 2f;
    public float destroyWait = 1f;
    public float respawnWait = 3f;

    bool isFalling;
    Rigidbody2D rb;
    Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallWait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(destroyWait);

        gameObject.SetActive(false);

        // Respawn riadi externý manager
        RespawnManager.GetInstance().ScheduleRespawn(this, respawnWait);
    }

public void Respawn()
{
    transform.position = startPosition;
    transform.rotation = Quaternion.identity;
    rb.bodyType = RigidbodyType2D.Dynamic;
    rb.linearVelocity = Vector2.zero;
    rb.angularVelocity = 0f;
    rb.bodyType = RigidbodyType2D.Static;
    isFalling = false;
    gameObject.SetActive(true);
}
}