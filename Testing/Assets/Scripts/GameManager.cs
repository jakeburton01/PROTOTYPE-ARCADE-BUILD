using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false; // Game has not ended

    public float restartDelay = 1f; // Seconds before game restarts

 public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true; // Game End Active
            Invoke("Restart", restartDelay); // 1 second delay before Scene restarts
        }

        
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the Scene
    }
}
