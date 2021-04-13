using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    private Button newGameBt;
    private Button continueBt;
    private Button exitBt;
    private PlayableDirector playDirector;

    private void Awake()
    {
        newGameBt = transform.GetChild(1).GetComponent<Button>();
        continueBt = transform.GetChild(2).GetComponent<Button>();
        exitBt = transform.GetChild(3).GetComponent<Button>();

        newGameBt.onClick.AddListener(PlayTimeline);
        continueBt.onClick.AddListener(LoadGame);
        exitBt.onClick.AddListener(QuitGame);

        playDirector = FindObjectOfType<PlayableDirector>();
        playDirector.stopped += NewGame;
    }


    void PlayTimeline()
    {
        playDirector.Play();
    }

    void NewGame(PlayableDirector obj) 
    {
        // Delete strorage
        PlayerPrefs.DeleteAll();
        SceneController.Instance.TransitionToFirstLevel();
    }

    void LoadGame()
    {
        SceneController.Instance.TransitionToLoadLevel();
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game !");
    }
}
