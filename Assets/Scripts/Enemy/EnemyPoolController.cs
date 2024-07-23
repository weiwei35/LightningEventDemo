using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

/*
关卡，固定时长结束，分为“杀敌”“奖励”两部分
1.敌人分3等级，等级1怪物刷新x波刷新一波等级2怪物，等级1怪物刷新y波刷新一波等级3怪物
2.每等级怪包含若干怪物组合，每波刷新时在组合库中随机一组
*/
public class EnemyPoolController : MonoBehaviour
{
    // public LevelDataSO levelData;
    // LevelItem level;
    public float enemyInitTime = 10;
    public EnemyGroupDataSO enemyGroup;
    int level2Count = 3;
    int level3Count = 4;
    int level4Count = 5;
    int level5Count = 6;
    int level6Count = 7;
    int level7Count = 8;
    int level8Count = 9;
    float enemyTimeCount = 0;
    int level1Count = 0;
    bool canCount = false;
    bool triggered = false;
    public LevelDataExcelSO levelData;
    Level level;
    public void SetLevel(int levelId) {
        level = levelData.GetLevelDataById(levelId);
        
        level2Count = enemyGroup.GetCountById(levelId,1);
        level3Count = enemyGroup.GetCountById(levelId,2);
        level4Count = enemyGroup.GetCountById(levelId,3);
        level5Count = enemyGroup.GetCountById(levelId,4);
        level6Count = enemyGroup.GetCountById(levelId,5);
        level7Count = enemyGroup.GetCountById(levelId,6);
        level8Count = enemyGroup.GetCountById(levelId,7);
    }
    // public void SetEnemyArray(){
    //     for(int i = 0; i < level.levelEnemy.Count(); i++) {
    //         StartCoroutine(InitEnemyArray(level.levelEnemy[i].startTime,level.levelEnemy[i].enemies));
    //     }
    // }
    // public void SetRewardEnemyArray(){
    //     for(int i = 0; i < level.rewardEnemy.Count(); i++) {
    //         StartCoroutine(InitEnemyArray(level.rewardEnemy[i].startTime,level.rewardEnemy[i].enemies));
    //     }
    // }
    // IEnumerator InitEnemyArray(float time,EnemyCount[] enemies){
    //     yield return new WaitForSeconds(time);
    //     for (int j = 0; j < enemies.Count(); j++)
    //     {
    //         for (int i = 0; i < enemies[j].count; i++)
    //         {
    //             var enemyNew = Instantiate(enemies[j].type);
    //             enemyNew.transform.parent = transform;
    //             enemyNew.transform.position = transform.position;
    //         }
    //     }
    // }
    private void Start() {
    }
    private void Update() {
        if(canCount)
            enemyTimeCount += Time.deltaTime;
        if(enemyTimeCount >= enemyInitTime){
            CheckLevel();
        }
    }
    void CheckLevel(){
        SetLevel1Once();
        enemyTimeCount = 0;
        if(level2Count > 0){
            if(level1Count % level2Count == 0 && level.enemyLevel2Group.count>0 && level1Count!=0 && !triggered){
                SetLevel2Once();
            }
        }
        if(level3Count > 0){
            if(level1Count % level3Count == 0 && level.enemyLevel3Group.count>0 && level1Count!=0 && !triggered){
                SetLevel3Once();
            }
        }
        if(level4Count > 0){
            if(level1Count % level4Count == 0 && level.enemyLevel4Group.count>0 && level1Count!=0 && !triggered){
                SetLevel4Once();
            }
        }
        if(level5Count > 0){
            if(level1Count % level5Count == 0 && level.enemyLevel5Group.count>0 && level1Count!=0 && !triggered){
                SetLevel5Once();
            }
        }
        if(level6Count > 0){
            if(level1Count % level6Count == 0 && level.enemyLevel6Group.count>0 && level1Count!=0 && !triggered){
                SetLevel6Once();
            }
        }
        if(level7Count > 0){
            if(level1Count % level7Count == 0 && level.enemyLevel7Group.count>0 && level1Count!=0 && !triggered){
                SetLevel7Once();
            }
        }
        if(level8Count > 0){
            if(level1Count % level8Count == 0 && level.enemyLevel8Group.count>0 && level1Count!=0 && !triggered){
                SetLevel8Once();
            }
        }
    }

    //初始化关卡怪物每10s生成1组怪
    public void SetEnemyArray(){
        canCount = true;
        level1Count = 0;

        if(level.levelType == LevelType.Boss){
            var enemyNew = Instantiate(level.boss);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
        }
    }
    void SetLevel1Once(){
        level1Count ++;
        triggered = false;
        for (int i = 0; i < level.enemyLevel1Group.count; i++)
        {
            var enemyNew = Instantiate(level.enemyLevel1Group.type);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
            EnemyController enemy = enemyNew.GetComponent<EnemyController>();
            enemy.startPos = level1Count-1;
        }
    }
    void SetLevel2Once(){
        triggered = true;
        for (int i = 0; i < level.enemyLevel2Group.count; i++)
        {
            var enemyNew = Instantiate(level.enemyLevel2Group.type);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
            EnemyController enemy = enemyNew.GetComponent<EnemyController>();
            enemy.startPos = level1Count-1;
        }
    }
    void SetLevel3Once(){
        triggered = true;
        for (int i = 0; i < level.enemyLevel3Group.count; i++)
        {
            var enemyNew = Instantiate(level.enemyLevel3Group.type);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
            EnemyController enemy = enemyNew.GetComponent<EnemyController>();
            enemy.startPos = level1Count-1;
        }
    }
    void SetLevel4Once(){
        triggered = true;
        for (int i = 0; i < level.enemyLevel4Group.count; i++)
        {
            var enemyNew = Instantiate(level.enemyLevel4Group.type);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
            EnemyController enemy = enemyNew.GetComponent<EnemyController>();
            enemy.startPos = level1Count-1;
        }
    }
    void SetLevel5Once(){
        triggered = true;
        for (int i = 0; i < level.enemyLevel5Group.count; i++)
        {
            var enemyNew = Instantiate(level.enemyLevel5Group.type);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
            EnemyController enemy = enemyNew.GetComponent<EnemyController>();
            enemy.startPos = level1Count-1;
        }
    }
    void SetLevel6Once(){
        triggered = true;
        for (int i = 0; i < level.enemyLevel6Group.count; i++)
        {
            var enemyNew = Instantiate(level.enemyLevel6Group.type);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
            EnemyController enemy = enemyNew.GetComponent<EnemyController>();
            enemy.startPos = level1Count-1;
        }
    }
    void SetLevel7Once(){
        triggered = true;
        for (int i = 0; i < level.enemyLevel7Group.count; i++)
        {
            var enemyNew = Instantiate(level.enemyLevel7Group.type);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
            EnemyController enemy = enemyNew.GetComponent<EnemyController>();
            enemy.startPos = level1Count-1;
        }
    }
    void SetLevel8Once(){
        triggered = true;
        for (int i = 0; i < level.enemyLevel8Group.count; i++)
        {
            var enemyNew = Instantiate(level.enemyLevel8Group.type);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
            EnemyController enemy = enemyNew.GetComponent<EnemyController>();
            enemy.startPos = level1Count-1;
        }
    }
    //设置奖励组怪物
    public void SetRewardEnemyArray(){
        canCount = false;
        for (int i = 0; i < level.rewardGroupId.count; i++)
        {
            var enemyNew = Instantiate(level.rewardGroupId.type);
            enemyNew.transform.parent = transform;
            enemyNew.transform.position = transform.position;
        }
    }
    public GameObject center;
    List<GameObject> enemyArray = new List<GameObject>();
    int i = 0;
    public void DestroyAllEnemy() {
        i=0;
        
        foreach (Transform item in transform)
        {
            // Destroy(item.gameObject);
            enemyArray.Add(item.gameObject);
            float time = Vector3.Distance(item.transform.position,center.transform.position) * 0.1f;
            StartCoroutine(DestroyEnemy(item.gameObject,time));
        }
    }
    IEnumerator DestroyEnemy(GameObject enemy,float time){
        yield return new WaitForSeconds(time);
        if(enemy.GetComponent<EnemyController>() != null)
            enemy.GetComponent<EnemyController>().anim.SetTrigger("dead");
        StartCoroutine(SetDestroy(enemy));
    }
    IEnumerator SetDestroy(GameObject gameObject){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
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
