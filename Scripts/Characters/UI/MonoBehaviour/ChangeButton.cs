using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButton : MonoBehaviour
{
    public int changeIndex;

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(SetCharacterIndex);
    }
    public void SetCharacterIndex()
    {
        PlayerSetManager.Instance.characterSet.activeIndex = changeIndex;
        Debug.Log(string.Format("Changed to {0}", changeIndex.ToString()));
        ChangeCharacterByIndex();
        InventoryManager.Instance.equipmentUI.RefreshHolders();
    }

    public void ChangeCharacterByIndex()
    {
        for (int i = 0; i < PlayerSetManager.Instance.characterSet.characterItems.Count; i++)
        {
            if (changeIndex == i)
            {
                PlayerSetManager.Instance.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                PlayerSetManager.Instance.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
