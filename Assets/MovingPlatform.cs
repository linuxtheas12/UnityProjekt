using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour
{
    public Transform BodA;
    public Transform BodB;
    public float moveSpeed = 4f;
    private Vector3 nextPosition;
    private Vector3 posA;
    private Vector3 posB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posA = BodA.position;
        posB = BodB.position;
        nextPosition = BodB.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards (transform.position, nextPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, nextPosition) < 0.01f)
            {
                  nextPosition = (nextPosition == posA) ? posB : posA;
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(UnparentPlayer(collision.transform));
        }
    }

    IEnumerator UnparentPlayer(Transform player) {
    yield return null; // poèká 1 frame
    if (player != null) {
        player.SetParent(null);
    }
}
}
