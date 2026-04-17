using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    // Tower level už nebudeme riešiť tu
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // V tower leveli už nič nerobíme – stará sa o to LockCameraX + Cinemachine
        // (ak chceš nechať fallback, môžeš, ale nie je potrebné)

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}