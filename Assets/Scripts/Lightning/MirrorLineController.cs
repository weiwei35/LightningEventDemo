using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MirrorLineController : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public float showTime = 0;
    public float startTime = 0.1f;
    public float keepTime = 0.5f;
    LineRenderer line;
    public float timeCount = 0;
    public GameObject follow;
    EnemyController enemy;
    LightningController lightning;
    PlayerController player;
    Vector3 midPoint;
    public GameObject lineCollider;
    CapsuleCollider capsule;
    public LightningEffect lightningAsset;
    LightningEffect lightningEffect;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lightning = FindObjectOfType<LightningController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        DrawLinePoints();
    }
    private void Update() {
            
    }
    private void FixedUpdate()
    {
        start.transform.position = follow.transform.position + new Vector3(0,1f,0);
        if(lightningEffect != null){
            lightningEffect.pos1.transform.position = start.transform.position;
            lightningEffect.pos2.transform.position = start.transform.position;
            lightningEffect.pos3.transform.position = end.transform.position;
            lightningEffect.pos4.transform.position = end.transform.position;
        }
        line.SetPosition(0,start.transform.position);
        line.SetPosition(1,end.transform.position);
        if(lineCollider!= null)
        {
            midPoint = (line.GetPosition(0) + line.GetPosition(1)) / 2;
            lineCollider.transform.position = midPoint;
            Vector3 direction = line.GetPosition(1) - line.GetPosition(0);
            capsule.isTrigger = true;
            capsule.radius = line.startWidth/2;
            capsule.height = direction.magnitude + line.startWidth;
            capsule.direction = 2;
            if(Vector3.Distance(line.GetPosition(1),line.GetPosition(0)) != 0)
            {Quaternion lookRotation = Quaternion.LookRotation(line.GetPosition(1) - line.GetPosition(0));
            lineCollider.transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x,lookRotation.eulerAngles.y,lookRotation.eulerAngles.z);}
        }
    }
    public void DrawLinePoints() {
        // canMove = false;
        
        lightning.PlayAudio(6);
        lightningEffect = Instantiate(lightningAsset);
        lightningEffect.transform.parent = transform;
        lightningEffect.pos1.transform.position = start.transform.position;
        lightningEffect.pos2.transform.position = start.transform.position;
        lightningEffect.pos3.transform.position = end.transform.position;
        lightningEffect.pos4.transform.position = end.transform.position;
        line.startWidth = lightning.lightningWidth;
        line.endWidth = lightning.lightningWidth;
        
        Invoke("EndLine", keepTime);

        lineCollider = Instantiate(lineCollider);
        lineCollider.transform.position = transform.position;
        lineCollider.transform.parent = transform;
        capsule = lineCollider.GetComponent<CapsuleCollider>();
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
