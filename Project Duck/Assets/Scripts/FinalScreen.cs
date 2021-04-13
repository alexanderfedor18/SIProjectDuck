using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI congrats;

    private void Start()
    {
        congrats.text = "Congratulations to Player " + Scoreboard.finalWinner + " for winning!";
        
    }

    public void returnToMenu()
    {
        var e = GameObject.Find("PlayerConfigManager");
        Destroy(e);
        Scoreboard.p1Score = 0;
        Scoreboard.p2Score = 0;
        SceneManager.LoadScene("MainMenu");

    }

}
