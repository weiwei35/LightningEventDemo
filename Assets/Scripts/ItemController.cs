using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public TMP_Text buffIcon;
    public TMP_Text buffName;
    public TMP_Text buffDesc;
    public TMP_Text buffStory;
    SelectItem itemCurrent;
    SelectItemUI select;
    public void SetItemInfo(SelectItem item) {
        buffIcon.text = item.name[0].ToString();
        buffName.text = item.name;
        buffDesc.text = item.desc;
        buffStory.text = item.story;
        itemCurrent = item;
    }
    public void ChooseItem(){
        select = GameObject.FindWithTag("EndLevel").GetComponent<SelectItemUI>();
        select.SaveChooseItem(itemCurrent);
    }
}
