using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenager : MonoBehaviour {

    public static GameMenager instance = null;

    public GameObject youWinText;
    public float resetDelay;

    public GameObject youLoseTExt;
   void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public void Win()
    {
        youWinText.SetActive(true);
        Time.timeScale = .5f;
        Invoke("Reset", resetDelay);

    }

     void Reset()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        youLoseTExt.SetActive(true);


        Time.timeScale = .5f;
        Invoke("Reset", resetDelay);



    }
}
