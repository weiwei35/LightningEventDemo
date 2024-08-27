using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public float moveSpeed = 8f; // 移动速度
    private int currentWaypoint = 0; // 当前路径点索引
    public List<Vector3> path; // 路径数组
    GameObject enemy;
    Tween tween1;
    Tween tween2;
    bool startFade = false;
    Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypoint < path.Count)
        {
            // 移动物体到下一个路径点
            tween1 = transform.DOMove(path[currentWaypoint],0.002f).OnComplete( ()=>{
                currentWaypoint++;
            });
            // transform.position = Vector3.MoveTowards(transform.position, path[currentWaypoint], moveSpeed * Time.deltaTime);
 
            // 当物体到达路径点时，更新索引
            // if (transform.position == path[currentWaypoint])
            // {
            //     currentWaypoint++;
            // }
        }else{
            if(!startFade){
                Fade();
                startFade = true;
            }
        }
    }
    void Fade(){
        anim.Play();
        if(enemy != null){
            tween2 = transform.DOMove(enemy.transform.position,0.1f).OnComplete( ()=>{
                if(enemy != null){
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    enemyController.Hurt(5,HurtType.Sword);
                }
                Destroy(gameObject);
            });
        }
    }
    private void OnDestroy() {
        if(tween1 != null){
            tween1.Kill();
        }
        if(tween2 != null){
            tween2.Kill();
        }
    }
    

    public void SetCircleLine(Vector3 start, GameObject end){
        if(end != null){
            enemy = end;
            Vector3 point0 = start; // 起始点
            Vector3 point3 = end.transform.position; // 结束点
            Vector3 point1 = CenterPoint(point0,point3); // 控制点
            Vector3 point2 = SidePoint(point0,point3); // 控制点
    
            for (int i = 0; i < 20; i++)
            {
                float t = i / (float)(20 - 1); // 参数t，从0到1，分割曲线
                Vector3 point = CalculateCubicBezierPoint(point0,point1, point2, point3, t);
                path.Add(point);
            }
        }
        
    }

    //取两点之间的控制点
    Vector3 CenterPoint(Vector3 pointA, Vector3 pointB){
        Vector3 vectorAB = pointB - pointA; // 两点间的向量
        Vector3 direction = vectorAB.normalized; // 方向向量
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up); // 垂直于AB的向量
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, 45) * direction; // 旋转方向向量
        Vector3 pointC = pointA + rotatedDirection * (vectorAB.magnitude); // 新的终点坐标

        float angle = Random.Range(0,Mathf.PI * 2);
        Vector3 point = pointC + new Vector3(Mathf.Cos(angle) * 8,Mathf.Sin(angle) * 8,0);
        Vector3 pointD = new Vector3(point.x,point.y,-5);
 
        // Debug.DrawLine(pointA, pointB, Color.blue); // 绘制起点和终点之间的连线
        // Debug.DrawLine(pointA, pointC, Color.red); // 绘制起点和旋转后终点之间的连线
        // Debug.Log("1:"+pointC);
        return pointD;
    }
    //取两点之间的控制点
    Vector3 SidePoint(Vector3 pointB, Vector3 pointA){
        Vector3 vectorAB = pointB - pointA; // 两点间的向量
        Vector3 direction = vectorAB.normalized; // 方向向量
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up); // 垂直于AB的向量
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, 45) * direction; // 旋转方向向量
        Vector3 pointC = pointA + rotatedDirection * (vectorAB.magnitude/3); // 新的终点坐标

        float angle = Random.Range(0,Mathf.PI * 2);
        Vector3 point = pointC + new Vector3(Mathf.Cos(angle) * 8,Mathf.Sin(angle) * 8,0);
        Vector3 pointD = new Vector3(point.x,point.y,-5);
 
        // Debug.DrawLine(pointA, pointB, Color.blue); // 绘制起点和终点之间的连线
        // Debug.DrawLine(pointA, pointC, Color.red); // 绘制起点和旋转后终点之间的连线
        // Debug.Log("2:"+pointC);
        return pointD;
    }
    //贝塞尔曲线
    Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * u * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += tt * t * p3;
        return p;
    }
}
