using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperIce : PaperModel
{
    public float overTime;
    public float countTime = 0;
    public GameObject singleC;
    public GameObject doubleC;
    private void Start() {
    }
    private void Update() {
        if(isOverLoad && !Global.isSlowDown){
            countTime += Time.deltaTime;
        }
        if(countTime>=overTime && isOverLoad){
            countTime = 0;
            isOverLoad = false;
            singleC.SetActive(true);
            doubleC.SetActive(false);
            EndOverLoadFun();
        }
    }
    public override void OverLoadFun(){
        base.OverLoadFun();
        singleC.SetActive(false);
        doubleC.SetActive(true);
    }
}
