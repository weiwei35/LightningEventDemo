using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoomController : MonoBehaviour
{
    public GameObject father;
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.Freeze(3);
        }
    }

    public void SetDestroy () {
        Destroy(father);
    }
}
