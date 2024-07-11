using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SelectItemUI : MonoBehaviour
{
    public InfoUIController uIController;
    // public ItemIcon itemIcon;
    public ItemDataSO itemData;
    public GameObject selectBar;
    public GameObject selectPrefab;
    SelectItem saveItem;
    List<SelectItem> items;
    PlayerController player;
    // Start is called before the first frame update
    void OnEnable() {
        // itemData = AssetDatabase.LoadAssetAtPath<ItemDataSO>("Assets/Resources/ItemData.asset");
        items = itemData.GetRandomSelectItem(3);
        ShowSelectItem();
    }
    void ShowSelectItem(){
        for (int i = 0; i < items.Count; i++)
        {
            var selectItem = Instantiate(selectPrefab);
            selectItem.transform.SetParent(selectBar.transform);
            selectItem.transform.localScale = new Vector3(1,1,1);
            selectItem.transform.localPosition = Vector3.zero;
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
            SwitchUI();
            SwitchLogic();
            return true;
        }else{
            return false;
        }
    }

    void SwitchUI(){
        //处理UI显示
            switch (saveItem.specialType) {
                case 1:
                    Global.item1Current.Add(saveItem);
                    int count = 0;
                    foreach (var item in Global.item1Current)
                    {
                        Debug.Log(item.name);
                        if(item.type == saveItem.type){
                            count++;
                        }
                    }
                    // itemIcon.itemType = saveItem.type;
                    // itemIcon.count.text = "";
                    if(count > 1)
                    {
                        // itemIcon.count.text = count.ToString();
                        uIController.AddItem1(saveItem.name[0].ToString(),count,saveItem.type);
                    }else{
                        // itemIcon.icon.text = saveItem.name[0].ToString();
                        // itemIcon.itemName.text = saveItem.name.ToString();
                        // itemIcon.desc.text = saveItem.desc.ToString();
                        uIController.SetItem1(saveItem.name[0].ToString(),saveItem.name.ToString(),saveItem.desc.ToString(),saveItem.type);
                    }
                    return;
                case 2:
                    Global.item2Current.Add(saveItem);
                    // itemIcon.count.text = "";
                    // itemIcon.icon.text = saveItem.name[0].ToString();
                    // itemIcon.itemName.text = saveItem.name.ToString();
                    // itemIcon.desc.text = saveItem.desc.ToString();
                    uIController.SetItem2(saveItem.name[0].ToString(),saveItem.name.ToString(),saveItem.desc.ToString());
                    return;
                case 3:
                    Global.item3Current.Add(saveItem);
                    // itemIcon.count.text = "";
                    // itemIcon.icon.text = saveItem.name[0].ToString();
                    // itemIcon.itemName.text = saveItem.name.ToString();
                    // itemIcon.desc.text = saveItem.desc.ToString();
                    uIController.SetItem3(saveItem.name[0].ToString(),saveItem.name.ToString(),saveItem.desc.ToString());
                    return;
            }
    }
    void SwitchLogic(){
        //处理数值逻辑
            switch (saveItem.type) {
                case 1:
                    player.SetHP(saveItem.buff);
                    return;
                case 2:
                    player.SetSpeed(saveItem.buff);
                    return;
                case 3:
                    player.SetProtect(saveItem.buff);
                    return;
                case 5:
                    player.SetHPSpeed(saveItem.buff);
                    return;
                case 6:
                    player.SetProtectSpeed(saveItem.buff);
                    return;
                case 201:
                    player.SetCircleCopy();
                    return;
                case 202:
                    player.SetOnceLightningCopy();
                    return;
                case 203:
                    player.SetOnceTimeCopy();
                    return;
                case 204:
                    player.SetMegaCopy();
                    return;
                case 301:
                    player.SetLightningMirror();
                    return;
                case 302:
                    player.SetLightningBoom();
                    return;
                case 303:
                    player.SetLightningOverflow();
                    return;
                case 304:
                    player.SetLightningAttract();
                    return;
                case 305:
                    player.SetLightningBoomPlayer();
                    return;
                case 401:
                    player.SetPaperFireBall();
                    return;
                case 402:
                    player.SetPaperIce();
                    return;
                case 403:
                    player.SetPaperHP();
                    return;
                case 404:
                    player.SetPaperProtect();
                    return;
                case 405:
                    player.SetPaperBlack();
                    return;
                case 406:
                    player.SetPaperConnect();
                    return;
                case 501:
                    player.SetBugHP();
                    return;
                case 502:
                    player.SetBugCircle();
                    return;
                case 503:
                    player.SetBugWall();
                    return;
                case 504:
                    player.SetBugFollow();
                    return;
                case 505:
                    player.SetBugMerge();
                    return;
                case 506:
                    player.SetBugAttack();
                    return;
                case 601:
                    player.SetDebuffSlow();
                    return;
                case 602:
                    player.SetDebuffDizzy();
                    return;
                case 603:
                    player.SetBuffSuper();
                    return;
                case 604:
                    player.SetMoveRandom();
                    return;
                default:
                    return;
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
