using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MirrorLineController : MonoBehaviour
{
    public Vector3 start;
    public GameObject end;
    public GameObject pos;
    public LayerMask layer;
    public float showTime = 0;
    public float startTime = 0.1f;
    public float keepTime = 0.5f;
    LineRenderer line;
    bool canMove = false;
    public bool isChecked = false;
    public float timeCount = 0;
    public GameObject follow;
    EnemyController enemy;
    LightningController lightning;
    PlayerController player;
    Vector3 midPoint;
    public GameObject lineCollider;
    CapsuleCollider capsule;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(!isChecked){
            isChecked = true;
            // CheckCopy();
        }
        DrawLinePoints();
    }
    private void Update() {
            
    }
    private void FixedUpdate()
    {
        if(pos.transform.position != end.transform.position && canMove)
            pos.transform.position = end.transform.position;
        line.SetPosition(0,start);
        line.SetPosition(1,pos.transform.position);
        if(canMove)
        {
            midPoint = (start + pos.transform.position) / 2;
            lineCollider.transform.position = midPoint;
            Vector3 direction = end.transform.position - start;
            capsule.isTrigger = true;
            capsule.radius = line.startWidth/2;
            capsule.height = direction.magnitude + line.startWidth;
            capsule.direction = 2;
            Quaternion lookRotation = Quaternion.LookRotation(end.transform.position - start);
            lineCollider.transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x,lookRotation.eulerAngles.y,lookRotation.eulerAngles.z);
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
                    MirrorLineController lineControllerCopy = lineCurCopy.GetComponent<MirrorLineController>();
                    lineControllerCopy.start = start;
                    lineControllerCopy.end.transform.position = new Vector3(item.transform.position.x,item.transform.position.y,-5);
                    lineControllerCopy.startTime = startTime;
                    lineControllerCopy.keepTime = keepTime;
                    lineControllerCopy.timeCount = lightning.lightningPreTime+1;
                    lineControllerCopy.isChecked = true;
                }
            }
        }
        if(player.GetComponent<PlayerController>().isCircleCopy){
                GameObject playerCopy = GameObject.FindGameObjectWithTag("PlayerCopy");
                var lineCurCopy = Instantiate(line.gameObject);
                lineCurCopy.transform.position = transform.position;
                MirrorLineController lineControllerCopy = lineCurCopy.GetComponent<MirrorLineController>();
                lineControllerCopy.start = start;
                lineControllerCopy.end.transform.position = new Vector3(playerCopy.transform.position.x,playerCopy.transform.position.y,-5);
                lineControllerCopy.startTime = startTime;
                lineControllerCopy.keepTime = keepTime;
                lineControllerCopy.timeCount = lightning.lightningPreTime+1;
                lineControllerCopy.isChecked = true;
        }
    }

    public void DrawLinePoints() {
        canMove = false;
        line.startWidth = lightning.lightningWidth;
        line.endWidth = lightning.lightningWidth;
        pos.transform.position = start;
        pos.transform.DOMove(end.transform.position,startTime).OnComplete(()=>
        {
            canMove = true;
            lineCollider = Instantiate(lineCollider);
            lineCollider.transform.position = transform.position;
            lineCollider.transform.parent = transform;
            capsule = lineCollider.GetComponent<CapsuleCollider>();
            Invoke("EndLine", keepTime);
        });
    }

    public void EndLine () {
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        foreach (var item in enemys)
        {
            item.isHitting = false;
        }
        Destroy(gameObject);
    }
}
