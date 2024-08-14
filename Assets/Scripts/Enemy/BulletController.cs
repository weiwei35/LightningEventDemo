using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 direction;
    public float bulletSpeed;
    public float hurt;
    public float length = 30;
    public Vector3 center;
    Rigidbody rb;
    public float angle;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        GameObject bulletFather = GameObject.FindWithTag("BulletFather");
        transform.SetParent(bulletFather.transform);
        direction.z = 0;
        float angle = Vector3.SignedAngle(Vector3.up,-direction,Vector3.forward); //得到围绕z轴旋转的角度
	    Quaternion rotation = Quaternion.Euler(0, 0, angle); //将欧拉角转换为四元数
	    transform.rotation = rotation;
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
            if(!player.isDead){
                player.Hurt(hurt);
            }
        }
    }
}
