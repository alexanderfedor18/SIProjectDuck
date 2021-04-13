using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string playSceneName;

    private GameObject lastSelected;

    private void Awake()
    {
        lastSelected = EventSystem.current.currentSelectedGameObject;
    }


    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
        lastSelected = EventSystem.current.currentSelectedGameObject;
    }


    public void PlayGame()
    {
        if (playSceneName != null)
        {
            SceneManager.LoadScene(playSceneName);
        }
        else
            Debug.Log("The play scene name is missing!");

    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
