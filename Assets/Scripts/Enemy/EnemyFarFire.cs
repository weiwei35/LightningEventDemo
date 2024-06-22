using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarFire : MonoBehaviour
{
    public EnemyFarMove enemyFarMove;
    List<GameObject> bulletList = new List<GameObject>();
    public void Fire() {
        var curBullet = GameObjectPoolTool.GetFromPoolForce(true,"Assets/Resources/Bullet.prefab");
        // var curBullet = Instantiate(bullet);
        curBullet.transform.position = enemyFarMove.bulletPos;
        Rigidbody bulletRb = curBullet.GetComponent<Rigidbody>();
        BulletController bulletController = curBullet.GetComponent<BulletController>();
        bulletController.hurt = enemyFarMove.bulletHurt;
        Vector3 direction = enemyFarMove.target.position - transform.position;
        direction.Normalize();
        bulletRb.velocity = direction * enemyFarMove.bulletSpeed;
        bulletController.bulletSpeed = enemyFarMove.bulletSpeed;
        bulletController.direction = direction;
        bulletController.center = enemyFarMove.center.transform.position;
        bulletController.length = 15;
        bulletList.Add(curBullet);
    }
}
