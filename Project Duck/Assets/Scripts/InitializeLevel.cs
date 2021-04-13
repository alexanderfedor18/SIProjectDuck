using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeLevel : MonoBehaviour
{

    [SerializeField]
    private Transform[] playerSpawns;
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(player1, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<Player>().InitializePlayer(playerConfigs[i], i+1);
        }


    }

}
