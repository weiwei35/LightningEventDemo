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
    Vector3 midPoint;
    public GameObject lineCollider;
    GameObject colliderCur;
    CapsuleCollider capsuleCollider;
    public LightningEffect lightningAsset;
    LightningEffect lightningEffect;
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
            pos.transform.position = follow.transform.position + new Vector3(0,1f,0);
            // end.transform.position = follow.transform.position;
            // SetCircleLine();
            lightningEffect.pos1.transform.position = pos.transform.position;
            lightningEffect.pos2.transform.position = pos.transform.position;
        }
        if(colliderCur != null){
            colliderCur.GetComponent<LineCollider>().start = start;
            colliderCur.GetComponent<LineCollider>().end = end.transform.position;
            for(int i = 0; i < line.positionCount-1; i++)
            {
                Vector3 midPoint = (line.GetPosition(i) + line.GetPosition(i+1)) / 2;
                colliderCur.transform.position = midPoint;
                colliderCur.transform.parent = transform;
                Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
                capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
                capsuleCollider.isTrigger = true;
                capsuleCollider.radius = line.startWidth/4;
                capsuleCollider.height = direction.magnitude + line.startWidth;
                capsuleCollider.direction = 2;
                if(Vector3.Distance(line.GetPosition(i+1),line.GetPosition(i)) != 0){
                    Quaternion lookRotation = Quaternion.LookRotation(line.GetPosition(i+1) - line.GetPosition(i));
                    colliderCur.transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x,lookRotation.eulerAngles.y,lookRotation.eulerAngles.z);
                }
            }
        }
        line.SetPosition(0,start);
        line.SetPosition(1,pos.transform.position);
    }

    public void CheckCopy() {
        if(player.GetComponent<PlayerController>().isOnceLightningCopy || player.GetComponent<PlayerController>().isOnceTimeCopy){
            GameObject[] playerOnceCopy = GameObject.FindGameObjectsWithTag("PlayerOnceCopy");
            if(playerOnceCopy.Length > 0){
                foreach (var item in playerOnceCopy)
                {
                    var lineCurCopy = Instantiate(gameObject);
                    lineCurCopy.transform.position = transform.position;
                    CopyLineController lineControllerCopy = lineCurCopy.GetComponent<CopyLineController>();
                    lineControllerCopy.start = start;
                    lineControllerCopy.end.transform.position = new Vector3(item.transform.position.x,item.transform.position.y,-5);
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
                var lineCurCopy = Instantiate(gameObject);
                lineCurCopy.transform.position = transform.position;
                CopyLineController lineControllerCopy = lineCurCopy.GetComponent<CopyLineController>();
                lineControllerCopy.start = start;
                lineControllerCopy.end.transform.position = new Vector3(playerCopy.transform.position.x,playerCopy.transform.position.y,-5);
                lineControllerCopy.startTime = startTime;
                lineControllerCopy.keepTime = keepTime;
                lineControllerCopy.follow = playerCopy;
                lineControllerCopy.timeCount = lightning.lightningPreTime+1;
                lineControllerCopy.isChecked = true;
        }
    }

    public void DrawLinePoints() {
        canMove = false;
        Global.isSlowDown = true;
        lightningEffect = Instantiate(lightningAsset);
        lightningEffect.transform.parent = transform;
        lightningEffect.pos1.transform.position = start;
        lightningEffect.pos2.transform.position = start;
        lightningEffect.pos1.transform.DOMove(end.transform.position,startTime);
        lightningEffect.pos2.transform.DOMove(end.transform.position,startTime);
        lightningEffect.pos3.transform.position = start;
        lightningEffect.pos4.transform.position = start;
        line.startWidth = lightning.lightningWidth;
        line.endWidth = lightning.lightningWidth;
        pos.transform.position = start;
        pos.transform.DOMove(end.transform.position,startTime).OnComplete(()=>
        {
            Global.isSlowDown = false;
            canMove = true;
            Invoke("EndLine", keepTime);
        });
        Vector3 midPoint = (line.GetPosition(0) + line.GetPosition(1)) / 2;
        colliderCur = Instantiate(lineCollider);
        colliderCur.transform.position = midPoint;
        colliderCur.transform.parent = transform;
        Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
        capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = line.startWidth/4;
        capsuleCollider.height = direction.magnitude + line.startWidth;
        capsuleCollider.direction = 2;

        // SetCircleLine();
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
}
