using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PaperHP : PaperModel
{
    public float overTime;
    public float countTime = 0;
    Animator anim;
    public GameObject treeEffect;
    private void Start() {
        anim = GetComponent<Animator>();
        InvokeRepeating("HPRecover",4,4);
    }
    private void Update() {
        if(isOverLoad){
            countTime += Time.deltaTime;
        }
        if(countTime>=overTime && isOverLoad){
            countTime = 0;
            isOverLoad = false;
            anim.speed = 1;

            CancelInvoke("HPRecover");
        
            InvokeRepeating("HPRecover",0,4);
        }
    }
    void HPRecover(){
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.OutsideRecoveryHP(Mathf.Max(Mathf.Ceil(player.GetHurtCount() * 0.1f), 1));
        treeEffect.SetActive(true);
        Invoke("SetTreeEffect",1f);

    }
    void SetTreeEffect(){
        treeEffect.SetActive(false);
    }
    public override void OverLoadFun(){
        anim.speed = 2;
        CancelInvoke("HPRecover");
        
        InvokeRepeating("HPRecover",0,2);
    }
}
