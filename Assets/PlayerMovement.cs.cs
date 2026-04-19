using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpingPower = 16f;
    private bool isFacingRight = true;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;
    [Header("Zvuky")]
    [SerializeField] private AudioSource footstepSource;

    void Update()
    {
        Debug.Log("isGrounded: " + IsGrounded() + " | dialogue: " + DialogueManager.GetInstance()?.dialogueIsPlaying + " | jump: " + Input.GetButtonDown("Jump"));
        if (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
        {
            StopMovement();
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0 && IsGrounded())
        {
            anim.SetBool("isRunning", true);
            if (!footstepSource.isPlaying)
                footstepSource.Play();
        }
        else
        {
            anim.SetBool("isRunning", false);
            if (footstepSource.isPlaying)
                footstepSource.Stop();
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private void StopMovement()
    {
        horizontal = 0;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool("isRunning", false);
        if (footstepSource.isPlaying) footstepSource.Stop();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.8f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}