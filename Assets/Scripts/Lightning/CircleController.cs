using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    [HideInInspector]
    public Material m;
    // [HideInInspector]
    public LineController line;
    public MirrorLineController mirrorLine;
    public GameObject startPoint;//雷点
    GameObject player;
    GameObject playerCopy;
    GameObject[] playerOnceCopy;
    LightningController lightning;
    List<Vector3> points = new List<Vector3>();
    List<GameObject> lines = new List<GameObject>();
    List<GameObject> lineCopys = new List<GameObject>();
    // List<GameObject> starts = new List<GameObject>();
    List<GameObject> circleLight = new List<GameObject>();
    // bool canFollow = false;
    Vector3 center;
    [Header("半径")]
    public float radius;
    [Header("圆心")]
    public GameObject centerPos;
    [Header("反射雷出现时长")]
    public float mirroeStartTime;
    [Header("反射雷停留时长")]
    public float mirroeKeepTime;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        center = centerPos.transform.position;
        DrawCircle ();
        lightning = GetComponent<LightningController>();
        // //初始化界面边缘的雷点
        // for (int i = 0; i < lightning.lightningCount; i++)
        // {
        //     var start = Instantiate(startPoint);
        //     start.transform.position = Vector3.zero;
        //     circleLight.Add(start);
        // }
    }

    private void Update() {
        //雷电跟随角色
        foreach (var item in lines)
        {
            if(item != null){
                LineController lineController = item.GetComponent<LineController>();
                if(lineController.follow != null)
                    lineController.end.transform.position = lineController.follow.transform.position + new Vector3(0,1f,0);
            }
        }
        // foreach (var item in lineCopys)
        // {
        //     if(item != null){
        //         MirrorLineController lineController = item.GetComponent<MirrorLineController>();
        //         if(lineController.follow != null)
        //             lineController.start = lineController.follow.transform.position + new Vector3(0,1f,0);
        //     }
        // }
        // //当有雷点出现时，界面边缘的雷点跟随显示
        // if(canFollow)
        //     GetCamLight();
    }

    //画结界
    public void DrawCircle () {
        LineRenderer line = GetComponent<LineRenderer>();

        line.material = m;
        line.loop = true;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.positionCount = 360;

        float angleOnce = 360/line.positionCount;
        Vector3[] circlePoints = new Vector3[line.positionCount];

        Vector3 vecStart = new Vector3(0,radius,0);

        for (int i = 0; i < line.positionCount; i++)
        {
            circlePoints[i] = Quaternion.Euler(0,0,angleOnce*i)*vecStart+center;
        }

        line.SetPositions(circlePoints);

    }

    //在圆上取count个数的随机点
    public void RandomPoints (float count) {
        points.Clear();
        //特殊雷点：主角和随机一个怪物连接的射线，左右夹角各15°，中随机一个点位
        Vector3 startPos = new Vector3(player.transform.position.x,player.transform.position.y,0);
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        Vector3 endPos;
        if(enemys.Count() > 0){
            int randomEnemyId = Random.Range(0,enemys.Count());
            GameObject randomEnemy = enemys[randomEnemyId].gameObject;
            endPos = new Vector3(randomEnemy.transform.position.x,randomEnemy.transform.position.y,0);
        }else{
            endPos = Vector3.zero;
        }
        
        Ray ray = new Ray(startPos, endPos - startPos);

        Vector3 intersectionPoint1 = CalculateCircleIntersection(ray.origin, -ray.direction, center, radius);//与随机一怪物相连，和圆的交点
        if (intersectionPoint1 != Vector3.zero) // 如果计算出交点，并且交点不在圆的外部
        {
            // Debug.DrawLine(ray.origin, intersectionPoint1, Color.red, 10f); // 在场景中绘制交点
            Quaternion rotation1 = Quaternion.Euler(0, 0, 15);
            // 计算新的方向向量
            Vector3 direction1 = rotation1*(endPos - startPos);
            Vector3 intersectionPoint2 = CalculateCircleIntersection(startPos, -direction1, center, radius);//正15度，和圆的交点
            // Debug.DrawLine(startPos, intersectionPoint2, Color.red, 10f); // 在场景中绘制交点
            Quaternion rotation2 = Quaternion.Euler(0, 0, -15);
            // 计算新的方向向量
            Vector3 direction2 = rotation2*(endPos - startPos);
            Vector3 intersectionPoint3 = CalculateCircleIntersection(startPos, -direction2, center, radius);//负15度，和圆的交点
            // Debug.DrawLine(startPos, intersectionPoint3, Color.red, 10f); // 在场景中绘制交点
            // 你可以在这里处理交点信息
            float angle =RandomAngle(intersectionPoint2,intersectionPoint3);
            Vector3 point = center + new Vector3(Mathf.Cos(angle * Mathf.PI/180) * radius,Mathf.Sin(angle * Mathf.PI/180) * radius,-5);
            // var start = Instantiate(startPointEP);
            // start.transform.position = point;
            // starts.Add(start);
            points.Add(point);
        }
        //普通雷点
        for (int i = 0; i < count-1;)
        {
            bool canSave = true;
            Vector3 pointCur = GetPoint();
            foreach (var item in points)
            {
                // 计算圆心到点的方向向量
                Vector2 direction1 = item - center;
                // 标准化方向向量以便于计算角度
                direction1.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;

                // 计算圆心到点的方向向量
                Vector2 direction2 = pointCur - center;
                // 标准化方向向量以便于计算角度
                direction2.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;

                if(Mathf.Abs(angle1-angle2) < 5){
                    canSave = false;
                    Debug.Log("两雷点离得太近");
                }
            }
            if(canSave){
                points.Add(pointCur);
                // var start = Instantiate(startPoint);
                // start.transform.position = pointCur;
                // starts.Add(start);
                i++;
            }
        }
        SetLines(lightning.startTime,lightning.keepTime,points);
        // GetCamLight();
    }
        //在圆上取count个数的随机点
    public void RandomPointsCopy (float count) {
        List<Vector3> pointsCopy = new List<Vector3>();
        pointsCopy.Clear();
        //普通雷点
        for (int i = 0; i < count;)
        {
            bool canSave = true;
            Vector3 pointCur = GetPoint();
            foreach (var item in pointsCopy)
            {
                // 计算圆心到点的方向向量
                Vector2 direction1 = item - center;
                // 标准化方向向量以便于计算角度
                direction1.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;

                // 计算圆心到点的方向向量
                Vector2 direction2 = pointCur - center;
                // 标准化方向向量以便于计算角度
                direction2.Normalize();
                // 计算点和圆心连线与正X轴的夹角（角度值）
                float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;

                if(Mathf.Abs(angle1-angle2) < 15){
                    canSave = false;
                    Debug.Log("两雷点离得太近");
                }
            }
            if(canSave){
                pointsCopy.Add(pointCur);
                // var start = Instantiate(startPoint);
                // start.transform.position = pointCur;
                // starts.Add(start);
                i++;
            }
        }
        SetLinesCopy(mirroeStartTime,mirroeStartTime,pointsCopy);
        // GetCamLight();
    }
    //在圆上取随机点
    Vector3 GetPoint(){
        float angle = Random.Range(0,Mathf.PI * 2);
        Vector3 point = center + new Vector3(Mathf.Cos(angle) * radius,Mathf.Sin(angle) * radius,-5);
        return point;
    }

    //计算射线与圆的交点
    Vector3 CalculateCircleIntersection(Vector3 rayOrigin, Vector3 rayDirection, Vector3 circleCenter, float circleRadius)
    {
        Vector3 relativeOrigin = rayOrigin - circleCenter;
        float a = Vector3.Dot(rayDirection, rayDirection);
        float b = 2 * Vector3.Dot(rayDirection, relativeOrigin);
        float c = Vector3.Dot(relativeOrigin, relativeOrigin) - circleRadius * circleRadius;
 
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            return Vector3.zero; // 无交点
        }
 
        float t = (-b - Mathf.Sqrt(discriminant)) / (2 * a); // 求解二次方程
        return rayOrigin + rayDirection * t; // 计算交点
    }

    //圆上两点间取一个随机点
    float RandomAngle(Vector3 point1,Vector3 point2){
        // 计算圆心到点的方向向量
        Vector2 direction1 = point1 - center;
 
        // 标准化方向向量以便于计算角度
        direction1.Normalize();
 
        // 计算点和圆心连线与正X轴的夹角（角度值）
        float angle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;
        // Debug.Log(angle1);

        // 计算圆心到点的方向向量
        Vector2 direction2 = point2 - center;
 
        // 标准化方向向量以便于计算角度
        direction2.Normalize();
 
        // 计算点和圆心连线与正X轴的夹角（角度值）
        float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
        // Debug.Log(angle2);
        float angle;
        if(angle2 > angle1)
            angle = Random.Range(angle1,angle2);
        else
            angle = Random.Range(angle2,angle1);
        // Debug.Log(angle);
        return angle;
    }

    //从随机点生成连向主角的线段
    public void SetLines (float startTime,float keepTime,List<Vector3> points) {
        float timeOffset = 0;
        timeOffset = (lightning.lightningPreTime/2)/points.Count;
        int i = 0;
        foreach(var point in points){
            var lineCur = Instantiate(line.gameObject);
            lineCur.transform.position = point;
            LineController lineController = lineCur.GetComponent<LineController>();
            lineController.start.transform.position = point;
            lineController.end.transform.position = player.transform.position;
            lineController.startTime = startTime;
            lineController.keepTime = keepTime;
            lineController.follow = player;
            lineController.showTime = i * timeOffset;
            i++;
            lines.Add(lineCur.gameObject);
        }
    }
    public void SetLinesCopy (float startTime,float keepTime,List<Vector3> points) {
        foreach(var point in points){
            var lineCur = Instantiate(mirrorLine.gameObject);
            lineCur.transform.position = player.transform.position;
            MirrorLineController lineController = lineCur.GetComponent<MirrorLineController>();
            lineController.start.transform.position = player.transform.position + new Vector3(0,1f,0);
            lineController.end.transform.position = point;
            lineController.startTime = startTime;
            lineController.keepTime = keepTime;
            lineController.showTime = 0;
            lineController.follow = player;
            lineController.timeCount = lightning.lightningPreTime +1;
            lineCopys.Add(lineCur.gameObject);
        }
    }
}
