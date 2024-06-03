using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SelectItemUI : MonoBehaviour
{
    public ItemDataSO itemData;
    public GameObject selectBar;
    public GameObject selectPrefab;
    SelectItem saveItem;
    List<SelectItem> items;
    PlayerController player;
    // Start is called before the first frame update
    void OnEnable() {
        itemData = AssetDatabase.LoadAssetAtPath<ItemDataSO>("Assets/Resources/ItemData.asset");
        items = itemData.GetRandomSelectItem(3);
        ShowSelectItem();
    }
    void ShowSelectItem(){
        for (int i = 0; i < items.Count; i++)
        {
            var selectItem = Instantiate(selectPrefab);
            selectItem.transform.SetParent(selectBar.transform);
            ItemController item = selectItem.GetComponent<ItemController>();
            item.SetItemInfo(items[i]);
        }
    }
    public void SaveChooseItem(SelectItem item){
        saveItem = item;
    }
    public bool SetPlayerStatus(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(saveItem != null)
        {
            switch (saveItem.type) {
                case 1:
                    player.SetHP(saveItem.buff);
                    return true;
                case 2:
                    player.SetSpeed(saveItem.buff);
                    return true;
                case 3:
                    player.SetProtect(saveItem.buff);
                    return true;
                case 5:
                    player.SetHPSpeed(saveItem.buff);
                    return true;
                case 6:
                    player.SetProtectSpeed(saveItem.buff);
                    return true;
                case 201:
                    player.SetCircleCopy();
                    return true;
                case 202:
                    player.SetOnceLightningCopy();
                    return true;
                case 203:
                    player.SetOnceTimeCopy();
                    return true;
                case 301:
                    player.SetLightningMirror();
                    return true;
                case 302:
                    player.SetLightningBoom();
                    return true;
                case 303:
                    player.SetLightningOverflow();
                    return true;
                default:
                    return false;
            }
        }else{
            return false;
        }
    }

    private void OnDisable() {
        items.Clear();
        saveItem = new SelectItem();
        Transform select = selectBar.transform; // 获取当前GameObject的Transform
        for (int i = 0; i < select.childCount; i++) // 遍历所有子物体
        {
            Transform child = select.GetChild(i); // 获取子物体的Transform
            Destroy(child.gameObject); // 打印子物体的名字
        }
    }
}
