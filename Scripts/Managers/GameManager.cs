using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStates player;
    public List<IEndGameObserver> allEndGameObservers = new List<IEndGameObserver>();
    private CinemachineFreeLook followCamera;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void RegisterPlayer(CharacterStates playerStates)
    {
        player = playerStates;
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        LookAtPoint lookAtPoint = FindObjectOfType<LookAtPoint>();
        if (followCamera != null)
        {
            followCamera.Follow = lookAtPoint.transform;
            followCamera.LookAt = lookAtPoint.transform;
        }
    }

    public void AddEndGameObserver(IEndGameObserver observer)
    {
        allEndGameObservers.Add(observer);
    }
    public void RemoveEndGameObserver(IEndGameObserver observer)
    {
        allEndGameObservers.Remove(observer);
    }

    public void NotifyAllObservers()
    {
        foreach (var observer in allEndGameObservers)
        {
            observer.EndNotify();
        }
    }
}
