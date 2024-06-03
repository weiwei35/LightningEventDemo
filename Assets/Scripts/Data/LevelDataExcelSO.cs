using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataExcelSO", menuName = "LightningDemo/LevelDataExcel", order = 0)]
public class LevelDataExcelSO : ScriptableObject {
    public List<Level> levels = new List<Level>();
    public List<Level> GetLevelDataById(int id) {
        List<Level> levelById = new List<Level>();
        foreach (var item in levels)
        {
            if(item.levelId == id){
                levelById.Add(item);
            }
        }
        return levelById;
    }
    public int GetLevelCount(){
        int count = 0;
        List<int> levelIdRecord = new List<int>();
        foreach (var item in levels)
        {
            if(!levelIdRecord.Contains(item.levelId)){
                count++;
                levelIdRecord.Add(item.levelId);
            }
        }
        return count;
    }
}
public class Level{
    public int levelId;
    public float levelTime;
    public int enemyArrayCount;
    public float arrayStartTime;
    public int enemyId;
    public int enemyCount;
    public bool isReward;
}