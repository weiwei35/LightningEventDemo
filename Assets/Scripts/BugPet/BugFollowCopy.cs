using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class BugFollowCopy : MonoBehaviour
{
    public float followHurt;
    public Transform target;
    public bool isBack = false;
    public GameObject anim;
    void Update()
    {
        EnemyController[] enemys = Transform.FindObjectsOfType<EnemyController>();
        List<EnemyController> enemyListByHP = enemys.OrderBy(p => Vector3.Distance(p.transform.position,transform.position)).ToList();

        if(target == null && !isBack){
            int i = -1;
            do
            {
                i++;
                if(i>enemys.Length-1)
                    break;
            } while (enemyListByHP[i].isFollowHitting);
            if(i<=enemys.Length-1){
                target = enemyListByHP[i].transform;
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
                anim.SetActive(true);
                if(enemyListByHP[i].canHurt)
                    enemyListByHP[i].HurtByFollow(followHurt,HurtType.BugFollow);
            }
        }else if(target != null && !isBack){
            EnemyController enemy = target.gameObject.GetComponent<EnemyController>();
            if(enemy.HP>0){
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
                anim.SetActive(true);
                if(enemy.canHurt)
                    enemy.HurtByFollow(followHurt,HurtType.BugFollow);
            }else{
                int i = -1;
                do
                {
                    i++;
                    if(i>enemys.Length-1)
                        break;
                } while (enemyListByHP[i].isFollowHitting);
                if(i<=enemys.Length-1){
                    target = enemyListByHP[i].transform;
                    Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
                    anim.SetActive(true);
                    if(enemyListByHP[i].canHurt)
                        enemyListByHP[i].HurtByFollow(followHurt,HurtType.BugFollow);
                }
            }
        }else if(isBack){
            anim.SetActive(false);
            transform.position = Vector3.Lerp(transform.position, target.position, 8 * Time.deltaTime);
            if(Vector3.Distance(transform.position, target.position) < 1f){
                Destroy(gameObject);
            }
        }
    }
}
