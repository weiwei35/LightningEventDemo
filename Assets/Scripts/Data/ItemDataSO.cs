using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "LightningEvent/ItemData", order = 0)]
public class ItemDataSO : ScriptableObject {
    public List<SelectItem> items = new List<SelectItem>();
    public List<SelectItem> GetSelectItems(){
        return items;
    }
    public List<SelectItem> GetRandomSelectItem(int count,List<int> specType,List<int> detailType){
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
                if(saveItem[i].type[0] == 201)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(201);
        if(player.isOnceLightningCopy){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 202)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(202);
        if(player.isOnceTimeCopy){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 203)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(203);
        if(player.isMegaCopy){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 204)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(204);
        if(player.isLightningMirror){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 301)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(301);
        if(player.isLightningBoom){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 302)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(302);
        if(player.isLightningAttract){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 303)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(303);
        if(player.isLightningOverflow){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 304)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(304);
        if(player.isLightningBoomPlayer){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 305)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(305);
        if(player.isPaperFireBall){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 401)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(401);
        if(player.isPaperIce){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 402)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(402);
        if(player.isPaperHP){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 403)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(403);
        if(player.isPaperProtect){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 404)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(404);
        if(player.isPaperBlack){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 405)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(405);
        if(player.isPaperConnect){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 406)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(406);
        if(player.isBugHP){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 501)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(501);
        if(player.isBugCircle){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 502)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(502);
        if(player.isBugWall){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 503)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(503);
        if(player.isBugFollow){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 504)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(504);
        if(player.isBugMerge){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 505)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(505);
        if(player.isBugAttack){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 506)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(506);
        if(player.isDebuffSlow){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 601)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(601);
        if(player.isDebuffDizzy){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 602)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(602);
        if(player.isBuffSuper){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 603)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(603);
        if(player.isMoveRandom){
            for (int i = 0; i < saveItem.Count; i++)
            {
                if(saveItem[i].type[0] == 604)
                    saveItem.Remove(saveItem[i]);
            }
        }
            //type.Add(604);
        for (int i = 0; i < count; i++)
        {
            //随机到神通道具
            if(specType[i] == 3){
                List<SelectItem> type3Item = new List<SelectItem>();
                foreach (var t in saveItem)
                {
                    if(t.specialType == 3){
                        if(detailType[i] > 0){
                            if(t.detailType == detailType[i])
                                type3Item.Add(t);
                        }
                    }
                }
                int randomId = Random.Range(0,type3Item.Count);
                while (Global.itemShowInShop.Contains(type3Item[randomId]))
                {
                    randomId++;
                    if(randomId >= type3Item.Count){
                        randomId = 0;
                    }
                }
                item.Add(type3Item[randomId]);

                for (int j = 0; j < saveItem.Count; j++)
                {
                    if(saveItem[j].type == type3Item[randomId].type){
                        saveItem.Remove(saveItem[j]);
                        j--;
                    }
                }
            }
            //随机到法宝道具
            else if(specType[i] == 2){
                List<SelectItem> type2Item = new List<SelectItem>();
                foreach (var t in saveItem)
                {
                    if(t.specialType == 2){
                        if(detailType[i] > 0){
                            if(t.detailType == detailType[i])
                                type2Item.Add(t);
                        }
                    }
                }
                int randomId = Random.Range(0,type2Item.Count);
                while (Global.itemShowInShop.Contains(type2Item[randomId]))
                {
                    randomId++;
                    if(randomId >= type2Item.Count){
                        randomId = 0;
                    }
                }
                item.Add(type2Item[randomId]);
                for (int j = 0; j < saveItem.Count; j++)
                {
                    if(saveItem[j].type == type2Item[randomId].type){
                        saveItem.Remove(saveItem[j]);
                        j--;
                    }
                }
            }
            //随机到碎片道具
            else if(specType[i] == 1){
                List<SelectItem> type1Item = new List<SelectItem>();
                foreach (var t in saveItem)
                {
                    if(t.specialType == 1){
                        if(detailType[i] > 0){
                            if(t.detailType == detailType[i]){
                                type1Item.Add(t);
                            }
                        }
                    }
                }
                int randomId = Random.Range(0,type1Item.Count);
                item.Add(type1Item[randomId]);
                int typeCur = type1Item[randomId].type[0];
                for (int j = 0; j < saveItem.Count; j++)
                {
                    if(saveItem[j].type[0] == typeCur){
                        saveItem.Remove(saveItem[j]);
                        j--;
                    }
                }
            }
        }
        foreach(var obj in item){
            Global.itemShowInShop.Add(obj);
        }
        return item;
    }

    public SelectItem GetRandomTrans(){
        int randomId = Random.Range(0,6);
        int i = 0;
        foreach (var item in items)
        {
            if(item.specialType == 4){
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
    public int[] type;
    public int[] buff;
    public int specialType;
    public string story;
    public int detailType;
    public float cost;
    public float costGrow;
}