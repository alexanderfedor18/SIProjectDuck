using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// primarily made through the use of youtube user Broken Knights Games tutorial
/// https://www.youtube.com/watch?v=_5pOiYHJgl0
/// </summary>

public class PlayerConfigurationManager : MonoBehaviour
{

    private List<PlayerConfig> playerConfigs;
    [SerializeField]
    private int maxPlayers = 2;
    [SerializeField]
    private string readyScene;

    //using singleton pattern to create a single instance of this class that is accessible by any gameObject
    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instance of singleton!");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfig>();
        }
    }

    
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        //uses linq expression in order to check all elements of an array with one line of code
        if (playerConfigs.Count == maxPlayers && playerConfigs.All(t => t.IsReady == true))
        {
            SceneManager.LoadScene(readyScene);
        }
    }

    public void OnPlayerJoin(PlayerInput pi)
    {
        if (!playerConfigs.Any(t => t.PlayerIndex == pi.playerIndex))
        { 
            Debug.Log("Player Joined: " + pi.playerIndex);
            //sets parent of prefab created to this object
            pi.transform.SetParent(transform);
            //creates and adds new player config to playerConfigs array
            playerConfigs.Add(new PlayerConfig(pi));
        }
    }

    public List<PlayerConfig> GetPlayerConfigs()
    {
        return playerConfigs;
    }


}



public class PlayerConfig
{
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }


    //constructor
    public PlayerConfig(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

}