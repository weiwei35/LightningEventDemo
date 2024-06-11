using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("生命值")]
    public float HP = 20f;
    [Header("攻击力")]
    public float attack = 5f;
    [Header("移动速度")]
    public float speed = 1.0f; // 移动速度
    float speedSave = 1;
    [Header("UI")]
    public TMP_Text text;
    [Header("随机范围圆心")]
    public Vector3 center = Vector3.zero;
    [Header("随机范围半径")]
    public float radius = 15f;
    [HideInInspector]
    public bool isHitting = false;
    public bool isFollowHitting = false;
    [HideInInspector]
    public Vector3 startPosition; // 开始位置
    [HideInInspector]
    public Animation anim;
    [HideInInspector]
    public Transform target;
    public GameObject lineCopy;

    public GameObject boom;
    PlayerController player;
    float moreHurt = 0;
    bool isDead = false;
    Tweener tweener;
    float circleCountTime = 0;
    float followCountTime = 0;

    public virtual void Start()
    {
        startPosition = transform.position; // 记录开始位置
        text.text = HP.ToString();
        anim = GetComponent<Animation>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        speedSave = speed;
    }
    public virtual void Update() {
        // if(HP <= 0 && !isDead){
        //     Death();
        // }
        text.text = (Mathf.Round(HP * 10.0f) / 10.0f).ToString();
        //速度处理
        if(Global.isSlowDown){
            speed = speedSave/10;
        }else{
            speed = speedSave;
        }
    }
    public virtual void Hurt (float hurt,HurtType type) {
        if(!isHitting && !isDead){
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
    public virtual void HurtByBugAttack(float hurt,HurtType type){
        if(type == HurtType.BugAttack){
            HP -= hurt;
            if(HP <= 0 && !isDead){
                Death();
            }
        }
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
        if (tweener != null)
        {
            tweener.Kill();
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
        Vector3 point = center + new Vector3(Mathf.Cos(angle) * radius,Mathf.Sin(angle) * radius,0);
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
                lineController.start = transform.position;
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

}

public enum HurtType{
    Lightning,
    Boom,
    Overflow,
    CopyPlayer,
    BugCircle,
    BugFollow,
    BugAttack
}