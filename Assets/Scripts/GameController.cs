using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global{
    public static bool isSlowDown = false;
    public static bool isEndBoss = false;
    public static List<Vector3> papersPosList = new List<Vector3>();
    public static List<GameObject> playerCopyList = new List<GameObject>();
    public static List<SelectItem> item1Current = new List<SelectItem>();
    public static List<SelectItem> item2Current = new List<SelectItem>();
    public static List<SelectItem> item3Current = new List<SelectItem>();
}
public class GameController : MonoBehaviour
{
    public EnemyPoolController enemyPool;
    // public LevelDataSO levelData;
    public LevelDataExcelSO levelData;
    [HideInInspector]
    // public LevelItem level;
    public Level level;
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
    public GameObject rewardTitle;
    GameObject papers;
    // Start is called before the first frame update
    private void Awake() {
        level = levelData.GetLevelDataById(levelId);
        levelTime = level.levelTime;
        rewardTime = level.rewardTime;
        enemyPool.SetLevel(levelId);
        // enemyPool.SetLevelEnemy();
        enemyPool.SetEnemyArray();
    }
    void Start()
    {
        selectPanel = endLevelPanel.GetComponent<SelectItemUI>();
        papers = GameObject.FindGameObjectWithTag("Papers");
    }

    // Update is called once per frame
    void Update()
    {
        if(level.levelType == LevelType.Normal){
            if(isReward){
                timeCur += Time.deltaTime;
                rewardTime = level.rewardTime;
                if(timeCur >= rewardTime){
                    //奖励关卡结束下一关
                    NextLevel();
                }
            }else{
                timeCur += Time.deltaTime;
                if(timeCur >= levelTime){
                    if(rewardTime > 0){
                        //结束关卡，进入奖励关卡
                        rewardTitle.SetActive(true);
                        enemyPool.SetRewardEnemyArray();
                        isReward = true;
                        timeCur = 0;
                    }else{
                        NextLevel();
                    }
                    
                }
            }
        }
        else if(level.levelType == LevelType.Boss) {
            if(Global.isEndBoss){
                NextLevel();
            }
        }
    }

    public void NextLevel() {
        Global.isEndBoss = false;
        rewardTitle.SetActive(false);
        enemyPool.DestroyAllEnemy();
        isReward = false;
        timeCur = 0;
        levelId ++;
        Time.timeScale = 0;
        endLevelPanel.SetActive(true);
        Global.papersPosList.Clear();
        foreach (Transform item in papers.transform)
        {
            PaperController paper = item.GetComponent<PaperController>();
            if(paper!= null)
            {
                paper.DestroyChild();
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
                foreach (Transform item in papers.transform)
                {
                    PaperController paper = item.GetComponent<PaperController>();
                    if(paper!= null)
                    {
                        paper.countCurrent = paper.count;
                    }
                }
            }
        }
    }
}