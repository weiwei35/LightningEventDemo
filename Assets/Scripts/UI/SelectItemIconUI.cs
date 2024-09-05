using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectItemIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text icon;

    public int itemType;//用于叠加同类型道具

    public GameObject tips;
    public TMP_Text itemName;
    public TMP_Text desc;
    public TMP_Text story;
    public TMP_Text cost;
    RectTransform rectTransform;
    SelectItem itemCurrent;
    SelectItemUI select;

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        tips.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tips.SetActive(false);
    }

    public void SetItemInfo(SelectItem item) {
        icon.text = item.name[0].ToString();
        itemName.text = item.name;
        desc.text = item.desc;
        story.text = item.story;
        cost.text = "$" + (item.cost + item.cost * Global.levelCount * item.costGrow);
        itemCurrent = item;
    }

    private void Start() {
        select = GameObject.FindWithTag("EndLevel").GetComponent<SelectItemUI>();
    }
    public void ChooseItem(){
        // select.SaveChooseItem(itemCurrent);
        select.SetPlayerStatus(itemCurrent,gameObject);
    }
}
