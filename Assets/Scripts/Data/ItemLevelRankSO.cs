using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemLevelRankSO", menuName = "LightningEvent/ItemLevelRankSO", order = 0)]
public class ItemLevelRankSO : ScriptableObject {
    public List<ItemLevelRank> itemLevelRanks = new List<ItemLevelRank>();
    public float GetItem1RankByLevel(int id){
        foreach (var item in itemLevelRanks)
        {
            if(item.levelId == id){
                return item.item1Rank / (item.item1Rank+item.item2Rank+item.item3Rank);
            }
        }
        return -1;
    }
    public float GetItem2RankByLevel(int id){
        foreach (var item in itemLevelRanks)
        {
            if(item.levelId == id){
                return item.item2Rank / (item.item1Rank+item.item2Rank+item.item3Rank);
            }
        }
        return -1;
    }
    public float GetItem3RankByLevel(int id){
        foreach (var item in itemLevelRanks)
        {
            if(item.levelId == id){
                return item.item3Rank / (item.item1Rank+item.item2Rank+item.item3Rank);
            }
        }
        return -1;
    }
}

[System.Serializable]
public class ItemLevelRank{
    public int levelId;
    public float item1Rank;
    public float item2Rank;
    public float item3Rank;
}