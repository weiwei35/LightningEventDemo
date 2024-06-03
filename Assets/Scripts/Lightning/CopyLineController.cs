using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CopyLineController : MonoBehaviour
{
    public Vector3 start;
    public GameObject end;
    public GameObject pos;
    public LayerMask layer;
    public GameObject follow;
    public GameObject startPointEP;//特殊雷点
    public GameObject startPoint;
    public float showTime = 0;
    public float startTime = 0.1f;
    public float keepTime = 0.5f;
    LineRenderer line;
    bool canMove = false;
    public bool isChecked = false;
    public float timeCount = 0;

    EnemyController enemy;
    LightningController lightning;
    PlayerController player;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        StartCoroutine(SetStartPoint());
    }
    //依次出现雷点
    IEnumerator SetStartPoint(){
        yield return new WaitForSeconds(showTime);
        startPoint.gameObject.SetActive(true);
        var start = Instantiate(startPointEP);
        start.transform.position = transform.position;
        start.transform.parent = transform;
    }
    private void Update() {
        timeCount += Time.deltaTime;
        if(timeCount > lightning.lightningPreTime){
            timeCount = 0;
            if(follow == null)
                end.transform.position = start;
            if(!isChecked){
                isChecked = true;
                // CheckCopy();
            }
            DrawLinePoints();
        }
    }
    private void FixedUpdate()
    {
        if(pos.transform.position != follow.transform.position && canMove && follow!= null){
            pos.transform.position = follow.transform.position + new Vector3(0,0.5f,0);
            // end.transform.position = follow.transform.position;
        }
            
        line.SetPosition(0,start);
        line.SetPosition(1,pos.transform.position);
        RaycastHit[] hits = new RaycastHit[5];
        Vector3 startPos = new Vector3(start.x,start.y,-5);
        Vector3 endPos = new Vector3(pos.transform.position.x,pos.transform.position.y,-5);
        var distance = Vector3.Distance(startPos, endPos);
        Vector3 direction = endPos - startPos;
        if(canMove){
            int count = Physics.RaycastNonAlloc(startPos,direction,hits,distance,layer);
            if(count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    enemy = hits[i].collider.gameObject.GetComponent<EnemyController>();
                    if(enemy != null && !enemy.isHitting)
                    {
                        Debug.Log("雷电攻击：" + hits[i].collider.name);
                        lightning.HurtEnemy(enemy,HurtType.CopyPlayer);
                    }
                }
            }
        }
    }

    public void CheckCopy() {
        if(player.GetComponent<PlayerController>().isOnceLightningCopy || player.GetComponent<PlayerController>().isOnceTimeCopy){
            GameObject[] playerOnceCopy = GameObject.FindGameObjectsWithTag("PlayerOnceCopy");
            if(playerOnceCopy.Length > 0){
                foreach (var item in playerOnceCopy)
                {
                    var lineCurCopy = Instantiate(gameObject);
                    lineCurCopy.transform.position = transform.position;
                    LineController lineControllerCopy = lineCurCopy.GetComponent<LineController>();
                    lineControllerCopy.start = start;
                    lineControllerCopy.end.transform.position = new Vector3(item.transform.position.x,item.transform.position.y,-4);
                    lineControllerCopy.startTime = startTime;
                    lineControllerCopy.keepTime = keepTime;
                    lineControllerCopy.follow = item;
                    lineControllerCopy.timeCount = lightning.lightningPreTime+1;
                    lineControllerCopy.isChecked = true;
                }
            }
        }
        if(player.GetComponent<PlayerController>().isCircleCopy){
                GameObject playerCopy = GameObject.FindGameObjectWithTag("PlayerCopy");
                var lineCurCopy = Instantiate(line.gameObject);
                lineCurCopy.transform.position = transform.position;
                LineController lineControllerCopy = lineCurCopy.GetComponent<LineController>();
                lineControllerCopy.start = start;
                lineControllerCopy.end.transform.position = new Vector3(playerCopy.transform.position.x,playerCopy.transform.position.y,-4);
                lineControllerCopy.startTime = startTime;
                lineControllerCopy.keepTime = keepTime;
                lineControllerCopy.follow = playerCopy;
                lineControllerCopy.timeCount = lightning.lightningPreTime+1;
                lineControllerCopy.isChecked = true;
        }
    }

    public void DrawLinePoints() {
        canMove = false;
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;
        pos.transform.position = start;
        pos.transform.DOMove(end.transform.position,startTime).OnComplete(()=>
        {
            canMove = true;
            Invoke("EndLine", keepTime);
            Invoke("SetEndLine", 0);
        });
    }

    public void EndLine () {
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        foreach (var item in enemys)
        {
            item.isHitting = false;
        }
        if(follow != null && follow.layer == 7){
            PlayerOnceController copy = follow.GetComponent<PlayerOnceController>();
            copy.lightningCount ++;
        }
        lightning.isEndLight = true;
        lightning.HurtPlayer();
        Destroy(gameObject);
    }

    public void SetEndLine () {
        lightning.isSetLight = true;
    }
}
