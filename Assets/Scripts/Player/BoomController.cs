using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomController : MonoBehaviour
{
    public float boomHurt = 2f;
    public GameObject father;
    private void OnEnable() {
        Invoke("SetDestroy",0.5f);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if(player.isLightningBoomPlayer){
                // boomHurt = boomHurt + boomHurt*((player.HP-player.GetHurtCount())/player.HP);
                boomHurt = player.HP/2;
            }
            enemy.Hurt(boomHurt,HurtType.Boom);
            Debug.Log("爆炸伤害："+ other.name);
        }
    }

    public void SetDestroy () {
        Destroy(father);
    }
}
