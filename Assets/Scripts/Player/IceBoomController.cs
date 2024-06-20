using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoomController : MonoBehaviour
{
    public GameObject father;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.Freeze();
        }
    }

    public void SetDestroy () {
        Destroy(father);
    }
}
