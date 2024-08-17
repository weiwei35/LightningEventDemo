using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndPanelController : MonoBehaviour
{
    public TMP_Text enemyCount;
    public TMP_Text levelCount;
    public TMP_Text levelUpCount;
    public GameObject itemFather;
    public GameObject itemIcon;
    public void SetEnemyCount(int count){
        enemyCount.text = count.ToString();
    }
    public void SetLevelCount(int count){
        levelCount.text = count.ToString();
    }
    public void SetLevelUpCount(int count){
        levelUpCount.text = count.ToString();
    }
    List<SelectItem> item1 = new List<SelectItem>();
    List<SelectItem> item2 = new List<SelectItem>();
    List<ItemIcon> itemList = new List<ItemIcon>();
    int count = 0;
    private void OnEnable() {
        SetItemList();
        SetEnemyCount(Global.enemyCount);
        SetLevelUpCount(Global.exp_level);
        SetLevelCount(Global.levelCount);
    }

    public void SetItemList(){
        foreach (var item in Global.item1Current)
        {
            count = 0;
            foreach(var obj in item1){
                if(obj.type == item.type){
                    count ++;
                }
            }
            if(count > 0){
                //重叠
                AddItem(item);
            }else{
                //新增
                item1.Add(item);
                SetItem(item);
            }
        }
        foreach (var item in Global.item2Current)
        {
            count = 0;
            foreach(var obj in item2){
                if(obj.id == item.id){
                    count ++;
                }
            }
            if(count > 0){
                //重叠
                AddItem(item);
            }else{
                //新增
                item2.Add(item);
                SetItem(item);
            }
        }
        foreach (var item in Global.item3Current)
        {
            var obj = Instantiate(itemIcon);
            obj.transform.SetParent(itemFather.transform);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<ItemIcon>().icon.text = item.name[0].ToString();
            obj.GetComponent<ItemIcon>().count.gameObject.SetActive(false);
            obj.GetComponent<ItemIcon>().itemName.text = item.name.ToString();
            obj.GetComponent<ItemIcon>().desc.text = item.desc.ToString();
            obj.GetComponent<ItemIcon>().itemType = item.id;
            itemList.Add(obj.GetComponent<ItemIcon>());
        }
    }

    void SetItem(SelectItem item){
        var obj = Instantiate(itemIcon);
        obj.transform.SetParent(itemFather.transform);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<ItemIcon>().icon.text = item.name[0].ToString();
        obj.GetComponent<ItemIcon>().count.gameObject.SetActive(false);
        obj.GetComponent<ItemIcon>().count.text = 1.ToString(); 
        obj.GetComponent<ItemIcon>().itemName.text = item.name.ToString();
        obj.GetComponent<ItemIcon>().desc.text = item.desc.ToString();
        obj.GetComponent<ItemIcon>().itemType = item.id;
        itemList.Add(obj.GetComponent<ItemIcon>());
    }

    void AddItem(SelectItem item){
        foreach (var obj in itemList)
        {
            if(item.id == obj.itemType){
                obj.count.gameObject.SetActive(true);
                int itemCount = int.Parse(obj.count.text) + 1;
                obj.count.text = itemCount.ToString();
            }
        }
    }
}
