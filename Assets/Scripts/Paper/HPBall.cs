using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HPBall : MonoBehaviour
{
    public float hp;
    PlayerController player;
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update() {
        if(player.needRecovery){
            if(Vector3.Distance(transform.position,player.transform.position) <= 2){
                RecoveryHP();
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            PlayerController playerCollider = other.GetComponent<PlayerController>();
            playerCollider.OutsideRecoveryHP(hp);

            Destroy(gameObject);
        }
    }
    void RecoveryHP(){
        transform.DOMove(player.transform.position,0.2f).OnComplete( ()=>{
            player.OutsideRecoveryHP(hp);
            Destroy(gameObject);
        });
    }
}
