using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyRush : EnemyController
{
    public SpriteRenderer sprite;
    public Transform mainEnemy;
    public Animator animator;
    Vector3 rushDis;
    bool isRushing = false;
    bool isColdTime = false;
    float coldTime = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SpawnAtRandomCircle();
        // sprite = GetComponent<SpriteRenderer>();
        // animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(!isInBlackHall && !isBoom && !isRushing)
        {
            FollowMove();
        }
        // 获取当前物体的欧拉角
        Vector3 currentRotation = mainEnemy.rotation.eulerAngles;
        if(transform.position.x > target.position.x){
                currentRotation.z = Mathf.Clamp(currentRotation.z, 150, 210);
            }else if(transform.position.x < target.position.x){
                if(currentRotation.z < 180)
                    currentRotation.z = Mathf.Clamp(currentRotation.z, 0, 30);
                if(currentRotation.z > 180)
                    currentRotation.z = Mathf.Clamp(currentRotation.z, 330, 360);
            }

        // 应用限制后的欧拉角
        mainEnemy.rotation = Quaternion.Euler(currentRotation);
        if(isColdTime){
            coldTime += Time.deltaTime;
        }
        if(coldTime > 3){
            isColdTime = false;
        }

        if(Vector3.Distance(target.position,transform.position) < 5 && !isRushing && !isColdTime){
            //向主角方向冲刺
            isRushing = true;
            RushToPlayer();
        }
    }

    private void FollowMove () {
        if(target != null){
            transform.position = Vector3.MoveTowards(transform.position,target.position+new Vector3(0,1,0),Time.deltaTime * speed);
            Vector2 v = target.transform.position - transform.position;
            var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            var trailRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            mainEnemy.rotation = trailRotation;
            if(transform.position.x > target.position.x){
                sprite.flipX = false;
                sprite.flipY = true;
            }else if(transform.position.x < target.position.x){
                sprite.flipY = false;
                sprite.flipX = false;
            }
        }
    }

    void RushToPlayer(){
        rushDis = target.position - transform.position;
        rushDis.Normalize();
        transform.DOMove(transform.position,1).OnComplete( ()=>
            {
                animator.SetBool("rush",true);
                transform.DOMove(transform.position+rushDis*5,0.5f).OnComplete( ()=>
                    {
                        animator.SetBool("rush",false);

                        isColdTime = true;

                        isRushing = false;
                    }
                );
            }
        );
        
    }
}
