using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public ExpLevelDataSO expLevelData;
    // public EnemyPoolController enemyPool;
    // public LevelDataSO levelData;
    public LevelEnemyController levelEnemyController;
    // public LevelDataExcelSO levelData;
    // public LevelItem level;
    // public Level level;
    public LevelDataSO levelData;
    LevelData level;
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
    public TMP_Text expLevelText;
    public TMP_Text expLevelTextPanel;
    public Slider sliderExp;
    GameObject papers;
    // bool isLevelUp = false;

    public GameObject bgChangeLevel;
    public Image levelImg;
    public GameObject level1Level;
    public Image level1Img;
    public List<Sprite> levelSprite = new List<Sprite>();

    bool startCheckingEnemy = false;
    [Header("圆心")]
    public GameObject centerPos;

    public GameObject bulletFather;
    public AudioSource levelBGM;
    public AudioClip[] levelBGMClips;
    public AudioClip[] levelBGMLoopClips;
    public AudioSource levelStart;
    public AudioSource levelBack;
    public CirclePanelController circlePanel;
    public bool isBossLevel;
    public GameObject startBtn;
    public GameObject nextBtn;
    public GameObject refreshBtn;
    // Start is called before the first frame update
    private void Awake() {
        Application.targetFrameRate = 60;
        DOTween.SetTweensCapacity(800,200);
        selectPanel = endLevelPanel.GetComponent<SelectItemUI>();
        Global.exp = 0;
        Global.exp_level = 0;
        Global.GameBegain = false;
        Global.isGameOver = false;
        levelId = 1;
        if(Global.continueGame){
            if(LoadData())
                selectPanel.LoadItem();
                startBtn.SetActive(false);
                nextBtn.SetActive(true);
                refreshBtn.SetActive(true);
            if(levelId == 0){
                levelId = 1;
                startBtn.SetActive(true);
                nextBtn.SetActive(false);
                refreshBtn.SetActive(false);
            }
        }
        // bgChangeLevel.SetActive(false);
        level1Level.SetActive(true);
        levelStart.Play();
        if(levelId < 6){
            levelBGM.clip = levelBGMClips[0];
            levelBGM.Play();
        }else if(levelId > 5 && levelId < 11){
            levelBGM.clip = levelBGMClips[1];
            levelBGM.volume = 0.5f;
            levelBGM.Play();
        }else if(levelId > 10 && levelId < 16){
            levelBGM.clip = levelBGMClips[2];
            levelBGM.volume = 0.5f;
            levelBGM.Play();
        }
        levelBack.Play();
        level1Img.gameObject.SetActive(true);
        level1Img.sprite = levelSprite[levelId-1];
        player.transform.position = centerPos.transform.position;

        level = levelData.GetLevelById(levelId);
        if(level.type != 2){
            isBossLevel = false;
        levelTime = levelData.GetLevelTimeById(levelId);
        rewardTime = levelData.GetRewardTimeById(levelId);
        }else{
            isBossLevel = true;
        }
        // enemyPool.SetLevel(levelId);
        // enemyPool.SetLevelEnemy();
        // enemyPool.SetEnemyArray();
        levelEnemyController.SetLevel(levelId);
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
        SaveStartData();
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
            endLevelPanel.SetActive(true);
            startBtn.SetActive(true);
            nextBtn.SetActive(false);
            refreshBtn.SetActive(false);
            // Invoke("SetTimeStop",0.5f);
        }
        if(Global.isEndLevel){
            NextLevel();
        }
    }

    void SetTimeStop(){
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Global.levelCount = levelId - 1;
        expText.text = Global.exp.ToString();
        expLevelText.text = Global.exp_sum.ToString();
        expLevelTextPanel.text = Global.exp_sum.ToString();
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
            // isLevelUp = true;
        }
        // if(isLevelUp && !Global.isSlowDown && !Global.isGameOver){
        //     isLevelUp = false;
        //     SetTimeStop();
        //     endLevelPanel.SetActive(true);
        // }
        if(level.type == 0){
            if(isReward){
                if(!Global.isSlowDown)
                    timeCur += Time.deltaTime;
                if(timeCur >= rewardTime && !Global.isSlowDown && !endLevelPanel.activeInHierarchy){
                    //奖励关卡结束下一关
                    Global.isReward = false;
                    NextLevel();
                }
            }else{
                if(!Global.isSlowDown && Global.GameBegain)
                    timeCur += Time.deltaTime;
                if(timeCur >= levelTime){
                    if(rewardTime > 0){
                        Global.isReward = true;
                        //结束关卡，进入奖励关卡
                        // rewardTitle.SetActive(true);
                        // enemyPool.SetRewardEnemyArray();
                        levelEnemyController.SetRewardLevel(levelId);
                        isReward = true;
                        timeCur = 0;
                    }else if(!Global.isSlowDown && !endLevelPanel.activeInHierarchy){
                        NextLevel();
                    }
                    
                }
            }
        }
        else if(level.type == 2) {
            if(Global.isEndBoss && !Global.isSlowDown && !endLevelPanel.activeInHierarchy){
                NextLevel();
            }
        }

        if(Global.isGameOver){
            gameSave.Delet();
        }

        if(startCheckingEnemy){
            if(levelEnemyController.GetAllEnemyCount() == 0){
                startCheckingEnemy = false;
                // Invoke("SetNextLevel",3);
                StartCoroutine(SetNextLevel());
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            Time.timeScale = 0;
            // bgChangeLevel.SetActive(true);
            // levelImg.gameObject.SetActive(false);
            // SceneManager.LoadSceneAsync("UIScene");
            selectPanel.isStopGame = true;
            endLevelPanel.SetActive(true);
        }

        if(levelBGM.isPlaying == false){
            if(levelId < 6){
                levelBGM.clip = levelBGMLoopClips[0];
                levelBGM.volume = 0.5f;
                levelBGM.loop = true;
                levelBGM.Play();
            }else if(levelId > 5 && levelId < 11){
                levelBGM.clip = levelBGMLoopClips[1];
                levelBGM.volume = 0.5f;
                levelBGM.loop = true;
                levelBGM.Play();
            }else if(levelId > 10 && levelId < 16){
                levelBGM.clip = levelBGMLoopClips[2];
                levelBGM.volume = 0.5f;
                levelBGM.loop = true;
                levelBGM.Play();
            }
        }

        if(Global.isGameOver){
            BulletController1 bulletController = bulletFather.GetComponent<BulletController1>();
            bulletController.DestroyAllBullets();
            levelEnemyController.DestroyAllEnemy();
            isReward = false;
            Global.papersPosList.Clear();
            foreach (Transform item in papers.transform)
            {
                PaperController paper = item.GetComponent<PaperController>();
                if(paper!= null)
                {
                    paper.DestroyChild();
                }
            }
            GameObject[] playerOnceCopy = GameObject.FindGameObjectsWithTag("PlayerOnceCopy");
            if(playerOnceCopy.Length > 0){
                foreach (var item in playerOnceCopy)
                {
                    Destroy(item);
                }
            }
            DOTween.To(()=>levelBGM.volume, x =>levelBGM.volume = x,0,1);
        }
    }

    public void NextLevel() {
        timeCur = 0;
        Global.isChangeLevel = true;
        Global.isEndBoss = false;
        rewardTitle.SetActive(false);
        // enemyPool.DestroyAllEnemy();
        BulletController1 bulletController = bulletFather.GetComponent<BulletController1>();
        bulletController.DestroyAllBullets();
        levelEnemyController.DestroyAllEnemy();
        isReward = false;
        Global.papersPosList.Clear();
        foreach (Transform item in papers.transform)
        {
            PaperController paper = item.GetComponent<PaperController>();
            if(paper!= null)
            {
                paper.DestroyChild();
            }
            if(item.gameObject.layer == 15){
                Destroy(item.gameObject);
            }
        }
        GameObject[] playerOnceCopy = GameObject.FindGameObjectsWithTag("PlayerOnceCopy");
        if(playerOnceCopy.Length > 0){
            foreach (var item in playerOnceCopy)
            {
                Destroy(item);
            }
        }
        DOTween.To(()=>levelBGM.volume, x =>levelBGM.volume = x,0,1);
        startCheckingEnemy = true;
        Global.lightningCount = 0;
    }
    IEnumerator SetNextLevel(){
        yield return new WaitForSeconds(0.5f);
        timeCur = 0;
        SetLevelItem();
        // levelId ++;
        // bgChangeLevel.SetActive(true);
        // levelImg.gameObject.SetActive(true);
        // levelImg.sprite = levelSprite[levelId-1];
        // player.transform.position = centerPos.transform.position;
        // Invoke("SetLevel",3.5f);
        // circlePanel.ResetCircle();
    }
    void SetLevelItem(){
        startBtn.SetActive(false);
        nextBtn.SetActive(true);
        refreshBtn.SetActive(true);
        endLevelPanel.SetActive(true);
        Invoke("SetTimeStop",0.5f);
        SaveEndtData();
    }
    void PlayLevelSound(){
        levelStart.Play();
    }
    public void StopLevelSound(){
        levelBGM.Stop();
        levelBack.Stop();
    }
    public GameObject blackBG;
    public GameObject successPanel;
    public GameObject failPanel;
    public GameObject canvas;
    public void SetLevel(){
        foreach (Transform item in bulletFather.transform)
        {
            Destroy(item.gameObject);
        }
        bgChangeLevel.SetActive(false);

        if(levelId > levelData.GetLevelCount()){
            // Time.timeScale = 0;
            // Application.Quit();
            Global.isGameOver = true;
            // bgChangeLevel.SetActive(true);
            levelImg.gameObject.SetActive(false);
            // SceneManager.LoadSceneAsync("UIScene");
            StopLevelSound(); 
            blackBG.SetActive(true);
            canvas.SetActive(true);
            failPanel.SetActive(false);
            successPanel.SetActive(true);
        }else{
            SaveStartData();
            if(player.isOnceTimeCopy)
                player.isTimeCopied = false;
            if(player.isOnceLightningCopy)
                player.isLightCopied = false;
            level = levelData.GetLevelById(levelId);
            
            if(level.type != 2){
                isBossLevel = false;
                levelTime = levelData.GetLevelTimeById(levelId);
                rewardTime = levelData.GetRewardTimeById(levelId);
            }else{
                isBossLevel = true;
            }
            // enemyPool.SetLevel(levelId);
            // enemyPool.SetEnemyArray();
            levelEnemyController.SetLevel(levelId);
            foreach (Transform item in papers.transform)
            {
                PaperController paper = item.GetComponent<PaperController>();
                if(paper!= null)
                {
                    paper.DestroyChild();
                    paper.countCurrent = paper.count;
                }
            }
            if(levelId < 6){
                levelBGM.clip = levelBGMClips[0];
                levelBGM.volume = 0.5f;
                levelBGM.Play();
            }else if(levelId > 5 && levelId < 11){
                levelBGM.clip = levelBGMClips[1];
                levelBGM.volume = 0.5f;
                levelBGM.Play();
            }else if(levelId > 10 && levelId < 16){
                levelBGM.clip = levelBGMClips[2];
                levelBGM.volume = 0.5f;
                levelBGM.Play();
            }
            levelBGM.loop = false;
            
            levelBack.Play();
        }
    }
    public Animator panelAnim;
    public void SetStartItem(){
        // if(selectPanel.SetPlayerStatus()){
            Time.timeScale = 1;
            if(!Global.GameBegain)
                Global.GameBegain = true;
            Animator anim = selectPanel.GetComponent<Animator>();
            anim.SetTrigger("close");
            panelAnim.SetTrigger("close");
            levelUI.SetLevelPlayer();
            Invoke("CloseEndPanel",1f);
        // }
    }
    public void SetItem(){
        Time.timeScale = 1;
        if(!Global.GameBegain)
            Global.GameBegain = true;
        Animator anim = selectPanel.GetComponent<Animator>();
        anim.SetTrigger("close");
        panelAnim.SetTrigger("close");
        levelUI.SetLevelPlayer();
        Invoke("CloseEndPanel",1f);
        levelId ++;
        bgChangeLevel.SetActive(true);
        levelImg.gameObject.SetActive(true);
        levelImg.sprite = levelSprite[levelId-1];
        player.transform.position = centerPos.transform.position;
        Invoke("SetLevel",3.5f);
        if(circlePanel.enabled)
            circlePanel.ResetCircle();
    }

    void CloseEndPanel(){
        Global.isChangeLevel = false;
        if(endLevelPanel.activeSelf){
            endLevelPanel.SetActive(false);
        }
        PlayLevelSound();
    }
    public void Continue(){
        Time.timeScale = 1;
        Animator anim = selectPanel.GetComponent<Animator>();
        anim.SetTrigger("close");
        panelAnim.SetTrigger("close");
        Invoke("ClosePanel",1f);
    }
    void ClosePanel(){
        if(endLevelPanel.activeSelf){
            endLevelPanel.SetActive(false);
        }
    }
    public void BackMenu(){
        Time.timeScale = 1;
        bgChangeLevel.SetActive(true);
        levelImg.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("UIScene");
    }

    public GameSaveManager gameSave;
    public PlayerController player;
    public LightningController lightning;
    void SaveStartData(){
        gameSave.data.levelId = levelId;
        gameSave.data.isEndLevel = false;

        gameSave.data.heroId = player.heroId;
        gameSave.data.speed = player.moveSpeed;
        gameSave.data.HP = player.HP;
        gameSave.data.protect = player.protect;
        gameSave.data.HPSpeed = player.HPSpeed;
        gameSave.data.protectSpeed = player.protectSpeed;

        gameSave.data.lightningTime = lightning.lightningTime;
        gameSave.data.lightningTimeOriginal = lightning.lightningTimeOriginal;
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

    bool LoadData(){
        if(gameSave.Load()){
            levelId = gameSave.data.levelId;
            Global.isEndLevel = gameSave.data.isEndLevel;

            player.heroId = gameSave.data.heroId;
            player.moveSpeed = gameSave.data.speed;
            player.HP = gameSave.data.HP;
            player.protect = gameSave.data.protect;
            player.HPSpeed = gameSave.data.HPSpeed;
            player.protectSpeed = gameSave.data.protectSpeed;

            lightning.lightningTime = gameSave.data.lightningTime;
            lightning.lightningTimeOriginal = gameSave.data.lightningTimeOriginal;
            lightning.lightningCount = gameSave.data.lightningCount;
            lightning.lightningHurt = gameSave.data.lightningHurt;

            Global.exp = gameSave.data.exp;
            Global.exp_level = gameSave.data.exp_level;
            return true;
        }else{
            return false;
        }
    }
}