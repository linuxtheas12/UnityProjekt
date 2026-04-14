using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    private static RespawnManager instance;
    public static RespawnManager GetInstance() { return instance; }

    private void Awake()
    {
        instance = this;
    }

    public void ScheduleRespawn(FallingPlatfrom platform, float delay)
    {
        StartCoroutine(RespawnAfterDelay(platform, delay));
    }

    private IEnumerator RespawnAfterDelay(FallingPlatfrom platform, float delay)
    {
        yield return new WaitForSeconds(delay);
        platform.Respawn();
    }
}