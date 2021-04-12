using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private string saveLevel = "saveLevel";

    public string SaveLevel { get { return PlayerPrefs.GetString(saveLevel); } }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToMainLevel();
            Destroy(CharacterSetManager.Instance.gameObject);
            Destroy(InventoryManager.Instance.gameObject);
            Destroy(PlayerHealthBarUI.Instance.gameObject);
        }
        
    }

    public void SavePlayerData()
    {
        SaveData(GameManager.Instance.player.characterData, GameManager.Instance.player.name);
        Debug.Log(GameManager.Instance.player.name);
    }
    public void LoadPlayerData()
    {
        LoadData(GameManager.Instance.player.characterData, GameManager.Instance.player.name);
    }

    public void SaveData(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(saveLevel, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void LoadData(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }

    }
}
