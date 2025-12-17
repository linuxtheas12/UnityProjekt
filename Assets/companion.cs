using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    public Transform player;        // Hráč
    public float followSpeed = 5f;  // Rýchlosť
    public float followDistance = 1.5f; // Povolená vzdialenosť

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FollowPlayer();
        SetSortingOrder();
    }

    void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > followDistance)
        {
            transform.position = Vector2.Lerp(
                transform.position,
                player.position,
                followSpeed * Time.deltaTime
            );
        }
    }

    void SetSortingOrder()
    {
        // 🚶 Ak je player vyššie → Companion ide dozadu
        if (player.position.y > transform.position.y)
            sr.sortingOrder = -1;
        else
            sr.sortingOrder = 1;
    }
}
