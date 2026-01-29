using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] private AudioSource footstepSource; // Sem vlož AudioSource s loopnutým zvukom chôdze

    void Update()
    {

        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            StopMovement();
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        // 1. Animácia a Zvuk chôdze
        if (horizontal != 0 && IsGrounded())
        {
            anim.SetBool("isRunning", true);

            // Spusti zvuk, ak ešte nehrá
            if (!footstepSource.isPlaying)
            {
                footstepSource.Play();
            }
        }
        else
        {
            anim.SetBool("isRunning", false);

            // Zastav zvuk, ak hráè stojí alebo je vo vzduchu
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }

        // 2. Logika skoku
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);

            // Tip: Tu môžeš prida samostatný jednorazový zvuk skoku
            // footstepSource.Stop(); // Volite¾né: okamžite utíš kroky pri výskoku
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
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
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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