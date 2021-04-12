using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack States/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public float doubleAttack;
    public int minDamage;
    public int maxDamage;
    public float criticalMultiplier;
    public float criticalChance;

    public void ApplyWeaponData(AttackData_SO weaponData)
    {
        attackRange = weaponData.attackRange;
        coolDown = weaponData.coolDown;
        minDamage += weaponData.minDamage;
        maxDamage += weaponData.maxDamage;
        criticalMultiplier += weaponData.criticalMultiplier;
        criticalChance += weaponData.criticalChance;
    }
    public void UnApplyWeaponData(AttackData_SO weaponData, AttackData_SO baseData)
    {
        attackRange = baseData.attackRange;
        coolDown = baseData.coolDown;
        minDamage -= weaponData.minDamage;
        maxDamage -= weaponData.maxDamage;
        criticalMultiplier -= weaponData.criticalMultiplier;
        criticalChance -= weaponData.criticalChance;
    }
}
