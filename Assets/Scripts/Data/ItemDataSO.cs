using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "LightningDemo/ItemData", order = 0)]
public class ItemDataSO : ScriptableObject {
    public List<SelectItem> items = new List<SelectItem>();
    public List<SelectItem> GetSelectItems(){
        return items;
    }
    public List<SelectItem> GetRandomSelectItem(int count,float item1Rank,float item2Rank,float item3Rank){
        List<SelectItem> item = new List<SelectItem>();
        List<int> random = new List<int>();
        List<int> type = new List<int>();
        List<SelectItem> saveItem = new List<SelectItem>();
        foreach (var save in items)
        {
            saveItem.Add(save);
        }
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(player.isCircleCopy){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 201)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(201);
        if(player.isOnceLightningCopy){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 202)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(202);
        if(player.isOnceTimeCopy){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 203)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(203);
        if(player.isMegaCopy){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 204)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(204);
        if(player.isLightningMirror){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 301)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(301);
        if(player.isLightningBoom){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 302)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(302);
        if(player.isLightningAttract){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 303)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(303);
        if(player.isLightningOverflow){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 304)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(304);
        if(player.isLightningBoomPlayer){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 305)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(305);
        if(player.isPaperFireBall){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 401)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(401);
        if(player.isPaperIce){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 402)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(402);
        if(player.isPaperHP){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 403)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(403);
        if(player.isPaperProtect){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 404)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(404);
        if(player.isPaperBlack){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 405)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(405);
        if(player.isPaperConnect){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 406)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(406);
        if(player.isBugHP){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 501)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(501);
        if(player.isBugCircle){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 502)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(502);
        if(player.isBugWall){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 503)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(503);
        if(player.isBugFollow){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 504)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(504);
        if(player.isBugMerge){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 505)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(505);
        if(player.isBugAttack){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 506)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(506);
        if(player.isDebuffSlow){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 601)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(601);
        if(player.isDebuffDizzy){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 602)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(602);
        if(player.isBuffSuper){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 603)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(603);
        if(player.isMoveRandom){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type == 604)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(604);
        for (int i = 0; i < count; i++)
        {
            bool canSaveItem = true;
            int currentSpecType = Random.Range(1,101);
            Debug.Log(currentSpecType);
            //随机到神通道具
            if(currentSpecType <= item3Rank*100 && canSaveItem){
                canSaveItem = false;
                List<SelectItem> type3Item = new List<SelectItem>();
                foreach (var t in saveItem)
                {
                    if(t.specialType == 3){
                        type3Item.Add(t);
                    }
                }
                int randomId = Random.Range(0,type3Item.Count);
                item.Add(type3Item[randomId]);
                for (int j = 0; j < saveItem.Count; j++)
                {
                    if(saveItem[j].type == type3Item[randomId].type)
                        saveItem.Remove(saveItem[j]);
                }
            }
            //随机到法宝道具
            else if(currentSpecType <= item2Rank*100 && canSaveItem){
                canSaveItem = false;
                List<SelectItem> type2Item = new List<SelectItem>();
                foreach (var t in saveItem)
                {
                    if(t.specialType == 2){
                        type2Item.Add(t);
                    }
                }
                int randomId = Random.Range(0,type2Item.Count);
                item.Add(type2Item[randomId]);
                for (int j = 0; j < saveItem.Count; j++)
                {
                    if(saveItem[j].type == type2Item[randomId].type)
                        saveItem.Remove(saveItem[j]);
                }
            }
            //随机到碎片道具
            else if(currentSpecType <= item1Rank*100 && canSaveItem){
                canSaveItem = false;
                List<SelectItem> type1Item = new List<SelectItem>();
                foreach (var t in saveItem)
                {
                    if(t.specialType == 1){
                        type1Item.Add(t);
                    }
                }
                int randomId = Random.Range(0,type1Item.Count);
                item.Add(type1Item[randomId]);
                int typeCur = type1Item[randomId].type;
                for (int j = 0; j < saveItem.Count; j++)
                {
                    if(saveItem[j].type == typeCur){
                        saveItem.Remove(saveItem[j]);
                        j--;
                    }
                }
            }
            else if(currentSpecType > item1Rank*100 && currentSpecType > item2Rank*100 && currentSpecType > item3Rank*100){
                i--;
            }
        }
        
        // for (int i = 0; i < count; i++)
        // {
        //     int randomId;
        //     int typeId;
        //     do
        //     {
        //         int currentSpecType = Random.Range(0,11);
        //         if(currentSpecType <= item1Rank*10)
        //         randomId = Random.Range(0,items.Count);
        //         typeId = items[randomId].type;
        //     } while (random.Contains(randomId) || type.Contains(typeId));
        //     random.Add(randomId);
        //     type.Add(typeId);
        //     item.Add(items[randomId]);
        // }
        return item;
    }

    public SelectItem GetRandomTrans(){
        int randomId = Random.Range(0,6);
        int i = 0;
        foreach (var item in items)
        {
            if(item.specialType == 3){
                if(i == randomId){
                    return item;
                }else{
                    i++;
                }
            }
        }
        return null;
    }

    public SelectItem GetSelectItemById(int idSave){
        foreach (var item in items)
        {
            if(item.id == idSave){
                return item;
            }
        }
        return null;
    }
}

[System.Serializable]
public class SelectItem{
    public int id;
    public string name;
    public string desc;
    public int type;
    public int buff;
    public int specialType;
    public string story;
}