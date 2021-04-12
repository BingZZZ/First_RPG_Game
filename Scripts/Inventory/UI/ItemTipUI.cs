using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTipUI : MonoBehaviour
{
    public Text cItemName;
    public Text cItemDescription;

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();
    }
    
    public void UpdatePosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        Vector3[] corners = new Vector3[4];
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.GetWorldCorners(corners);
        float height = corners[1].y - corners[0].y;
        float width = corners[3].x - corners[0].x;
        if (mousePoint.y < height)
            rectTransform.position = mousePoint + Vector3.up * height * 0.6f;
        else if (mousePoint.x < width)
            rectTransform.position = mousePoint + Vector3.right * width * 0.6f;
        else if ((Screen.height - mousePoint.y) < height)
            rectTransform.position = mousePoint + Vector3.down * height * 0.6f;
        else if ((Screen.width - mousePoint.x) < width)
            rectTransform.position = mousePoint + Vector3.left * width * 0.6f;
        else 
            rectTransform.position = mousePoint + Vector3.left * width * 0.6f;
    }
    public void SetItemTip(string itemName, string itemDescription)
    {
        cItemName.text = itemName;
        cItemDescription.text = itemDescription;
    }
}
