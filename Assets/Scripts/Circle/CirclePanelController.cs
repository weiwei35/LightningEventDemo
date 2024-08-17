using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class CirclePanelController : MonoBehaviour
{
    public bool checkingDoor =  false;
    public DoorDataSO doorData;
    public GameObject doorItemUI;
    public GameObject doorItemPrefab;
    public void SetDoorUI(int id){
        // var door = Instantiate(doorItemPrefab);
        if(!doorItemPrefab.activeInHierarchy){
            doorItemPrefab.SetActive(true);
        }
        ItemIconDoor itemIconDoor = doorItemPrefab.GetComponent<ItemIconDoor>();
        //设置奇门icon内容
        itemIconDoor.icon.text = doorData.GetDoorItem(id).name[0].ToString();
        itemIconDoor.itemName.text = doorData.GetDoorItem(id).name;
        itemIconDoor.desc.text = doorData.GetDoorItem(id).desc;
    }
    public void DeletDoorUI(){
        foreach (Transform item in doorItemUI.transform)
        {
            Destroy(item.gameObject);
        }
    }
    void CheckDoorId(){
        // DeletDoorUI();
        int id = -1;
        if(inDoor_KAI){
            id = 0;
        }else if(inDoor_XIU){
            id = 1;
        }else if(inDoor_SHENG){
            id = 2;
        }else if(inDoor_JING){
            id = 3;
        }else if(inDoor_SHANG){
            id = 4;
        }else if(inDoor_DU){
            id = 5;
        }else if(inDoor_SI){
            id = 6;
        }else if(inDoor_JING_Bad){
            id = 7;
        }

        if(id!= -1){
            SetDoorUI(id);
        }else{
            doorItemPrefab.SetActive(false);
        }
    }

    public bool inDoor_DU = false;
    public bool inDoor_JING = false;
    public bool inDoor_SI = false;
    public bool inDoor_JING_Bad = false;
    public bool inDoor_KAI = false;
    public bool inDoor_XIU = false;
    public bool inDoor_SHENG = false;
    public bool inDoor_SHANG = false;

    public float turnTime = 5;
    public float sleepTime = 10;
    float countTime = 0;
    bool isTurn = true;
    public GameObject[] circles;
    public bool isFadeIn = false;
    public bool isFadeOut = false;
    public bool checkFade = false;
    // bool saveDoor = false;
    public SpriteRenderer[] circle;

    private void Update() {
        if(isTurn && Global.GameBegain){
            // DeletDoorUI();
            CheckDoorId();
            // saveDoor = false;
            countTime += Time.deltaTime;
            if(countTime >= turnTime){
                isFadeOut = true;
                checkingDoor = true;
                // saveDoor = true;
                isTurn = false;
                countTime = 0;
                SetCircleFadeOut();
            }
        }else if(Global.GameBegain){
            // saveDoor = false;
            countTime += Time.deltaTime;
            if(countTime >= sleepTime){
                isTurn = true;
                countTime = 0;
                isFadeIn = true;
                SetCircleFadeIn();
                foreach (var item in circles)
                    {
                        // SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
                        // sprite.material.color = new Color(sprite.material.color.r,sprite.material.color.g,sprite.material.color.b,0);
                        item.SetActive(true);
                        // sprite.material.DOFade(1,0.3f);
                    }
            }
        }
            if(!isFadeOut && !isTurn){
                foreach (var item in circles)
                    {
                        // SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
                        // sprite.material.DOFade(0,0.3f).OnComplete(()=>{
                            item.SetActive(false);
                        // });
                    }
        }
        
    }
    void SetCircleFadeOut(){
        foreach (var item in circle)
        {
            item.DOFade(0,1);
        }
    }
    void SetCircleFadeIn(){
        foreach (var item in circle)
        {
            item.DOFade(1,1);
        }
    }

    public void ResetCircle(){
        isTurn = true;
        countTime = 0;
        foreach (var item in circles)
        {
            // SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
            // sprite.material.color = new Color(sprite.material.color.r,sprite.material.color.g,sprite.material.color.b,0);
            item.SetActive(true);
            // sprite.material.DOFade(1,0.3f);
        }
        isFadeIn = true;
        SetCircleFadeIn();
    }
}
