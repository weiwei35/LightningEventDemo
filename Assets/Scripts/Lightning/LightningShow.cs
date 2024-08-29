using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class LightningShow : MonoBehaviour
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
    Vector3 endPos1;
    public bool isRush = false;
    public GameObject lineRush;
    public bool canCopy = true;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playsound = false;
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
    bool playsound = false;
    private void Update() {
        timeCount += Time.deltaTime;
        if(timeCount > lightning.lightningPreTime - 0.5f){
            if(Input.GetKeyDown(KeyCode.Space) && !Global.isSlowDown && player.rushing && player.skill_rush){
                endPos1 = player.transform.position;
                isRush = true;
                Global.isSlowDown = true;
            }
        }
        if(timeCount > lightning.lightningPreTime - 0.1f){
        }
        if(timeCount > lightning.lightningPreTime - 0.1f && !playsound){
            lightning.PlayLightningAudio();
            playsound = true;
        }
        if(timeCount > lightning.lightningPreTime){
            timeCount = 0;
            if(follow == null)
                end.transform.position = start.transform.position;
            if(!isChecked && canCopy){
                isChecked = true;
                CheckCopy();
            }
            DrawLinePoints();
        }
        if(lightningEffect != null && startTime <= 0.2f){
            lightningEffect.pos1.transform.position = start.transform.position;
            lightningEffect.pos2.transform.position = start.transform.position;
            lightningEffect.pos3.transform.position = end.transform.position;
            lightningEffect.pos4.transform.position = end.transform.position;
        }
    }
    private void FixedUpdate()
    {
        if(!isRush)
        {
        if(colliderCur!= null){
            colliderCur.GetComponent<LineColliderMain>().start = start.transform.position;
            colliderCur.GetComponent<LineColliderMain>().end = end.transform.position;
            for(int i = 0; i < line.positionCount-1; i++)
            {
                Vector3 midPoint = (line.GetPosition(i) + line.GetPosition(i+1)) / 2;
                colliderCur.transform.position = midPoint;
                colliderCur.transform.parent = transform;
                Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
                capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
                if(capsuleCollider != null){
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
        }
        line.SetPosition(0,start.transform.position);
        line.SetPosition(1,end.transform.position);
        }
        else{
            // end.transform.position = follow.transform.position + new Vector3(0,1f,0);
            end.transform.position = endPos1;
            if(colliderCur!= null){
                colliderCur.GetComponent<LineColliderMain>().start = start.transform.position;
                colliderCur.GetComponent<LineColliderMain>().end = end.transform.position;
                for(int i = 0; i < line.positionCount-1; i++)
                {
                    Vector3 midPoint = (line.GetPosition(i) + line.GetPosition(i+1)) / 2;
                    colliderCur.transform.position = midPoint;
                    colliderCur.transform.parent = transform;
                    Vector3 direction = line.GetPosition(0) - line.GetPosition(1);
                    capsuleCollider = colliderCur.GetComponent<CapsuleCollider>();
                    if(capsuleCollider != null){
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
            }
            line.SetPosition(0,start.transform.position);
            // line.SetPosition(1,end.transform.position);
            line.SetPosition(1,endPos1);
        }
    }

    public void CheckCopy() {
        if(player.GetComponent<PlayerController>().isOnceLightningCopy || player.GetComponent<PlayerController>().isOnceTimeCopy){
            GameObject[] playerOnceCopy = GameObject.FindGameObjectsWithTag("PlayerOnceCopy");
            if(playerOnceCopy.Length > 0){
                foreach (var item in playerOnceCopy)
                {
                    var lineCurCopy = Instantiate(copyLine);
                    lineCurCopy.transform.position = transform.position;
                    CopyLineController lineControllerCopy = lineCurCopy.GetComponent<CopyLineController>();
                    lineControllerCopy.start.transform.position = start.transform.position;
                    lineControllerCopy.end.transform.position = new Vector3(item.transform.position.x,item.transform.position.y,-5) + new Vector3(0,0.6f,0);
                    lineControllerCopy.startTime = startTime;
                    lineControllerCopy.keepTime = keepTime;
                    lineControllerCopy.follow = item;
                    lineControllerCopy.timeCount = lightning.lightningPreTime+1;
                }
            }
        }
        if(player.GetComponent<PlayerController>().isCircleCopy){
                GameObject playerCopy = GameObject.FindGameObjectWithTag("PlayerCopy");
                var lineCurCopy = Instantiate(copyLine);
                lineCurCopy.transform.position = transform.position;
                CopyLineController lineControllerCopy = lineCurCopy.GetComponent<CopyLineController>();
                lineControllerCopy.start.transform.position = start.transform.position;
                lineControllerCopy.end.transform.position = new Vector3(playerCopy.transform.position.x,playerCopy.transform.position.y,-5) + new Vector3(0,0.6f,0);
                lineControllerCopy.startTime = startTime;
                lineControllerCopy.keepTime = keepTime;
                lineControllerCopy.follow = playerCopy;
                lineControllerCopy.timeCount = lightning.lightningPreTime+1;
        }
    }
    Tween tween1;
    Tween tween2;

    public void DrawLinePoints() {
        lightningEffect = Instantiate(lightningAsset);
        lightningEffect.transform.parent = transform;
        if(!isRush)
        {
            if(startTime >= 0.2f){
                lightningEffect.pos1.transform.position = start.transform.position;
                lightningEffect.pos2.transform.position = start.transform.position;
                lightningEffect.pos3.transform.position = start.transform.position;
                lightningEffect.pos4.transform.position = start.transform.position;
                tween1 = lightningEffect.pos1.transform.DOMove(end.transform.position,startTime);
                tween2 = lightningEffect.pos2.transform.DOMove(end.transform.position,startTime);
            }else{
                lightningEffect.pos1.transform.position = start.transform.position;
                lightningEffect.pos2.transform.position = start.transform.position;
                lightningEffect.pos3.transform.position = end.transform.position;
                lightningEffect.pos4.transform.position = end.transform.position;
            }
        }else{
            keepTime *= 2;
            lightningEffect.pos1.transform.position = start.transform.position;
            lightningEffect.pos2.transform.position = start.transform.position;
            lightningEffect.pos3.transform.position = endPos1;
            lightningEffect.pos4.transform.position = endPos1;
            StartCoroutine(SetRushLine());
        }
        Invoke("EndLine", keepTime+startTime);
        Invoke("SetEndLine", 0);

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
    IEnumerator SetRushLine(){
        yield return new WaitForSeconds(startTime);
        var lineCur = Instantiate(lineRush);
        lineCur.transform.position = endPos1;
        LightningShow lineController = lineCur.GetComponent<LightningShow>();
        lineController.start.transform.position = endPos1;
        lineController.end.transform.position = player.transform.position + new Vector3(0,1f,0);;
        lineController.startTime = startTime;
        lineController.keepTime = keepTime;
        lineController.follow = player.gameObject;
        lineController.isRush = false;
        lineController.timeCount = lightning.lightningPreTime+1;
        lineController.canCopy = false;
    }

    public void EndLine () {
        
        Global.isSlowDown = false;
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

    public void SetEndLine () {
        if(!Global.isChangeLevel)
            lightning.HurtPlayer();
        lightning.isSetLight = true;
    }

    private void OnDestroy() {
        if(tween1 != null){
            tween1.Kill();
        }
        if(tween2!= null){
            tween2.Kill();
        }
        isRush = false;
    }
}
