using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 direction;
    public float bulletSpeed;
    public float hurt;
    public float length = 30;
    public Vector3 center = Vector3.zero;
    Rigidbody rb;

    private void Start() {
        center = new Vector3(0,0,transform.position.z);
        rb = GetComponent<Rigidbody>();
    }
    private void Update() {
        float distance = Vector3.Distance(transform.position,center);
        if(distance >= length){
            Destroy(gameObject);
        }
        //速度处理
        if(Global.isSlowDown){
            rb.velocity = direction * (bulletSpeed/10);
        }else{
            rb.velocity = direction * bulletSpeed;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
            PlayerController player = other.GetComponent<PlayerController>();
            if(player.HP > 0)
                player.Hurt(hurt);
        }
    }
}
