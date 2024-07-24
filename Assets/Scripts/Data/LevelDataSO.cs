using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "LightningEvent/LevelData", order = 0)]
public class LevelDataSO : ScriptableObject {
    public List<LevelItem> levels = new List<LevelItem>();
    public LevelItem GetLevelDataById(int id) {
        foreach (var item in levels)
        {
            if(item.levelId == id){
                return item;
            }
        }
        return null;
    }
    public int GetLevelCount(){
        return levels.Count;
    }
    
}

[System.Serializable]
public class LevelItem{
    public int levelId;
    public LevelType levelType;
    public float levelTime;
    public EnemyArray[] levelEnemy;
    public float rewardTime;
    public EnemyArray[] rewardEnemy;
}

[System.Serializable]
public struct EnemyCount
{
    public GameObject type;
    public int count;
}
[System.Serializable]
public struct EnemyArray
{
    public EnemyCount[] enemies;
    public float startTime;
}
public enum LevelType{
    Normal,
    Boss
}