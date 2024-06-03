using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomController : MonoBehaviour
{
    public float boomHurt = 2f;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.Hurt(boomHurt,HurtType.Boom);
            Debug.Log("爆炸伤害："+ other.name);
        }
    }

    public void SetDestroy () {
        Destroy(gameObject);
    }
}
