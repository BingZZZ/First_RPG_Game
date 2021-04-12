using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalSlot;
        public Transform originalParent;
    }
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentData;
    public InventoryData_SO shopData;

    [Header("Container UI")]
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;
    public ContainerUI shopUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDragData;

    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject statesPanel;
    public GameObject ShopPanel;

    public bool isOpen = false;

    [Header("Character States")]
    public Text cHealth;
    public Text cAttack;
    public Text cCriticalChance;
    public Text cCriticalMulti;

    private CharacterStates playerStates;

    [Header("Item Tip")]
    public ItemTipUI itemTipUI;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statesPanel.SetActive(isOpen);
        }
        playerStates = GameManager.Instance.player;
        UpdateStates(playerStates.CurrentHealth, playerStates.MaxHealth,
            playerStates.attackData.minDamage, playerStates.attackData.maxDamage,
            playerStates.attackData.criticalChance, playerStates.attackData.criticalMultiplier);
    }
    public void UpdateStates(int currentHealth, int maxHealth, int minAttack, int maxAttack, float criticalChance, float criticalMulti)
    {
        cHealth.text = string.Format("{0}/{1}", currentHealth.ToString(), maxHealth.ToString());
        cAttack.text = string.Format("{0}-{1}", minAttack.ToString(), maxAttack.ToString());
        cCriticalChance.text = string.Format("{0}%", (criticalChance * 100).ToString());
        cCriticalMulti.text = string.Format("{0}%", (criticalMulti * 100).ToString());
    }

    #region Check if mouse point in slot range
    public bool CheckInInventoryUI(Vector3 mPosition)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform rt = (RectTransform)inventoryUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, mPosition))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckInActionUI(Vector3 mPosition)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform rt = (RectTransform)actionUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, mPosition))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckInEquipmentUI(Vector3 mPosition)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform rt = (RectTransform)equipmentUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, mPosition))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
    
}
