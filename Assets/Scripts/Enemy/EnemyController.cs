using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("生命值")]
    public float HP = 20f;
    float maxHP = 0;
    [Header("攻击力")]
    public float attack = 5f;
    float attackSave = 5f;
    [Header("移动速度")]
    public float speed = 1.0f; // 移动速度
    float speedSave = 1;
    [Header("UI")]
    public TMP_Text text;
    [Header("随机范围圆心")]
    public Transform center;
    [Header("随机范围半径")]
    public float radius = 15f;
    [Header("眩晕动画")]
    public GameObject duzzyEffect;
    [Header("暴走动画")]
    public GameObject crazyEffect;
    [Header("回血动画")]
    public GameObject recoverEffect;
    [HideInInspector]
    public bool isHitting = false;
    public bool isFollowHitting = false;
    [HideInInspector]
    public Vector3 startPosition; // 开始位置
    public Animator anim;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public bool isInBlackHall;
    public bool isBoomHall;
    public GameObject lineCopy;

    public GameObject boom;
    PlayerController player;
    float moreHurt = 0;
    bool isDead = false;
    Tweener tweener;
    float circleCountTime = 0;
    float followCountTime = 0;
    public bool iceSpeed = false;
    public Vector3 boomDir;
    public bool debuffSlowing = false;
    float debuffCount = 0;
    bool isFreeze = false;
    bool isCrazy = false;
    public bool isBoom = false;
    Rigidbody rb;
    Vector3 randomAngle;
    public virtual void Start()
    {
        maxHP = HP;
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position; // 记录开始位置
        text.text = HP.ToString();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        speedSave = speed;
        attackSave = attack;
        center = GameObject.FindGameObjectWithTag("Center").transform;
        randomAngle = RandomUnitVector();
    }
    public virtual void Update() {
        // if(HP <= 0 && !isDead){
        //     Death();
        // }
        text.text = (Mathf.Round(HP * 10.0f) / 10.0f).ToString();
        //速度处理
        if(Global.isSlowDown){
            speed = speedSave/10;
            anim.speed = 0.5f;
        }else if(iceSpeed){
            anim.speed = 0.5f;
            speed = speedSave/5;
        }else if(debuffSlowing){
            speed = speedSave * 0.5f;
            anim.speed = 0.5f;
        }else if(isCrazy){
            speed = speedSave + 1;
            attack = attackSave +1;
            anim.speed = 2;
        }else if(!Global.isSlowDown && !iceSpeed && !debuffSlowing && !isFreeze && !isCrazy){
            anim.speed = 1;
            speed = speedSave;
        }

        if(isBoomHall){
            MoveBoom(boomDir);
            if(Vector3.Distance(transform.position,boomDir) > 3){
                isBoomHall = false;
                isInBlackHall = false;
            }
        }
        if(isBoom){
            rb.AddForce(5 * randomAngle, ForceMode.Impulse);
            Invoke("RestoreBoom",0.5f);
        }
        if(debuffSlowing && !Global.isSlowDown){
            debuffCount += Time.deltaTime;
        }
        if(debuffCount > 3 && debuffSlowing){
            debuffSlowing = false;
            debuffCount = 0;
        }
    }

    Vector3 RandomUnitVector()
    {
        float zenith = Random.Range(0f, Mathf.PI); // 随机选择一个极角
        float azimuth = Random.Range(0f, Mathf.PI * 2f); // 随机选择一个方位角

        float x = Mathf.Sin(zenith) * Mathf.Cos(azimuth); // 根据球坐标系转换为笛卡尔坐标系
        float y = Mathf.Sin(zenith) * Mathf.Sin(azimuth);
        float z = Mathf.Cos(zenith);

        return new Vector3(x, y, z).normalized; // 返回单位向量
    }
    void RestoreBoom(){
        isBoom = false;
    }
    public void RandomMoveInHall(Vector3 centerHall,float range)
    {
        if(!isBoomHall)
        {
            // 生成均匀分布的点
            float randomAngle = Random.Range(0f, 360f); // 随机角度
            float randomRadius = Random.Range(0f, range); // 随机半径
    
            // 计算坐标
            float randomX= Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius;
            float randomY = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius;
            Vector3 randomPos = centerHall + new Vector3(randomX,randomY,0);
            tweener = transform.DOMove(randomPos,1).OnComplete(()=>{
                RandomMoveInHall(centerHall,range);
            });
        }
    }
    public void MoveBoom(Vector3 target){
        Vector3 direction = transform.position - target;
        direction.Normalize();
        Rigidbody rb = GetComponent<Rigidbody>();
        // rb.velocity = direction * speed *50;
        rb.velocity = Vector3.zero;
        if(direction == Vector3.zero){
            direction = Vector3.up;
        }
        rb.AddForce(direction*30,ForceMode.Impulse);
    }
    public virtual void Hurt (float hurt,HurtType type) {
        if(!isHitting && !isDead){
            if (tweener != null)
            {
                tweener.Kill();
            }
            isHitting = true;
            if(hurt >= HP){
                if(type == HurtType.Lightning||type == HurtType.Overflow)
                    moreHurt = hurt - HP;
                Death();
            }
            HP -= hurt;
            // text.text = HP.ToString();
            if(type == HurtType.Lightning && player.isLightningBoom)
                SetBoom();
            if(player.isDebuffDizzy){
                int random = Random.Range(1,11);
                if(random > 5){
                    Freeze();
                }
            }
        }
    }
    public virtual void HurtByCircle(float hurt,HurtType type){
        if(type == HurtType.BugCircle){
            circleCountTime += Time.deltaTime;
            if(circleCountTime >= 0.1f){
                circleCountTime = 0;
                HP -= hurt;
                // text.text = HP.ToString();
                if(HP <= 0 && !isDead){
                    Death();
                }
            }
        }
    }
    public virtual void HurtByFollow(float hurt,HurtType type){
        if(type == HurtType.BugFollow){
            isFollowHitting = true;
            followCountTime += Time.deltaTime;
            if(followCountTime >= 0.5f){
                followCountTime = 0;
                HP -= hurt;
                if(HP <= 0 && !isDead){
                    Death();
                }
            }
        }
    }
    public virtual void HurtByPaperIce(float hurt,HurtType type){
        if(type == HurtType.PaperIce){
            HP -= hurt;
            iceSpeed = true;
            if(HP <= 0 && !isDead){
                Death();
            }
            Invoke("ResetSpeed",5);
        }
        
    }
    public virtual void HurtByBugAttack(float hurt,HurtType type){
        HP -= hurt;
        if(HP <= 0 && !isDead){
            Death();
        }
    }
    public virtual void Freeze(){
        isFreeze = true;
        speed = 0;
        duzzyEffect.SetActive(true);
        Invoke("ResetFreeze",3);
    }
    void ResetFreeze(){
        duzzyEffect.SetActive(false);
        speed = speedSave;
        isFreeze = false;
    }
    void ResetSpeed(){
        iceSpeed = false;
    }
    public void SetCrazy () {
        isCrazy = true;
        speed = speedSave + 1;
        attack = attackSave +1;
        anim.speed = 2;
        crazyEffect.SetActive(true);
        Invoke("ResetCrazy",5);
    }
    public void ResetCrazy () {
        crazyEffect.SetActive(false);
        isCrazy = false;
        speed = speedSave;
        attack = attackSave;
        anim.speed = 1;
    }
    public void Recover (float hp) {
        if(HP < maxHP){
            if(hp > (maxHP-HP)){
                HP = maxHP;
            }else{
                HP += hp;
            }
            text.text = HP.ToString();
            recoverEffect.SetActive(true);
            Invoke("ResetRecover",1);
        }
    }
    void ResetRecover(){
        recoverEffect.SetActive(false);
    }

    public virtual void Death () {
        speed = 0;
        isDead = true;
        gameObject.tag = "Untagged";
        if(moreHurt > 0 && player.isLightningOverflow)
        {
            SetMoreHurt(moreHurt);
            Debug.Log("溢出伤害:" + moreHurt);
        }
        
        StartCoroutine(SetDestroy());
    }
    IEnumerator SetDestroy(){
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isDead)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if(player.HP > 0)
                player.Hurt(attack);
        }
    }
    //在圆形范围边缘随机生成
    public virtual void SpawnAtRandomCircle(){
        float angle = Random.Range(0,Mathf.PI * 2);
        Vector3 point = center.position + new Vector3(Mathf.Cos(angle) * radius,Mathf.Sin(angle) * radius,0);
        transform.position = new Vector3(point.x,point.y,-5);
    }

    //受伤后产生爆炸
    void SetBoom(){
        var boomCur = Instantiate(boom);
        boomCur.transform.position = transform.position;
    }
    //死亡后溢出伤害加到最近的敌人上
    void SetMoreHurt(float hurt){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] filteredEnemies = System.Array.FindAll(enemies, enemy => enemy != gameObject);
        if(filteredEnemies.Length <= 0){
            Debug.Log("死光了");
        }else
        {
            float minDistance = -1;
            int enemyId = -1;
            for (int i = 0; i < filteredEnemies.Length; i++)
            {
                if(i == 0){
                    Vector3 pos1 = new Vector3(filteredEnemies[i].transform.position.x,filteredEnemies[i].transform.position.y,0);
                    Vector3 pos2 = new Vector3(transform.transform.position.x,transform.position.y,0);
                    minDistance = Vector3.Distance(pos1,pos2);
                    enemyId = 0;
                }else{
                    Vector3 pos1 = new Vector3(filteredEnemies[i].transform.position.x,filteredEnemies[i].transform.position.y,0);
                    Vector3 pos2 = new Vector3(transform.transform.position.x,transform.position.y,0);
                    float distance = Vector3.Distance(pos1,pos2);
                    if(distance < minDistance){
                        enemyId = i;
                    }
                }
            }
            Debug.Log("距离最近的敌人："+enemyId);
            if(enemyId<filteredEnemies.Length && enemyId>=0){
                var lineCur = Instantiate(lineCopy.gameObject);
                lineCur.transform.position = transform.position;
                OverLineController lineController = lineCur.GetComponent<OverLineController>();
                lineController.start.transform.position = transform.position;
                lineController.end.transform.position = filteredEnemies[enemyId].transform.position;
                lineController.startTime = 0.2f;
                lineController.keepTime = 0.1f;
                lineController.showTime = 0;
                EnemyController hurtEnemy = filteredEnemies[enemyId].GetComponent<EnemyController>();
                hurtEnemy.Hurt(hurt,HurtType.Overflow);
            }else if(enemyId>filteredEnemies.Length-1){
                SetMoreHurt(hurt);
            }else if(enemyId<0){
                Debug.Log("死光了");
            }
        }
        
    }
    //移动到雷电上
    public void MoveToLine(Vector3 pos){
        if(transform != null)
            tweener = transform.DOMove(pos,0.1f);
    }
    private void OnDestroy() {
        if (tweener != null)
        {
            tweener.Kill();
        }
    }

}

public enum HurtType{
    Lightning,
    Boom,
    Overflow,
    MirrorLine,
    CopyPlayer,
    BugCircle,
    BugFollow,
    BugAttack,
    PaperFireBall,
    PaperIce,
    BlackHall
}