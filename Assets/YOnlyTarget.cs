using UnityEngine;

public class YOnlyTarget : MonoBehaviour
{
    [Tooltip("Player, ktorého Y sledujeme")]
    public Transform player;

    private float lockedX;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // Zamkneme X na aktuálnej pozícii (alebo si ho môžeš nastaviť ručne v Inspectore)
        lockedX = transform.position.x;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Kamera sleduje iba Y hráča, X zostane zamknutý
        transform.position = new Vector3(
            lockedX,
            player.position.y,      // tu môžeš pridať offset ak chceš, napr. player.position.y + 2f
            transform.position.z
        );
    }
}