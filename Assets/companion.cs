using System;
using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    [Header("Nastavenia pohybu")]
    public Transform player;
    public float followSpeed = 8f;
    public float followDistance = 0.5f;

    [Header("Zvuky")]
    [SerializeField] private AudioSource barkSource;
    [SerializeField] private float minBarkDelay = 5f;
    [SerializeField] private float maxBarkDelay = 15f;
    private float nextBarkTime;

    [Header("Referencie")]
    private SpriteRenderer sr;
    private Animator anim;
    private Vector3 lastPosition;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        lastPosition = transform.position;

        nextBarkTime = Time.time + UnityEngine.Random.Range(minBarkDelay, maxBarkDelay);

        // Ak si nepriradil AudioSource v Inspectore, skúsime ho nájsť automaticky
        if (barkSource == null) barkSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 🛑 AK BEŽÍ DIALÓG, WOLFIE STOJÍ
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            anim.SetBool("PESBEZI", false);
            return;
        }

        if (Time.time >= nextBarkTime)
        {
            Bark();
        }

        FollowPlayer();
        HandleFlip();
        HandleAnimation();
        SetSortingOrder();

        lastPosition = transform.position;
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

    void HandleAnimation()
    {
        // Kontrola, či sa reálne pohol od minulého snímku
        bool isMoving = Vector3.Distance(transform.position, lastPosition) > 0.005f;
        anim.SetBool("PESBEZI", isMoving);
    }

    void HandleFlip()
    {
        // Wolfie sa vždy díva na hráča
        if (player.position.x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    void SetSortingOrder()
    {
        // 🚶 Ak je player vyššie (na y osi) → Companion ide dozadu
        if (player.position.y > transform.position.y)
            sr.sortingOrder = -1;
        else
            sr.sortingOrder = 1;
    }
    void Bark()
    {
        if (barkSource != null && !barkSource.isPlaying)
        {
            barkSource.Play();
        }
        // Naplánujeme ďalšie brechnutie
        // Namiesto: nextBarkTime = Time.time + Random.Range(minBarkDelay, maxBarkDelay);
        // Napíš toto:
        nextBarkTime = Time.time + UnityEngine.Random.Range(minBarkDelay, maxBarkDelay);
    }
}