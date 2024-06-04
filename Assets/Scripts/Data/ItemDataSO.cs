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
        if(player.isLightningMirror)
            type.Add(301);
        if(player.isLightningBoom)
            type.Add(302);
        if(player.isLightningOverflow)
            type.Add(303);
        if(player.isLightningAttract)
            type.Add(304);
        for (int i = 0; i < count; i++)
        {
            int randomId;
            int typeId;
            do
            {
                randomId = Random.Range(0,items.Count);
                typeId = items[randomId].type;
            } while (random.Contains(randomId) || type.Contains(typeId));
            random.Add(randomId);
            type.Add(typeId);
            item.Add(items[randomId]);
        }
        return item;
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
}