using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI controllerText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button readyButton;
    [SerializeField]
    private Image bottomFace;
    [SerializeField]
    private Image enterButton;
    


    private string scheme;

    //ignores any inputs from user for 1.5 seconds
    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndexAndScheme(int pi, string controller)
    {
        scheme = controller;
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;

        if (controller.Equals("gamepad", System.StringComparison.InvariantCultureIgnoreCase))
        {
            controllerText.SetText("Gamepad");
            enterButton.gameObject.SetActive(false);
            bottomFace.gameObject.SetActive(true);
        } else
        {
            controllerText.SetText("Mouse and Keyboard");
            enterButton.gameObject.SetActive(true);
            bottomFace.gameObject.SetActive(false);
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }



}
