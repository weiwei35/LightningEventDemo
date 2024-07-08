using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoUIController : MonoBehaviour
{
    public TMP_Text saying;
    public TMP_Text protect;
    public TMP_Text hp;
    public TMP_Text speed;
    public TMP_Text lightningHurt;
    public TMP_Text lightningSpeed;
    public TMP_Text lightningCount;
    public GameObject item1Parent;
    public GameObject item2Parent;
    public GameObject item3Parent;
    List<ItemIcon> item1List = new List<ItemIcon>();

    
    public void SetSayingText(string text) {
        saying.text = text;
    }
    public void SetProtectText(float text) {
        protect.text = text.ToString();
    }
    public void SetHPText(float text) {
        hp.text = text.ToString();
    }
    public void SetSpeedText(float text) {
        speed.text = text.ToString();
    }
    public void SetLightningHurtText(float text) {
        lightningHurt.text = text.ToString();
    }
    public void SetLightningSpeedText(float text) {
        lightningSpeed.text = text.ToString();
    }
    public void SetLightningCountText(float text) {
        lightningCount.text = text.ToString();
    }

    public void SetItem1(ItemIcon item) {
        var icon = Instantiate(item.gameObject);
        icon.transform.parent = item1Parent.transform;
        icon.transform.localScale = new Vector3(1,1,1);
        item1List.Add(icon.GetComponent<ItemIcon>());
    }
    public void AddItem1(ItemIcon itemCurrent){
        foreach (var item in item1List)
        {
            if(item.itemType == itemCurrent.itemType){
                item.count.gameObject.SetActive(true);
                item.count = itemCurrent.count;
            }
        }
    }
    public void SetItem2(ItemIcon item) {
        var icon = Instantiate(item.gameObject);
        icon.transform.parent = item2Parent.transform;
        icon.transform.localScale = new Vector3(1,1,1);
    }
    public void SetItem3(ItemIcon item) {
        var icon = Instantiate(item.gameObject);
        icon.transform.parent = item3Parent.transform;
        icon.transform.localScale = new Vector3(1,1,1);
    }
}
