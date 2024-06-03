using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public TMP_Text buffName;
    public TMP_Text buffNum;
    SelectItem itemCurrent;
    SelectItemUI select;
    public void SetItemInfo(SelectItem item) {
        buffName.text = item.name;
        if(item.buff != -1)
            buffNum.text = item.buff.ToString();
        else
            buffNum.text = "";
        itemCurrent = item;
    }
    public void ChooseItem(){
        select = GameObject.FindWithTag("EndLevel").GetComponent<SelectItemUI>();
        select.SaveChooseItem(itemCurrent);
    }
}
