using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CopyLineController : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public GameObject follow;
    public GameObject startPointEP;//特殊雷点
    public GameObject startPoint;
    public float showTime = 0;
    public float startTime = 0.1f;
    public float keepTime = 0.5f;
    LineRenderer line;
    public GameObject copyLine;
    public bool isChecked = false;
    public float timeCount = 0;
    LightningController lightning;
    PlayerController player;
    public GameObject lineCollider;
    GameObject colliderCur;
    CapsuleCollider capsuleCollider;
    public LightningEffect lightningAsset;
    LightningEffect lightningEffect;

    //冲刺折线相关
    public bool isRush = false;
    public bool canCopy = true;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update() {
        timeCount += Time.deltaTime;
        if(timeCount > lightning.lightningPreTime){
            timeCount = 0;
            if(follow == null)
                end.transform.position = start.transform.position;
            DrawLinePoints();
        }
    }
    private void FixedUpdate()
    {
        end.transform.position = follow.transform.position + new Vector3(0,1f,0);
        if(lightningEffect != null){
            lightningEffect.pos1.transform.position = start.transform.position;
            lightningEffect.pos2.transform.position = start.transform.position;
            lightningEffect.pos3.transform.position = end.transform.position;
            lightningEffect.pos4.transform.position = end.transform.position;
        }
        if(colliderCur != null){
            colliderCur.GetComponent<LineCollider>().start = start.transform.position;
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
        line.SetPosition(0,start.transform.position);
        line.SetPosition(1,end.transform.position);
    }

    public void DrawLinePoints() {
        lightningEffect = Instantiate(lightningAsset);
        lightningEffect.transform.parent = transform;
        lightningEffect.pos1.transform.position = start.transform.position;
        lightningEffect.pos2.transform.position = start.transform.position;
        lightningEffect.pos3.transform.position = end.transform.position;
        lightningEffect.pos4.transform.position = end.transform.position;

        Invoke("EndLine", keepTime);

        line.startWidth = lightning.lightningWidth;
        line.endWidth = lightning.lightningWidth;
        
        Vector3 midPoint = (line.GetPosition(0) + line.GetPosition(1)) / 2;
        colliderCur = Instantiate(lineCollider);
        colliderCur.transform.position = midPoint;
        colliderCur.transform.parent = transform;
        Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
        if(capsuleCollider != null){
            capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = true;
            capsuleCollider.radius = line.startWidth/4;
            capsuleCollider.height = direction.magnitude + line.startWidth;
            capsuleCollider.direction = 2;
        }
    }

    public void EndLine () {
        lightning.isSetLight = false;
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
        Destroy(gameObject);
    }
}
