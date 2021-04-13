using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : Singleton<PlayerHealthBarUI>
{
    private Image healthSlider;
    private Image expSlider;
    private Text levelText;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();   
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        levelText = transform.GetChild(2).GetComponent<Text>();
    }

    private void Update()
    {
        UpdateHealth();
        UpdateExp();
        levelText.text = "Lv  " + GameManager.Instance.player.characterData.currentLevel.ToString();
    }

    private void UpdateHealth()
    {
        float fillPercent = (float)GameManager.Instance.player.CurrentHealth / GameManager.Instance.player.MaxHealth;
        healthSlider.fillAmount = fillPercent;
    }

    private void UpdateExp()
    {
        float fillPercent = (float)GameManager.Instance.player.characterData.currentExp / GameManager.Instance.player.characterData.baseExp;
        expSlider.fillAmount = fillPercent;
    }
}
