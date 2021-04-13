using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStates : MonoBehaviour
{
    public CharacterData_SO characterData;
    public CharacterData_SO templateData;
    public AttackData_SO attackData;
    public AttackData_SO baseAttackData;

    public event Action<int, int> UpdateHealthBarOnAttack;

    [Header("Weapon")]
    public Transform weaponSlot;

    [Header("Original Weapon")]
    public GameObject originalWeaponPrefab;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        if (templateData != null)
        characterData = Instantiate(templateData);
    }

    #region Read_from_characterData
    public int MaxHealth { 
        get { if (characterData != null) return characterData.maxHealth; else return 0; } 
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth { 
        get { if (characterData != null) return characterData.currentHealth; else return 0; } 
        set { characterData.currentHealth = value; }
    }
    public int BaseDefense {
        get { if (characterData != null) return characterData.baseDefense; else return 0; } 
        set { characterData.baseDefense = value; }
    }
    public int CurrentDefense {
        get { if (characterData != null) return characterData.currentDefense; else return 0; } 
        set { characterData.currentDefense = value; }
    }
    #endregion

    #region Combat
    public void TakeDamage(CharacterStates defender)
    {
        float damage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        // Defense
        damage = Mathf.Max(damage - defender.CurrentDefense, 1.0f);

        // Criticl
        if (isCritical)
        {
            damage = damage * attackData.criticalMultiplier;
        }
        defender.CurrentHealth = Mathf.Max(defender.CurrentHealth - (int)damage, 0);

        defender.UpdateHealthBarOnAttack?.Invoke(defender.CurrentHealth, defender.MaxHealth);
        if (defender.characterData.currentHealth == 0)
            characterData.UpdateExp(defender.characterData.killPoing);
    }

    public void TakeDamage(float damage)
    {
        float currentDamage = Mathf.Max(damage - CurrentDefense, 1.0f);
        CurrentHealth = Mathf.Max(CurrentHealth - (int)currentDamage, 0);
    }
    #endregion

    #region Equipment
    public void ChangeWeapon(ItemData_SO currentWeapon, ItemData_SO changedWeapon)
    {
        UnEquipWeapon(currentWeapon);
        EquipWeapon(changedWeapon);
    }
    public void EquipWeapon(ItemData_SO weaponData)
    {
        if (weaponData)
        {
            attackData.ApplyWeaponData(weaponData.weaponData);
        }
    }
    public void UnEquipWeapon(ItemData_SO weaponData)
    {
        if (weaponData)
        {
            if (weaponSlot.transform.childCount != 0)
            {
                for (int i = 0; i < weaponSlot.transform.childCount; i++)
                {
                    Destroy(weaponSlot.transform.GetChild(i).gameObject);
                }
            }
            attackData.UnApplyWeaponData(weaponData.weaponData, baseAttackData);
           //TODO: change weapon animation
        }
    }
    #endregion

    #region Action Items
    public void ApplyHealth(int healthPoint)
    {
        CurrentHealth = CurrentHealth + healthPoint;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;

    }
    #endregion
}
