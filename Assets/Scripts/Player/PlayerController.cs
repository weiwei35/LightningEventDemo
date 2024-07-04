using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("移动速度")]
    public float moveSpeed = 5f;
    float speed = 5f;
    [Header("生命值")]
    public float HP = 100f;
    float HPCurrent;
    [Header("生命恢复速度(值/s)")]
    public float HPSpeed = 1f;
    bool needRecovery = false;
    bool startRecovery = false;
    [Header("UI")]
    public TMP_Text textHP;
    public TMP_Text textPro;
    public Slider sliderHP;
    public Slider sliderPro;
    [Header("受伤保护时长")]
    public float coldTime = 2f;
    [Header("护甲值")]
    public float protect = 100f;
    float protectCurrent = 100f;
    [Header("护甲恢复速度(值/s)")]
    public float protectSpeed = 1f;
    bool protectRecovery = false;
    bool startProtectRecovery = false;
    [Header("分身法宝1")]
    public bool isCircleCopy = false;
    public GameObject playerCopyPre;
    GameObject playerCopy;
    [Header("分身法宝2")]
    public bool isOnceLightningCopy = false;
    [HideInInspector]
    public float lightningCount = 0;
    public int lightningNum = 3;
    [HideInInspector]
    public bool isLightCopied = false;
    GameObject playerOnceCopy;
    public GameObject playerOnceCopyPre;
    [Header("分身法宝3")]
    public bool isOnceTimeCopy = false;
    [Header("分身法宝4")]
    public bool isMegaCopy = false;
    [HideInInspector]
    public bool isTimeCopied = false;
    [HideInInspector]
    public float copyTimeCount = 0;
    public float copyTimeNum = 3;
    [Header("雷电法宝1")]
    public bool isLightningMirror = false;
    [Header("雷电法宝2")]
    public bool isLightningBoom = false;
    [Header("雷电法宝3")]
    public bool isLightningOverflow = false;
    [Header("雷电法宝4")]
    public bool isLightningAttract = false;
    [Header("雷电法宝5")]
    public bool isLightningBoomPlayer = false;
    public GameObject boom;
    [Header("虫虫·1")]
    public GameObject petBugs;
    [HideInInspector]
    public List<BugController> bugs = new List<BugController>();
    public bool isBugHP = false;
    public BugController bugHPPre;
    public GameObject recoverAnim;
    public GameObject recoverProtectAnim;
    [Header("虫虫·2")]
    public bool isBugCircle = false;
    public BugController bugCirclePre;
    public CircleCollider bugCircleCollider;
    [Header("虫虫·3")]
    public bool isBugFollow = false;
    public BugController bugFollowPre;
    [Header("虫虫·4")]
    public bool isBugWall = false;
    public BugController bugWallPre;
    [Header("虫虫·5")]
    public bool isBugAttack = false;
    public BugController bugAttackPre;
    [Header("虫虫·6")]
    public bool isBugMerge = false;
    public BugController bugMergePre;
    bool isTriggerMerge = false;
    [Header("符箓·火球")]
    public GameObject papers;
    public bool isPaperFireBall = false;
    public PaperController paperFirePre;
    [Header("符箓·冰剑")]
    public bool isPaperIce = false;
    public PaperController paperIcePre;
    [Header("符箓·回血")]
    public bool isPaperHP = false;
    public PaperController paperHPPre;
    [Header("符箓·回护甲")]
    public bool isPaperProtect = false;
    public PaperController paperProtectPre;
    [Header("符箓·黑洞")]
    public bool isPaperBlack = false;
    public PaperController paperBlackPre;
    [Header("符箓·串联")]
    public bool isPaperConnect = false;
    [Header("Debuff·减速")]
    public bool isDebuffSlow = false;
    public GameObject moveSlow;
    [Header("Debuff·冰冻")]
    public bool isDebuffFreeze = false;
    public GameObject freezeBoom;
    [Header("Debuff·麻痹")]
    public bool isDebuffDizzy = false;
    [Header("Buff·冲击波")]
    public bool isBuffBoom = false;
    [Header("Buff·无敌")]
    public bool isBuffSuper = false;
    public GameObject superEffect;
    bool isSuper = false;
    public float superTime = 5;
    float superTimeCount = 0;
    [Header("瞬移")]
    public bool isMoveRandom = false;
    bool isMoving = false;
    public float moveTime = 5;
    float moveTimeCount = 0;
    [HideInInspector]
    public float moveX;
    [HideInInspector]
    public float moveY;
    Vector3 lastPos;
    PlayerAnimation anim;
    Rigidbody rb;
    [Header("其他引用")]
    public CircleController circle;
    bool ishitting = false;
    public Transform follow;
    [HideInInspector]
    public bool canHurt = false;
    public GameController gameController;
    [Header("死亡动画处理")]
    bool isDead = false;
    public GameObject blackBG;
    public Material blackM;
    public GameObject canvas;
    public GameObject enemyPool;
    public GameObject lightning;
    public GameObject effect;
    SpriteRenderer sprite;
    private void Start() {
        HPCurrent = HP;
        protectCurrent = protect;
        textHP.text = HPCurrent.ToString();
        textPro.text = protectCurrent.ToString();
        sliderHP.value = HPCurrent/HP;
        sliderPro.value = protectCurrent/protect;
        anim = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody>();
        speed = moveSpeed;
        sprite = GetComponent<SpriteRenderer>();
    }
 
    // Update is called once per frame
    private void FixedUpdate() {
        // Get input from the horizontal and vertical axes
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        // 根据玩家的输入设置角色的初速度
        Vector2 velocity = new Vector2(moveX, moveY);
        rb.velocity = velocity * moveSpeed;

        if(moveX > 0){
            // transform.localScale = new Vector3(-0.8f,0.8f,0.8f);
            sprite.flipX = true;
        }else if(moveX < 0){
            // transform.localScale = new Vector3(0.8f,0.8f,0.8f);
            sprite.flipX = false;
        }
    }

    private void Update() {
        //UI显示
        textHP.text = HPCurrent.ToString();
        textPro.text = protectCurrent.ToString();
        sliderHP.value = HPCurrent/HP;
        sliderPro.value = protectCurrent/protect;
        //速度处理
        if(Global.isSlowDown){
            moveSpeed = speed/20;
        }else{
            moveSpeed = speed;
        }

        //受伤状态判定
        if(!IsInCircle() && !gameController.isReward && !isSuper){
            canHurt = true;
        }else{
            canHurt = false;
        }
        //死亡判定
        if(HPCurrent <= 0){
            HPCurrent = 0.1f;
            isDead = true;
            Death();
        }
        //移动范围判定
        if(CheckPlayerInView()){
            lastPos = transform.position;
        }else{
            transform.position = lastPos;
        }

        //处理分身位置
        if(isCircleCopy && playerCopy != null){
            Vector3 centerPos = new Vector3(circle.centerPos.transform.position.x,circle.centerPos.transform.position.y,transform.position.z);
            Vector3 playerPos = GetSymmetricPosition(transform.position,centerPos);
            playerCopy.transform.position = new Vector3(playerPos.x,playerPos.y,transform.position.z);
        }else if(!isCircleCopy && playerCopy != null){
            Destroy(playerCopy);
        }

        //处理雷击间隔分身
        if(lightningCount >= lightningNum && !isLightCopied){
            lightningCount = 0;
            PlayerOnceLightCopy();
        }
        if(!isOnceLightningCopy)
            lightningCount = 0;

        //处理时间间隔分身
        if(isOnceTimeCopy)
            copyTimeCount += Time.deltaTime;
        else
            copyTimeCount = 0;
        if(copyTimeCount >= copyTimeNum && !isTimeCopied){
            copyTimeCount = 0;
            PlayerOnceTimeCopy();
        }

        //恢复生命数值
        if(HPCurrent < HP && !isDead){
            //需要恢复生命值
            needRecovery = true;
        }
        else if(HPCurrent >= HP){
            HPCurrent = HP;
            CancelInvoke("RecoveryHP");
            needRecovery = false;
        }
        if(needRecovery && !startRecovery){
            startRecovery = true;
            InvokeRepeating("RecoveryHP",1,1);
        }else if(!needRecovery){
            startRecovery = false;
            CancelInvoke("RecoveryHP");
        }

        //恢复护甲数值
        if(protectCurrent < protect){
            protectRecovery = true;
        }
        else if(protectCurrent >= protect){
            protectCurrent = protect;
            CancelInvoke("RecoveryProtect");
            protectRecovery = false;
        }
        if(protectRecovery && !startProtectRecovery){
            startProtectRecovery = true;
            InvokeRepeating("RecoveryProtect",1,1);
        }else if(!protectRecovery){
            startProtectRecovery = false;
            CancelInvoke("RecoveryProtect");
        }

        //虫虫6：4只虫以上才生效
        if(bugs.Count >= 4 && isBugMerge && !isTriggerMerge){
            TriggerBugMerge();
        }
        //角色定时进入一段无敌状态
        if(isBuffSuper){
            superTimeCount += Time.deltaTime;
            if(superTimeCount > superTime){
                superEffect.SetActive(true);
                isSuper = true;
                Invoke("RestoreSuper",superTime);
            }
        }
        //角色每隔一段时间会朝面向方向瞬移一段距离
        if(isMoveRandom){
            moveTimeCount += Time.deltaTime;
            if(moveTimeCount > moveTime && !Global.isSlowDown && !isMoving){
                isMoving = true;
                MoveRandom();
            }
        }
    }
    void MoveRandom(){
        Vector3 centerPos = new Vector3(circle.centerPos.transform.position.x,circle.centerPos.transform.position.y,transform.position.z);
        Vector3 playerPos = GetSymmetricPosition(transform.position,centerPos);
        Vector3 endpos = RandomMoveObject(playerPos);
        transform.DOScale(Vector3.zero,0.1f).OnComplete( ()=>{
            transform.position = endpos;
            transform.DOScale(new Vector3(0.8f,0.8f,0.8f),0.1f);
            RestoreMove();
        });
        // Invoke("RestoreMove",0.2f);
    }
    void RestoreSuper(){
        superEffect.SetActive(false);
        superTimeCount = 0;
        isSuper = false;
    }
    void RestoreMove(){
        isMoving = false;
        moveTimeCount = 0;
    }

    public bool IsInCircle(){
        // 计算物体与此脚本所附加物体之间的距离
        float distance = Vector3.Distance(transform.position,circle.centerPos.transform.position);
        // 如果物体在范围内，输出信息
        if (distance <= circle.radius + 1)
        {
            return true;
        }
        // 如果物体不在范围内
        else
        {
            return false;

        }
    }
    void RecoveryHP(){
        HPCurrent += HPSpeed;
    }
    public float GetHurtCount(){
        return HP-HPCurrent;
    }
    public void OutsideRecoveryHP(float hp){
        HPCurrent += hp;
        recoverAnim.SetActive(true);
        Invoke("SetRecoverAnimOff",1f);
    }
    void SetRecoverAnimOff(){
        recoverAnim.SetActive(false);
    }
    public void OutsideRecoveryProtect(float protect){
        protectCurrent += protect;
        recoverProtectAnim.SetActive(true);
        Invoke("SetRecoverProtectAnimOff",1.01f);
    }
    void SetRecoverProtectAnimOff(){
        recoverProtectAnim.SetActive(false);
    }
    void RecoveryProtect(){
        protectCurrent += protectSpeed;
    }
    public float GetHurtProtectCount(){
        return protect-protectCurrent;
    }
    //角色死亡处理：聚焦--场景遮黑--死亡动画--游戏结束
    private void Death()
    {
        transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,-6);
        moveSpeed = 0f;
        enemyPool.SetActive(false);
        lightning.SetActive(false);
        canvas.SetActive(false);
        effect.SetActive(false);
        blackBG.SetActive(true);
        Camera camera = Camera.main;
        camera.transform.DOMoveX(transform.position.x - 3,2);
        camera.transform.DOMoveY(transform.position.y,2).OnComplete(() =>
        {
            DOTween.To(()=>camera.orthographicSize, x =>camera.orthographicSize = x,5,2).OnComplete(()=>
                {
                    anim.PlayDeadAnim();
                }
            );
        });
    }

    public void Hurt (float hurt) {
        if(!ishitting && !gameController.isReward && !isSuper){
            ishitting = true;
            anim.PlayHurtAnim();
            float offsetHurt = 0;
            //优先伤害护甲
            if(protectCurrent > 0){
                if(protectCurrent > hurt){
                    protectCurrent -= hurt;
                }else if(protectCurrent <= hurt){
                    offsetHurt = hurt - protectCurrent;
                    HPCurrent -= offsetHurt;
                    protectCurrent = 0;
                } 
            }else{
                HPCurrent -= hurt;
            }
            textHP.text = HPCurrent.ToString();
            textPro.text = protectCurrent.ToString();
            sliderHP.value = HPCurrent/HP;
            sliderPro.value = protectCurrent/protect;

            if(isBuffBoom){
                SetMegaBoom();
            }
            
            if(isDebuffFreeze){
                SetFreezeBoom();
            }

            StartCoroutine(HittingCold());
        }
    }
    IEnumerator HittingCold(){
        yield return new WaitForSeconds(coldTime);
        ishitting = false;
    }

    public void EndGame () {
        SceneManager.LoadSceneAsync("UIScene");
    }
    bool CheckPlayerInView(){
        // 获取屏幕中心点
        Camera mainCamera = Camera.main;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 screenCenter = new Vector3(screenWidth * 0.5f, screenHeight * 0.5f,0);
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);

        //获取屏幕 4个顶点
        float cameraHeight = mainCamera.orthographicSize; // 如果是正交摄像机，获取宽度
        float cameraWidth = cameraHeight * mainCamera.aspect; // 获取高度，考虑屏幕比率
        float maxX = worldCenter.x + cameraWidth - 0.5f;
        float minX = worldCenter.x - cameraWidth + 0.5f;
        float maxY = worldCenter.y + cameraHeight - 1.5f;
        float minY = worldCenter.y - cameraHeight;
        if(transform.position.x<maxX&&transform.position.x>minX&&transform.position.y<maxY&&transform.position.y>minY)
            return true;
        else
            return false;
    }
    public void SetHP(int buff) {
        HP += buff;
        HPCurrent = HP;
    }
    public void SetSpeed(int buff) {
        moveSpeed += buff;
    }
    public void SetProtect(int buff) {
        protect += buff;
        protectCurrent = protect;
    }
    public void SetHPSpeed(int buff) {
        HPSpeed += buff;
    }
    public void SetProtectSpeed(int buff) {
        protectSpeed += buff;
    }
    //分身法宝01：围绕圆心生成角色分身，角色以及分身同时吸引雷劫
    public void SetCircleCopy() {
        isCircleCopy = !isCircleCopy;
        if(isCircleCopy){
            playerCopy = Instantiate(playerCopyPre);
            Global.playerCopyList.Add(playerCopy);
            Vector3 centerPos = new Vector3(circle.centerPos.transform.position.x,circle.centerPos.transform.position.y,transform.position.z);
            playerCopy.transform.position = GetSymmetricPosition(transform.position,centerPos);
            playerCopy.tag = "PlayerCopy";
        }
    }
    // 调用这个函数来获取关于圆点中心对称的位置
    public Vector3 GetSymmetricPosition(Vector3 originalPosition, Vector3 center)
    {
        // 步骤1：计算到圆心的向量
        Vector3 toCenter = originalPosition - center;
 
        // 步骤2：反转向量
        Vector3 symmetricPosition = -toCenter + center;
 
        return symmetricPosition;
    }
    //在pos周围范围内随机点
    public Vector3 RandomMoveObject(Vector3 pos)
    {
        Vector3 random;

        do
        {// 生成均匀分布的点
        float randomAngle = UnityEngine.Random.Range(0f, 360f); // 随机角度
        float randomRadius = UnityEngine.Random.Range(0f, 5); // 随机半径
 
        // 计算坐标
        random.x = pos.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius;
        random.y = pos.y + Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius;
        random.z = transform.position.z;}
        while(Vector3.Distance(random,circle.centerPos.transform.position) > circle.radius);
        return random;
    }

    //分身法宝2：每被雷击中三次召唤一个一次性分身
    public void SetOnceLightningCopy() {
        isOnceLightningCopy = !isOnceLightningCopy;
        
    }
    public void PlayerOnceLightCopy() {
        isLightCopied = true;
        playerOnceCopy = Instantiate(playerOnceCopyPre);
        Global.playerCopyList.Add(playerOnceCopy);
        playerOnceCopy.transform.position = transform.position;
        playerOnceCopy.tag = "PlayerOnceCopy";
    }

    //分身法宝3：每隔一段时间会在自己所在位置召唤一个一次性分身
    public void SetOnceTimeCopy() {
        isOnceTimeCopy = !isOnceTimeCopy;
    }
    //分身法宝4：雷点法宝适用于分身
    public void SetMegaCopy() {
        isMegaCopy = !isMegaCopy;
    }
    public void PlayerOnceTimeCopy() {
        isTimeCopied = true;
        playerOnceCopy = Instantiate(playerOnceCopyPre);
        Global.playerCopyList.Add(playerOnceCopy);
        playerOnceCopy.transform.position = transform.position;
        playerOnceCopy.tag = "PlayerOnceCopy";
    }

    //雷电法宝1：角色被雷击后，雷击将随机角度反射
    public void SetLightningMirror() {
        isLightningMirror = !isLightningMirror;
    }

    //雷电法宝2：当雷电攻击怪物后会发生爆炸伤害
    public void SetLightningBoom() {
        isLightningBoom = !isLightningBoom;
    }

    //雷电法宝3：当雷电攻击怪物后会发生爆炸伤害
    public void SetLightningOverflow() {
        isLightningOverflow = !isLightningOverflow;
    }

    //雷电法宝4：雷电会将周围的怪物吸附牵引
    public void SetLightningAttract() {
        isLightningAttract = !isLightningAttract;
    }
    //雷电法宝5：雷电会将周围的怪物吸附牵引
    public void SetLightningBoomPlayer() {
        isLightningBoomPlayer = !isLightningBoomPlayer;
    }
    public void SetBoom() {
        var boomCur = Instantiate(boom);
        boomCur.transform.position = transform.position + new Vector3(0,1,0);
        var scaleOffset = 1 + 2*(HPCurrent/HP); 
        boomCur.transform.localScale = new Vector3(scaleOffset,scaleOffset,scaleOffset);
    }
    public void SetCopyBoom(GameObject copy) {
        var boomCur = Instantiate(boom);
        boomCur.transform.position = copy.transform.position + new Vector3(0,0.6f,0);
        var scaleOffset = 1 + 2*(HPCurrent/HP); 
        boomCur.transform.localScale = new Vector3(scaleOffset,scaleOffset,scaleOffset);
    }

    //虫虫1：持续2秒回复生命值，每秒给角色回复10%已损失生命值，最小值为1
    public void SetBugHP() {
        isBugHP = !isBugHP;
        var bug1 = Instantiate(bugHPPre.gameObject);
        BugController bug = bug1.GetComponent<BugController>();
        bug.transform.parent = petBugs.transform;
        bug.bugId = bugs.Count;
        bugs.Add(bug);
    }
    //虫虫1：在自身周围形成一个环形造成伤害持续10秒
    public void SetBugCircle() {
        isBugCircle = !isBugCircle;
        var bug1 = Instantiate(bugCirclePre.gameObject);
        BugCircleController bug = bug1.GetComponent<BugCircleController>();
        bug.transform.parent = petBugs.transform;
        bug.bugId = bugs.Count;
        bugs.Add(bug);
        bugCircleCollider.minDistance = bug.circleRadiusMin;
        bugCircleCollider.transform.localScale = new Vector3(bug.circleRadiusMax*2,bug.circleRadiusMax*2,bug.circleRadiusMax*2);
        bugCircleCollider.hurtCount = bug.hurtCount;
    }
    //虫虫3：持续2秒回复生命值，每秒给角色回复10%已损失生命值，最小值为1
    public void SetBugFollow() {
        isBugFollow = !isBugFollow;
        var bug1 = Instantiate(bugFollowPre.gameObject);
        BugFollowController bug = bug1.GetComponent<BugFollowController>();
        bug.transform.parent = petBugs.transform;
        bug.bugId = bugs.Count;
        bugs.Add(bug);
    }
    //虫虫4：在身体后方召唤一道墙壁阻挡怪物和子弹，持续5秒
    public void SetBugWall() {
        isBugWall = !isBugWall;
        var bug1 = Instantiate(bugWallPre.gameObject);
        BugWallController bug = bug1.GetComponent<BugWallController>();
        bug.transform.parent = petBugs.transform;
        bug.bugId = bugs.Count;
        bugs.Add(bug);
    }
    //虫虫5：自动攻击距离最近的怪物一次
    public void SetBugAttack() {
        isBugAttack = !isBugAttack;
        var bug1 = Instantiate(bugAttackPre.gameObject);
        BugAttackController bug = bug1.GetComponent<BugAttackController>();
        bug.transform.parent = petBugs.transform;
        bug.bugId = bugs.Count;
        bugs.Add(bug);
    }
    //虫虫6：角色同时存在4只以上蛊虫时，每隔2秒会自动为一只处于充能状态的蛊虫完全充能
    public void SetBugMerge() {
        isBugMerge = !isBugMerge;
        // var bug1 = Instantiate(bugMergePre.gameObject);
        // BugController bug = bug1.GetComponent<BugController>();
        // bug.transform.parent = petBugs.transform;
        // bug.bugId = bugs.Count;
        // bugs.Add(bug);
    }
    void TriggerBugMerge(){
        isTriggerMerge = true;
        InvokeRepeating("RandomBugEnergy",0,2);
    }
    void RandomBugEnergy(){
        int id = -1;
        int count = 0;
        do
        {
            id = UnityEngine.Random.Range(0,bugs.Count);
            count ++;
            if(count > 20){
                return;
            }
        } while(bugs[id].isTriggered);
        if(id >= 0 && !bugs[id].isTriggered){
            bugs[id].energyCurrent += bugs[id].energy;
            Debug.Log("充能虫子："+bugs[id].name);
        }
    }

    //符箓·火球：每8轮雷劫再次召唤，对一定范围内敌人每秒喷射一个火球。
    public void SetPaperFireBall() {
        isPaperFireBall = !isPaperFireBall;
        var paper = Instantiate(paperFirePre);
        paper.transform.parent = papers.transform;
        paper.transform.position = papers.transform.position;
    }

    //符箓·寒冰：每10轮雷劫再次召唤，剑气围绕符箓旋转，对扫过的敌人造成伤害并减少20%移动速度。
    public void SetPaperIce() {
        isPaperIce = !isPaperIce;
        var paper = Instantiate(paperIcePre);
        paper.transform.parent = papers.transform;
        paper.transform.position = papers.transform.position;
    }
    //符箓·寒冰：每10轮雷劫再次召唤，剑气围绕符箓旋转，对扫过的敌人造成伤害并减少20%移动速度。
    public void SetPaperHP() {
        isPaperHP = !isPaperHP;
        var paper = Instantiate(paperHPPre);
        paper.transform.parent = papers.transform;
        paper.transform.position = papers.transform.position;
    }
    //符箓·寒冰：每10轮雷劫再次召唤，剑气围绕符箓旋转，对扫过的敌人造成伤害并减少20%移动速度。
    public void SetPaperProtect() {
        isPaperProtect = !isPaperProtect;
        var paper = Instantiate(paperProtectPre);
        paper.transform.parent = papers.transform;
        paper.transform.position = papers.transform.position;
    }
    //符箓·寒冰：每10轮雷劫再次召唤，剑气围绕符箓旋转，对扫过的敌人造成伤害并减少20%移动速度。
    public void SetPaperBlack() {
        isPaperBlack = !isPaperBlack;
        var paper = Instantiate(paperBlackPre);
        paper.transform.parent = papers.transform;
        paper.transform.position = papers.transform.position;
    }
    //符箓·串联：每轮雷劫只会生成一道雷击，雷击道数会转化成为雷电伤害，并将自动串联所有符箓。
    public void SetPaperConnect() {
        isPaperConnect = !isPaperConnect;
        LightningController lightningController = lightning.GetComponent<LightningController>();
        lightningController.ConnectPaper();
    }
    //Debuff·减速：移动路径上留下一个可以降低怪物速度的减速区域
    public void SetDebuffSlow() {
        isDebuffSlow = !isDebuffSlow;
        moveSlow.SetActive(isDebuffSlow);
    }
    //Debuff·冻结：受伤后有几率，将周围的怪物冻结
    public void SetDebuffFreeze() {
        isDebuffFreeze = !isDebuffFreeze;
    }
    public void SetFreezeBoom() {
        var boomCur = Instantiate(freezeBoom);
        boomCur.transform.position = transform.position + new Vector3(0,1,0);
    }
    //Debuff·麻痹：雷击会让怪物有几率麻痹
    public void SetDebuffDizzy() {
        isDebuffDizzy = !isDebuffDizzy;
    }
    //Buff·冲击波：受到伤害后，发出冲击波将周围的怪物击退
    public void SetBuffBoom() {
        isBuffBoom = !isBuffBoom;
    }
    public void SetMegaBoom() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.layer == 6)
                collider.GetComponent<EnemyController>().isBoom = true;
        }

        var boomCur = Instantiate(boom);
        boomCur.transform.position = transform.position + new Vector3(0,1,0);
        boomCur.transform.localScale = new Vector3(2,2,2);
    }
    //Buff·无敌：角色定时进入一段无敌状态
    public void SetBuffSuper() {
        isBuffSuper = !isBuffSuper;
    }
    //位移：角色每隔一段时间会朝面向方向瞬移一段距离
    public void SetMoveRandom() {
        isMoveRandom = !isMoveRandom;
    }
}
