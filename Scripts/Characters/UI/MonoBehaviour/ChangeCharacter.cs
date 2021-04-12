using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharacter : MonoBehaviour
{
    public CharacterSet_OS characterSet;
    public List<Button> changeButtons = new List<Button>();

    private void OnEnable()
    {
        ActivateButtons();

    }

    private void Update()
    {
        SetTransparent();
    }

    public void ActivateButtons()
    {
        for (int i = 0; i < characterSet.characterItems.Count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).gameObject.GetComponent<Image>().sprite = characterSet.characterItems[i].characterData.characterHead;
        }
    }
    public void SetTransparent()
    {
        for (int i = 0; i < characterSet.characterItems.Count; i++)
        {
            Color co = transform.GetChild(i).transform.GetComponent<Image>().color;
            if (i == characterSet.activeIndex)
                co.a = 1.0f;
            else
                co.a = 0.2f;
            transform.GetChild(i).transform.GetComponent<Image>().color = co;
        }
    }
}

