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
    public int GetItemSpecType(int id){
        foreach (var item in itemLevelRanks)
        {
            if(item.levelId == id){
                int random;
                do
                {
                    random = Random.Range(1,101);
                    if(random<=item.item1Rank){
                        return 1;
                    }else if(random>item.item1Rank && random<=item.item2Rank+item.item1Rank){
                        return 2;
                    }else if(random>item.item2Rank+item.item1Rank && random<=item.item2Rank+item.item1Rank+item.item3Rank){
                        return 3;
                    }
                }while(random>item.item3Rank+item.item2Rank+item.item1Rank);
            }
        }
        return 0;
    }
    public int GetPieceType(int id){
        foreach (var item in itemLevelRanks)
        {
            if(item.levelId == id){
                int random;
                do
                {
                    random = Random.Range(1,101);
                    if(random<=item.smallRank){
                        return 1;
                    }else if(random>item.smallRank && random<=item.smallRank+item.midRank){
                        return 2;
                    }else if(random>item.smallRank+item.midRank && random<=item.smallRank+item.midRank+item.largeRank){
                        return 3;
                    }
                }while(random>item.smallRank+item.midRank+item.largeRank);
            }
        }
        return 0;
    }
    public int GetTreasureType(int id){
        foreach (var item in itemLevelRanks)
        {
            if(item.levelId == id){
                int random;
                do
                {
                    random = Random.Range(1,101);
                    if(random<=item.littleRank){
                        return 1;
                    }else if(random>item.littleRank && random<=item.littleRank+item.bigRank){
                        return 2;
                    }
                }while(random>item.littleRank+item.bigRank);
            }
        }
        return 0;
    }
}

[System.Serializable]
public class ItemLevelRank{
    public int levelId;
    public float item1Rank;
    public float item2Rank;
    public float item3Rank;
    public float smallRank;
    public float midRank;
    public float largeRank;
    public float littleRank;
    public float bigRank;
}