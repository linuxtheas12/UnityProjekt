using UnityEngine;
using UnityEngine.SceneManagement;

public class LockCameraX : MonoBehaviour
{
    private float lockedX;
    private bool isTowerLevel;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "tower")
        {
            isTowerLevel = true;
            // Zamkne X na pozícii, kde začína virtuálna kamera (nastav si ju v scéne na stred veže)
            lockedX = transform.position.x;
        }
    }

    void LateUpdate()
    {
        if (!isTowerLevel) return;

        // Lock X po tom, ako Cinemachine vyrátal novú pozíciu
        Vector3 pos = transform.position;
        pos.x = lockedX;
        transform.position = pos;
    }
}