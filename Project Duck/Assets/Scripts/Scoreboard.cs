using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public static int p1Score;
    public static int p2Score;

    public TextMeshProUGUI p1s;
    public TextMeshProUGUI p2s;

    public static int finalWinner;


    public void updateScore(int playerNumLost)
    {
        if (playerNumLost == 1)
        {
            p2Score++;
            p2s.text = p2Score.ToString();
        } else
        {
            p1Score++;
            p1s.text = p1Score.ToString();
        }
        if (p1Score == 3)
        {
            finalWinner = 1;
            SceneManager.LoadScene("EndScreen");
            
        } else if (p2Score == 3)
        {
            finalWinner = 2;
            SceneManager.LoadScene("EndScreen");
        } else
        {
            SceneManager.LoadScene("PlayStage");
        }
    }

    private void Update()
    {
        p1s.text = p1Score.ToString();
        p2s.text = p2Score.ToString();
    }

}
