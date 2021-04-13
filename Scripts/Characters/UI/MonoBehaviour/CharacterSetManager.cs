using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetManager : Singleton<CharacterSetManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}
