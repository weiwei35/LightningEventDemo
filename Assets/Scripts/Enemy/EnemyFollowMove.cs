using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//移动方式：跟随主角；出现方式：屏幕边缘随机位置
public class EnemyFollowMove : EnemyController
{
    SpriteRenderer sprite;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        // SpawnAtRandomEdge();
        SpawnAtRandomCircle();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        FollowMove ();
    }
    public override void Hurt(float hurt,HurtType type)
    {
        base.Hurt(hurt,type);
        // anim.Play("hurt2");
    }
    public override void HurtByCircle(float hurt,HurtType type)
    {
        base.HurtByCircle(hurt,type);
        // anim.Play("hurt2");
    }
    public override void Death(){
        base.Death();
        // anim.Play("hurt2");
    }

    private void FollowMove () {
        if(target != null){
            transform.position = Vector3.MoveTowards(transform.position,target.position,Time.deltaTime * speed);
            if(transform.position.x > target.position.x){
                sprite.flipX = true;
            }else if(transform.position.x < target.position.x){
                sprite.flipX = false;
            }
        }
    }

    void SpawnAtRandomEdge()
    {
        // 获取屏幕中心点
        Camera mainCamera = Camera.main;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 screenCenter = new Vector3(screenWidth * 0.5f, screenHeight * 0.5f,0);
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);

        //获取屏幕 4个顶点
        float cameraHeight = mainCamera.orthographicSize; // 如果是正交摄像机，获取宽度
        float cameraWidth = cameraHeight * mainCamera.aspect; // 获取高度，考虑屏幕比率
        Vector3 topLeft = worldCenter + new Vector3(-cameraWidth, cameraHeight);
        Vector3 topRight = worldCenter + new Vector3(cameraWidth, cameraHeight);
        Vector3 bottomLeft = worldCenter + new Vector3(-cameraWidth, -cameraHeight);
        Vector3 bottomRight = worldCenter + new Vector3(cameraWidth, -cameraHeight);

        //取随机一边，取边上的一点
        int line = Random.Range(0,3);
        Vector3 point = new Vector3();
        switch(line){
            case 0:
            point = GetRandomPointOnLine(topLeft,topRight);
            break;
            case 1:
            point = GetRandomPointOnLine(topRight,bottomRight);
            break;
            case 2:
            point = GetRandomPointOnLine(bottomRight,bottomLeft);
            break;
            case 3:
            point = GetRandomPointOnLine(bottomLeft,topLeft);
            break;
        }
        transform.position = new Vector3(point.x,point.y,-5);
    }

    //取线段上的随机一点
    public static Vector3 GetRandomPointOnLine(Vector3 start, Vector3 end)
    {
        // 计算线段的总长度
        float lineLength = Vector3.Distance(start, end);
        
        // 生成一个0到1之间的随机数
        float randomPoint = Random.Range(0f, 1f);
        
        // 计算随机点在线段上的位置
        Vector3 randomPointOnLine = start + (end - start).normalized * (randomPoint * lineLength);
        return randomPointOnLine;
    }
}
