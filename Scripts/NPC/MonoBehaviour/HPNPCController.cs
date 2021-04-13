using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPNPCController : MonoBehaviour
{
    public InventoryData_SO shopData;
    public bool canShop;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canShop)
        {
            InventoryManager.Instance.shopData = shopData;
            InventoryManager.Instance.ShopPanel.SetActive(true);
            InventoryManager.Instance.bagPanel.SetActive(true);
            InventoryManager.Instance.shopUI.RefreshHolders();
            //InventoryManager.Instance.ShopPanel
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canShop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canShop = false;
        }
    }
}
