using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>
{
    private GameObject player;
    private GameObject fader;
    public GameObject playerPrefab;
    public GameObject faderPrefab;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch(transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        SaveManager.Instance.SavePlayerData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            fader = Instantiate(faderPrefab);
            yield return fader.GetComponent<Fader>().FadeOut(1.5f);
            yield return SceneManager.LoadSceneAsync(sceneName);
            // transitionDestination need to be defined here but not the line before "if". Because after reloading the new scene all varaibles would be disapeared.
            TransitionDestination transitionDestination = GetTransitionDestination(destinationTag);
            yield return Instantiate(playerPrefab, transitionDestination.transform.position, transitionDestination.transform.rotation);
            SaveManager.Instance.LoadPlayerData();
            yield return fader.GetComponent<Fader>().FadeIn(1.5f);
            Destroy(fader.gameObject);
            yield break;
        } else
        {
            player = GameManager.Instance.player.gameObject;
            NavMeshAgent playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            TransitionDestination transitionDestination = GetTransitionDestination(destinationTag);
            player.transform.SetPositionAndRotation(transitionDestination.transform.position, transitionDestination.transform.rotation);
            playerAgent.enabled = true;
            yield break;
        }
    }

    private TransitionDestination GetTransitionDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        foreach (var entrance in entrances)
        {
            if (entrance.destinationTag == destinationTag)
            {
                return entrance;
            }
        }
        return null;
    }

    public void TransitionToLoadLevel()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SaveLevel));
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("SampleScene"));
    }

    public void TransitionToMainLevel()
    {
        StartCoroutine(LoadMain());
    }
    
    IEnumerator LoadLevel(string sceneName)
    {
        fader = Instantiate(faderPrefab);
        yield return fader.GetComponent<Fader>().FadeOut(1.5f);
        yield return SceneManager.LoadSceneAsync(sceneName);
        TransitionDestination entrance = GetTransitionDestination(TransitionDestination.DestinationTag.Enter);
        yield return player = Instantiate(playerPrefab, entrance.transform.position, entrance.transform.rotation);
        SaveManager.Instance.SavePlayerData();
        yield return fader.GetComponent<Fader>().FadeIn(1.5f);
        Destroy(fader.gameObject);
        yield break;
    }

    IEnumerator LoadMain()
    {
        yield return SceneManager.LoadSceneAsync("Main");
        yield break;
    }
}
