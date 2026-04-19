using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Nastavenia")]
    [SerializeField] private float delayBeforeSceneLoad = 0.5f; // »as na dohranie zvuku a clony

    [Header("Zvuk")]
    [SerializeField] private AudioSource exitSound;

    private bool isExiting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExiting) return;

        if (collision.CompareTag("Player"))
        {
            if (DialogueManager.GetInstance().canGoToNextLevel)
            {
                StartCoroutine(ExecuteExit());
            }
            else
            {
                Debug.Log("Eöte si neodpovedal spr·vne!");
            }
        }
    }

    private IEnumerator ExecuteExit()
    {
        isExiting = true;

        // 1. SpustÌme zvuk hneÔ
        if (exitSound != null)
        {
            exitSound.Play();
        }

        // 2. VypoËÌtame Ôalöiu scÈnu
        GameSession.aktualnyStage++;
        string nextScene = CalculateNextSceneName(GameSession.aktualnyStage);

        // 3. SpustÌme Ëiernu clonu (ak existuje)
        // Ak tvoj SceneTransition.Instance.ChangeScene uû v sebe m· "yield return LoadSceneAsync", 
        // tak v tomto skripte uû nemusÌö Ëakaù.
        // Ale ak chceö maù istotu, ûe zvuk dohr·:

        if (SceneTransition.Instance != null)
        {
            // SpustÌme vizu·lny prechod
            SceneTransition.Instance.ChangeScene(nextScene);
        }
        else
        {
            // Ak nem·ö transition skript, poËk·me na zvuk a potom switch
            yield return new WaitForSeconds(delayBeforeSceneLoad);
            SceneManager.LoadScene(nextScene);
        }
    }

    private string CalculateNextSceneName(int stage)
    {
        if (stage <= 3) return "LES_" + stage;
        else if (stage <= 6) return "PUST_" + (stage - 3);
        else if (stage <= 9) return "MESTO_" + (stage - 6);
        else if (stage >= 9) return "TOWER_" + (stage - 9);
        else return "Menu";
    }
}