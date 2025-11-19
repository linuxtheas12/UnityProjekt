using UnityEngine;

public class MovementController : MonoBehaviour
{
    Vector2 move;
    public float speed = 5f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // zbierame input v Update
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        // fyzicky pohyb len tu
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }
}
