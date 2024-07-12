using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        DOTween.SetTweensCapacity(800,200);
        selectPanel = endLevelPanel.GetComponent<SelectItemUI>();
        Global.exp = 0;
        if(Global.continueGame){
            LoadData();
            selectPanel.LoadItem();
        }
        bgChangeLevel.SetActive(false);
        level = levelData.GetLevelDataById(levelId);
        levelTime = level.levelTime;
        rewardTime = level.rewardTime;
        enemyPool.SetLevel(levelId);
        // enemyPool.SetLevelEnemy();
        enemyPool.SetEnemyArray();
        if(Global.exp_level < expLevelData.GetLevelCount()){
            Global.exp_max = expLevelData.GetExpMaxByLevel(Global.exp_level);
        }else{
            Global.exp_max = expLevelData.GetExpMaxByLevel(expLevelData.GetLevelCount() - 1);
        }

        GameObject[] gameObjects = getDontDestroyOnLoadGameObjects();
        foreach (var item in gameObjects)
        {
            if(item.layer == 19)
                Destroy(item);
        }
    }
    private GameObject[] getDontDestroyOnLoadGameObjects()
    {
        var allGameObjects = new List<GameObject>();
        allGameObjects.AddRange(FindObjectsOfType<GameObject>());
        //移除所有场景包含的对象
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            for (var j = 0; j < objs.Length; j++)
            {
                allGameObjects.Remove(objs[j]);
            }
        }
        //移除父级不为null的对象
        int k = allGameObjects.Count;
        while (--k >= 0)
        {
            if (allGameObjects[k].transform.parent != null)
            {
                allGameObjects.RemoveAt(k);
            }
        }
        return allGameObjects.ToArray();
    }
    void Start()
    {
        papers = GameObject.FindGameObjectWithTag("Papers");

        if(levelId == 1){
            Time.timeScale = 0;
            endLevelPanel.SetActive(true);
        }
        if(Global.isEndLevel){
            NextLevel();
        }
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
        SaveEndtData();
    }

    public void SetLevel(){
        bgChangeLevel.SetActive(false);

        if(levelId > levelData.GetLevelCount()){
            // Time.timeScale = 0;
            // Application.Quit();
            bgChangeLevel.SetActive(true);
            SceneManager.LoadSceneAsync("UIScene");
        }else{
            SaveStartData();
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

    public GameSaveManager gameSave;
    public PlayerController player;
    public LightningController lightning;
    void SaveStartData(){
        gameSave.data.levelId = levelId;
        gameSave.data.isEndLevel = false;

        gameSave.data.speed = player.moveSpeed;
        gameSave.data.HP = player.HP;
        gameSave.data.protect = player.protect;
        gameSave.data.HPSpeed = player.HPSpeed;
        gameSave.data.protectSpeed = player.protectSpeed;

        gameSave.data.lightningTime = lightning.lightningTime;
        gameSave.data.lightningCount = lightning.lightningCount;
        gameSave.data.lightningHurt = lightning.lightningHurt;

        gameSave.data.exp = Global.exp;
        gameSave.data.exp_level = Global.exp_level;

        gameSave.Save();
    }
    void SaveEndtData(){
        SaveStartData();
        gameSave.data.isEndLevel = true;
        gameSave.Save();
    }

    void LoadData(){
        gameSave.Load();
        levelId = gameSave.data.levelId;
        Global.isEndLevel = gameSave.data.isEndLevel;

        player.moveSpeed = gameSave.data.speed;
        player.HP = gameSave.data.HP;
        player.protect = gameSave.data.protect;
        player.HPSpeed = gameSave.data.HPSpeed;
        player.protectSpeed = gameSave.data.protectSpeed;

        lightning.lightningTime = gameSave.data.lightningTime;
        lightning.lightningCount = gameSave.data.lightningCount;
        lightning.lightningHurt = gameSave.data.lightningHurt;

        Global.exp = gameSave.data.exp;
        Global.exp_level = gameSave.data.exp_level;
    }
}