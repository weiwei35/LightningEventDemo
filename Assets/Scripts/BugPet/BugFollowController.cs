using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BugFollowController : BugController
{
    [Header("追踪时长")]
    public float followTime = 8;
    float countTime = 0;
    [Header("追踪个数")]
    public int bugCount = 3;
    [Header("攻击伤害")]
    public float followHurt = 2;
    [Header("追击方式:1最近;2血量最多")]
    public int followType = 2;
    public MeshRenderer mesh;
    public BugFollowCopy bugFollowPre;
    List<BugFollowCopy> bugCopys = new List<BugFollowCopy>();
    bool startFollow = false;
    bool trueStart = false;
    public override void Update()
    {
        base.Update();
        if(energyCurrent >= energy){
            canRecoverEnergy = false;
            energyCurrent = 0;
            trueStart = true;
        }
        if(trueStart){
            if(followType == 2)
                SetBugsFollow();
            if(followType == 1)
                SetBugsFollowNear();
        }
        if(startFollow && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime >= followTime){
            countTime = 0;
            startFollow = false;
            isTriggered = false;
            canRecoverEnergy = true;
            // Debug.Log("开始充能");
            mesh.enabled = true;
            foreach (var item in bugCopys)
            {
                item.isBack = true;
                // Destroy(item.gameObject);
                item.target = transform;
            }
            bugCopys.Clear();
            var enemys = Transform.FindObjectsOfType<EnemyController>();
            foreach (var item in enemys)
            {
                item.isFollowHitting = false;
            }
        }
    }

    void SetBugsFollow(){
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        if(enemys.Length > 0){
            trueStart = false;
            isTriggered = true;
            mesh.enabled = false;
            startFollow = true;
            var enemyListByHP = enemys.OrderByDescending(p => p.HP).Take(bugCount).ToList();
            foreach (var item in enemyListByHP)
            {
                // Debug.Log(item.name +":"+item.HP);
                BugFollowCopy bugCopy = Instantiate(bugFollowPre);
                // bugCopy.transform.parent = transform.parent;
                bugCopy.transform.position = transform.position;
                // BugFollowController bugCopyFollow = bugCopy.GetComponent<BugFollowController>();
                bugCopy.target = item.transform;
                bugCopy.followHurt = followHurt;
                bugCopys.Add(bugCopy);
            }
        }
    }
    void SetBugsFollowNear(){
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        if(enemys.Length > 0){
            trueStart = false;
            isTriggered = true;
            mesh.enabled = false;
            startFollow = true;
            
            EnemyController enemy = null;
            float minDistance = 100;
            foreach (var item in enemys)
            {
                float distance = Vector3.Distance(item.transform.position,transform.position);
                if(minDistance > distance){
                    enemy = item;
                    minDistance = distance;
                }
            }
            // var enemyListByHP = enemys.OrderByDescending(p => p.HP).Take(bugCount).ToList();
            if(enemy != null)
            {
                // Debug.Log(item.name +":"+item.HP);
                BugFollowCopy bugCopy = Instantiate(bugFollowPre);
                // bugCopy.transform.parent = transform.parent;
                bugCopy.transform.position = transform.position;
                // BugFollowController bugCopyFollow = bugCopy.GetComponent<BugFollowController>();
                bugCopy.target = enemy.transform;
                bugCopy.followHurt = followHurt;
                bugCopys.Add(bugCopy);
            }
        }
    }
}
