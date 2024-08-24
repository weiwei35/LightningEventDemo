using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ConnectLineController : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public Vector3 follow;
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
    public bool isStartLine = false;
    public bool isLastLine = false;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(isStartLine){
            startPoint.gameObject.SetActive(true);
            var start = Instantiate(startPointEP);
            start.transform.position = transform.position;
            start.transform.parent = transform;
        }
        
    }
    private void Update() {
        timeCount += Time.deltaTime;
        if(timeCount > lightning.lightningPreTime){
            lightning.PlayAudio(1);
            // Global.isSlowDown = true;

            timeCount = 0;
            if(follow == null)
                end.transform.position = start.transform.position;
            if(!isChecked){
                isChecked = true;
            }
            DrawLinePoints();
        }
    }
    private void FixedUpdate()
    {
        if(lightningEffect != null){
            lightningEffect.pos1.transform.position = start.transform.position;
            lightningEffect.pos2.transform.position = start.transform.position;
            lightningEffect.pos3.transform.position = end.transform.position;
            lightningEffect.pos4.transform.position = end.transform.position;
        }
        if(colliderCur!= null){
            colliderCur.GetComponent<LineConnectCollider>().start = start.transform.position;
            colliderCur.GetComponent<LineConnectCollider>().end = end.transform.position;
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
        line.startWidth = lightning.lightningWidth;
        line.endWidth = lightning.lightningWidth;

        Invoke("EndLine", keepTime);

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
    }

    public void EndLine () {
        Destroy(gameObject);
    }
}
