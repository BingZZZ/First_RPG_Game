using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSet Data", menuName = "CharacterSet/CharacterSet Data")]
public class CharacterSet_OS : ScriptableObject
{

    public int activeIndex;
    public List<CharacterItem> characterItems = new List<CharacterItem>();
}

[System.Serializable]
public class CharacterItem
{
    public GameObject character;
    public CharacterData_SO characterData;
}
