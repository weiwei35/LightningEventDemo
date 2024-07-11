using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public static class Global{
    public static float exp = 0;
    public static int exp_level = 0;
    public static float exp_max = 0;
    public static bool isSlowDown = false;
    public static bool isEndBoss = false;
    public static bool isReward = false;
    public static List<Vector3> papersPosList = new List<Vector3>();
    public static List<GameObject> playerCopyList = new List<GameObject>();
    public static List<SelectItem> item1Current = new List<SelectItem>();
    public static List<SelectItem> item2Current = new List<SelectItem>();
    public static List<SelectItem> item3Current = new List<SelectItem>();
}
public class GameController : MonoBehaviour
{
    public List<float> expList = new List<float>();
    public ExpLevelDataSO expLevelData;
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
    public TMP_Text expText;
    public Slider sliderExp;
    GameObject papers;
    bool isLevelUp = false;

    public GameObject bgChangeLevel;
    // Start is called before the first frame update
    private void Awake() {
        bgChangeLevel.SetActive(false);
        level = levelData.GetLevelDataById(levelId);
        levelTime = level.levelTime;
        rewardTime = level.rewardTime;
        enemyPool.SetLevel(levelId);
        // enemyPool.SetLevelEnemy();
        enemyPool.SetEnemyArray();
        // Global.exp_max = expList[Global.exp_level];
        if(Global.exp_level < expLevelData.GetLevelCount()){
            Global.exp_max = expLevelData.GetExpMaxByLevel(Global.exp_level);
        }else{
            Global.exp_max = expLevelData.GetExpMaxByLevel(expLevelData.GetLevelCount() - 1);
        }
    }
    void Start()
    {
        selectPanel = endLevelPanel.GetComponent<SelectItemUI>();
        papers = GameObject.FindGameObjectWithTag("Papers");

        Time.timeScale = 0;
        endLevelPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        expText.text = Global.exp.ToString();
        sliderExp.value = Global.exp/Global.exp_max;
        if(Global.exp >= Global.exp_max){
            //升级
            Global.exp_level++;
            Global.exp = Global.exp - Global.exp_max;
            if(Global.exp_level < expLevelData.GetLevelCount()){
                Global.exp_max = expLevelData.GetExpMaxByLevel(Global.exp_level);
            }else{
                Global.exp_max = expLevelData.GetExpMaxByLevel(expLevelData.GetLevelCount() - 1);
            }
            isLevelUp = true;
        }
        if(isLevelUp && !Global.isSlowDown){
            isLevelUp = false;

            Time.timeScale = 0;
            endLevelPanel.SetActive(true);
        }
        if(level.levelType == LevelType.Normal){
            if(isReward){
                timeCur += Time.deltaTime;
                rewardTime = level.rewardTime;
                if(timeCur >= rewardTime){
                    //奖励关卡结束下一关
                    Global.isReward = false;
                    NextLevel();
                }
            }else{
                timeCur += Time.deltaTime;
                if(timeCur >= levelTime){
                    if(rewardTime > 0){
                        Global.isReward = true;
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
        SetLevelItem();
        levelId ++;
        Global.papersPosList.Clear();
        foreach (Transform item in papers.transform)
        {
            PaperController paper = item.GetComponent<PaperController>();
            if(paper!= null)
            {
                paper.DestroyChild();
            }
        }
        bgChangeLevel.SetActive(true);
        Invoke("SetLevel",2);
    }
    void SetLevelItem(){
        Time.timeScale = 0;
        endLevelPanel.SetActive(true);
    }

    public void SetLevel(){
        bgChangeLevel.SetActive(false);

        if(levelId > levelData.GetLevelCount()){
            // Time.timeScale = 0;
            // Application.Quit();
            bgChangeLevel.SetActive(true);
            SceneManager.LoadSceneAsync("UIScene");
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
    public void SetItem(){
        if(selectPanel.SetPlayerStatus()){
            levelUI.SetLevelPlayer();
            Time.timeScale = 1;
            if(endLevelPanel.activeSelf){
                endLevelPanel.SetActive(false);
            }
        }
    }
}