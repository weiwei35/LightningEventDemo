using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour
{
    LightningController lightning;
    public Vector3 start;
    public Vector3 end;
    private void OnTriggerEnter(Collider other) {
        lightning = FindObjectOfType<LightningController>();
        if(other.gameObject.layer == 6){
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            if(enemy != null && !enemy.isHitting)
            {
                Debug.Log("雷电攻击：" + other.name);
                lightning.HurtEnemy(enemy,HurtType.Lightning);
            }
        }
    }
}
