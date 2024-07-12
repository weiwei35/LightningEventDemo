using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "LightningDemo/ItemData", order = 0)]
public class ItemDataSO : ScriptableObject {
    public List<SelectItem> items = new List<SelectItem>();
    public List<SelectItem> GetSelectItems(){
        return items;
    }
    public List<SelectItem> GetRandomSelectItem(int count){
        List<SelectItem> item = new List<SelectItem>();
        List<int> random = new List<int>();
        List<int> type = new List<int>();
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(player.isCircleCopy)
            type.Add(201);
        if(player.isOnceLightningCopy)
            type.Add(202);
        if(player.isOnceTimeCopy)
            type.Add(203);
        if(player.isMegaCopy)
            type.Add(204);
        if(player.isLightningMirror)
            type.Add(301);
        if(player.isLightningBoom)
            type.Add(302);
        if(player.isLightningAttract)
            type.Add(303);
        if(player.isLightningOverflow)
            type.Add(304);
        if(player.isLightningBoomPlayer)
            type.Add(305);
        if(player.isPaperFireBall)
            type.Add(401);
        if(player.isPaperIce)
            type.Add(402);
        if(player.isPaperHP)
            type.Add(403);
        if(player.isPaperProtect)
            type.Add(404);
        if(player.isPaperBlack)
            type.Add(405);
        if(player.isPaperConnect)
            type.Add(406);
        if(player.isBugHP)
            type.Add(501);
        if(player.isBugCircle)
            type.Add(502);
        if(player.isBugWall)
            type.Add(503);
        if(player.isBugFollow)
            type.Add(504);
        if(player.isBugMerge)
            type.Add(505);
        if(player.isBugAttack)
            type.Add(506);
        if(player.isDebuffSlow)
            type.Add(601);
        if(player.isDebuffDizzy)
            type.Add(602);
        if(player.isBuffSuper)
            type.Add(603);
        if(player.isMoveRandom)
            type.Add(604);
        for (int i = 0; i < count; i++)
        {
            int randomId;
            int typeId;
            do
            {
                randomId = Random.Range(0,items.Count);
                typeId = items[randomId].type;
            } while (random.Contains(randomId) || type.Contains(typeId) || items[randomId].specialType == 3);
            random.Add(randomId);
            type.Add(typeId);
            item.Add(items[randomId]);
        }
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