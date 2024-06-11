using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global{
    public static bool isSlowDown = false;
}
public class GameController : MonoBehaviour
{
    public EnemyPoolController enemyPool;
    public LevelDataSO levelData;
    [HideInInspector]
    public LevelItem level;
    public LevelUIController levelUI;
    SelectItemUI selectPanel;
    [HideInInspector]
    public int levelId = 1;
    float levelTime;
    float rewardTime;
    float timeCur = 0;
    [HideInInspector]
    public bool isReward = false;
    public GameObject endLevelPanel;
    // Start is called before the first frame update
    void Start()
    {
        level = levelData.GetLevelDataById(levelId);
        levelTime = level.levelTime;
        rewardTime = level.rewardTime;
        enemyPool.SetLevel(levelId);
        // enemyPool.SetLevelEnemy();
        enemyPool.SetEnemyArray();
        selectPanel = endLevelPanel.GetComponent<SelectItemUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isReward){
            timeCur += Time.deltaTime;
            rewardTime = level.rewardTime;
            if(timeCur >= rewardTime){
                //奖励关卡结束下一关
                enemyPool.DestroyAllEnemy();
                isReward = false;
                timeCur = 0;
                levelId ++;
                Time.timeScale = 0;
                endLevelPanel.SetActive(true);
                // SetLevel();
            }
        }else{
            timeCur += Time.deltaTime;
            if(timeCur >= (levelTime)){
                //结束关卡，进入奖励关卡
                enemyPool.SetRewardEnemyArray();
                isReward = true;
                timeCur = 0;
            }
        }
    }

    public void SetLevel(){
        if(selectPanel.SetPlayerStatus()){
            levelUI.SetLevelPlayer();
            Time.timeScale = 1;
            if(endLevelPanel.activeSelf){
                endLevelPanel.SetActive(false);
            }
            if(levelId > levelData.GetLevelCount()){
                Time.timeScale = 0;
                Application.Quit();
            }else{
                level = levelData.GetLevelDataById(levelId);
                levelTime = level.levelTime;
                rewardTime = level.rewardTime;
                enemyPool.SetLevel(levelId);
                enemyPool.SetEnemyArray();
            }
        }
    }
}