
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TEST LOG: Collider entered the zone! Object: " + other.gameObject.name + " Tag: " + other.gameObject.tag);
    }
}