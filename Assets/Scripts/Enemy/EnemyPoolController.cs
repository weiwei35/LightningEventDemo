using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyPoolController : MonoBehaviour
{
    public LevelDataSO levelData;
    LevelItem level;
    public void SetLevel(int levelId) {
        level = levelData.GetLevelDataById(levelId);
    }

    public void SetLevelEnemy() {
        StartCoroutine(LevelEnemy());
    }
    IEnumerator LevelEnemy(){
        yield return new WaitForSeconds(3);
        foreach(var item in level.levelEnemy){
            for (int i = 0; i < item.enemies[i].count; i++)
            {
                var enemyNew = Instantiate(item.enemies[i].type);
                enemyNew.transform.parent = transform;
                enemyNew.transform.position = transform.position;
            }
        }
    }
    public void SetEnemyArray(){
        for(int i = 0; i < level.levelEnemy.Count(); i++) {
            StartCoroutine(InitEnemyArray(level.levelEnemy[i].startTime,level.levelEnemy[i].enemies));
        }
    }
    IEnumerator InitEnemyArray(float time,EnemyCount[] enemies){
        yield return new WaitForSeconds(time);
        for (int j = 0; j < enemies.Count(); j++)
        {
            for (int i = 0; i < enemies[j].count; i++)
            {
                var enemyNew = Instantiate(enemies[j].type);
                enemyNew.transform.parent = transform;
                enemyNew.transform.position = transform.position;
            }
        }
    }
    public void SetRewardEnemyArray(){
        for(int i = 0; i < level.rewardEnemy.Count(); i++) {
            StartCoroutine(InitEnemyArray(level.rewardEnemy[i].startTime,level.rewardEnemy[i].enemies));
        }
    }

    public void SetRewardEnemy() {
        StartCoroutine(RewardEnemy());
    }
    IEnumerator RewardEnemy(){
        yield return new WaitForSeconds(3);
        foreach(var item in level.rewardEnemy){
            for (int i = 0; i < item.enemies[i].count; i++)
            {
                var enemyNew = Instantiate(item.enemies[i].type);
                enemyNew.transform.parent = transform;
                enemyNew.transform.position = transform.position;
            }
        }
    }

    public void DestroyAllEnemy() {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
            // Debug.Log(item.gameObject.name); 
        }
    }

    public int GetAllEnemyCount() {
        int i = 0;
        foreach (Transform item in transform)
        {
            i++;
        }
        return i;
    }
}
