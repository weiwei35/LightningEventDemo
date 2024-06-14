using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperProtect : PaperModel
{
    public float overTime;
    public float countTime = 0;
    Animator anim;
    public GameObject protectEffect;
    private void Start() {
        anim = GetComponent<Animator>();
        InvokeRepeating("ProtectRecover",5,5);
    }
    private void Update() {
        if(isOverLoad){
            countTime += Time.deltaTime;
        }
        if(countTime>=overTime && isOverLoad){
            countTime = 0;
            isOverLoad = false;
            anim.speed = 1;

            CancelInvoke("ProtectRecover");
        
            InvokeRepeating("ProtectRecover",0,5);
        }
    }
    void ProtectRecover(){
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.OutsideRecoveryProtect(Mathf.Max(Mathf.Ceil(player.GetHurtProtectCount() * 0.1f), 1));
        protectEffect.SetActive(true);
        Invoke("SetTreeEffect",2);
    }
    void SetTreeEffect(){
        protectEffect.SetActive(false);
    }
    public override void OverLoadFun(){
        anim.speed = 2;
        CancelInvoke("ProtectRecover");
        
        InvokeRepeating("ProtectRecover",0,2);
    }
}
