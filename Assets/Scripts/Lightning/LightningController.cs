using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [Header("雷电频率")]
    public float lightningTime = 3f;
    [Header("雷电预告时长")]
    public float lightningPreTime = 3f;
    [Header("雷电点数")]
    public float lightningCount = 3;
    [Header("雷电伤害")]
    public float lightningHurt = 0;
    [Header("雷电宽度")]
    public float lightningWidth = 0.3f;
    [Header("雷电出现时长")]
    public float startTime = 0.1f;
    [Header("雷电持续时长")]
    public float keepTime = 0.5f;
    [Header("结界伤害")]
    public float circleHurt = 5f;
    float curTime = 0;
    //雷电结束
    public bool isEndLight = false;
    public bool endLight = false;
    public bool endConnect = true;
    //雷电到达player
    public bool isSetLight = false;
    public bool endsetLight = false;
    CircleController circle;
    PlayerController player;
    GameObject petBugs;
    GameObject papers;
    // Start is called before the first frame update
    void Start()
    {
        circle = GetComponent<CircleController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        petBugs = GameObject.FindGameObjectWithTag("PetBugs");
        papers = GameObject.FindGameObjectWithTag("Papers");
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if(curTime >= lightningTime){
            curTime = 0;
            circle.RandomPoints(lightningCount);
        }
        
        if(isEndLight && !endLight){
            endLight = true;
            StartCoroutine(EndLight());
        }
        if(player.isLightningMirror)
        {
            if(isSetLight && !endsetLight){
                endsetLight = true;
                StartCoroutine(SetEndLight());
            }
        }
    }

    public void HurtEnemy (EnemyController enemy,HurtType type) {
        enemy.Hurt(lightningHurt,type);
    }

    IEnumerator EndLight(){
        isEndLight = false;
        if(player.isOnceLightningCopy)
            player.lightningCount ++;
        foreach (Transform item in petBugs.transform)
        {
            BugController bug = item.GetComponent<BugController>();
            if(bug!= null && bug.canRecoverEnergy)
            {
                bug.energyCurrent += (int)lightningCount;
            }
        }
        foreach (Transform item in papers.transform)
        {
            PaperController paper = item.GetComponent<PaperController>();
            if(paper!= null)
            {
                paper.countCurrent ++;
                paper.isAddLight = true;
            }
        }
        yield return new WaitForSeconds(0.5f);
        endLight = false;
    }
    IEnumerator SetEndLight(){
        isSetLight = false;
        circle.RandomPointsMirror(lightningCount);
        if(player.isMegaCopy){
            circle.RandomPointsCopy(lightningCount);
        }
        yield return new WaitForSeconds(0.5f);
        endsetLight = false;
    }

    public void HurtPlayer () {
        if(player.canHurt){
            player.Hurt(circleHurt);
        }
    }

    public void ConnectPaper() {
        lightningCount = 1;
    }
}
