using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthBar;
    public Transform barPoint;
    public bool alwaysVisible;
    private float visibleTime = 5.0f;
    private float timeLeft;

    Image healthSlider;
    Transform UIBar;
    Transform cam;

    CharacterStates currentChara;

    private void Awake()
    {
        currentChara = GetComponent<CharacterStates>();
        currentChara.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar = Instantiate(healthBar, canvas.transform).transform;
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                UIBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void LateUpdate()
    {
        if (UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;
            timeLeft -= Time.deltaTime;
            if (timeLeft > 0f && !alwaysVisible)
                UIBar.gameObject.SetActive(true);
            else
                UIBar.gameObject.SetActive(false);
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth == 0)
        {
            Destroy(UIBar.gameObject);
        }
        timeLeft = visibleTime;
        UIBar.gameObject.SetActive(true);
        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
}
