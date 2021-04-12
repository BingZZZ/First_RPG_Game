using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character States/Data")]
public class CharacterData_SO : ScriptableObject
{
    public int maxHealth;
    public int currentHealth;
    public int baseDefense;
    public int currentDefense;

    [Header("exp")]
    public int killPoing;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;
    public float expIncrease;

    [Header("Equipment")]
    public InventoryData_SO equipmentData;

    [Header("Player Style")]
    public PlayerStyle playerStyle;

    [Header("Character Head")]
    public Sprite characterHead;
    
    public float LevelMultiplier
    {
        get { return 1 + levelBuff; }
    }
    public float ExpMultiplier
    {
        get { return 1 + expIncrease; }
    }

    public void UpdateExp(int point)
    {
        if (currentLevel == maxLevel)
            return;
        currentExp += point;
        if (currentExp >= baseExp)
        {
            currentExp -= baseExp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        maxHealth = (int) (maxHealth * LevelMultiplier) + 1;
        currentHealth = maxHealth;
        currentDefense -= baseDefense;
        baseDefense = (int)(baseDefense * LevelMultiplier) + 1;
        currentDefense += baseDefense;
        baseExp = (int)(baseExp * ExpMultiplier) + 1;
        currentLevel += 1;
    }
}
