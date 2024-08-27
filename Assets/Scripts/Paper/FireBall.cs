using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Tween tweener;
    private void OnDestroy() {
        if(tweener != null){
            tweener.Kill();
        }
    }
    public void MoveFire(EnemyController enemy){
        Vector3 pos = enemy.transform.position;
        tweener = transform.DOMove(pos,0.2f).OnComplete(()=>{
            if(enemy != null){
                enemy.HurtByBugAttack(5f,HurtType.PaperFireBall);
            }
            Destroy(gameObject);
        });
    }
}
