using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetManager : Singleton<PlayerSetManager>
{
    public Vector3 resetPosition;
    public Quaternion resetRotation;
    public CharacterSet_OS characterSet;
    public Transform lookAtPoint;
    private GameObject activatePlayer;

    public void OnEnable()
    {
        InstantiateCharacters();
    }

    private void Start()
    {
    }
    public void SetPositionRotation(Transform playerTransform)
    {
        lookAtPoint.position = playerTransform.position;
        lookAtPoint.rotation = playerTransform.rotation;
    }

    private void InstantiateCharacters()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < characterSet.characterItems.Count; i++)
        {
            GameObject InstantiatedCharacter = Instantiate(characterSet.characterItems[i].character, transform);
            if (i == characterSet.activeIndex) activatePlayer = InstantiatedCharacter;
            InstantiatedCharacter.SetActive(false);
        }
        activatePlayer.SetActive(true);

        InventoryManager.Instance.inventoryUI.RefreshHolders();
        InventoryManager.Instance.actionUI.RefreshHolders();
        InventoryManager.Instance.equipmentUI.RefreshHolders();
    }
}
