using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BugFollowCopy : MonoBehaviour
{
    public float followHurt;
    public Transform target;
    void Update()
    {
        EnemyController[] enemys = Transform.FindObjectsOfType<EnemyController>();
        if(target == null){
            int i = 0;
            do
            {
                i++;
                if(i>enemys.Length-1)
                    break;
            } while (enemys[i].isFollowHitting);
            if(i<=enemys.Length-1){
                target = enemys[i].transform;
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
                enemys[i].HurtByFollow(followHurt,HurtType.BugFollow);
            }
        }else{
            EnemyController enemy = target.gameObject.GetComponent<EnemyController>();
            if(enemy.HP>0){
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
                enemy.HurtByFollow(followHurt,HurtType.BugFollow);
            }else{
                int i = 0;
                do
                {
                    i++;
                    if(i>enemys.Length-1)
                        break;
                } while (enemys[i].isFollowHitting);
                if(i<=enemys.Length-1){
                    target = enemys[i].transform;
                    Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
                    enemys[i].HurtByFollow(followHurt,HurtType.BugFollow);
                }
            }
        }
    }
}
