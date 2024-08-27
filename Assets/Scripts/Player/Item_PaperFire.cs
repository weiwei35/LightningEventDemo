using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Item_PaperFire : MonoBehaviour
{
    [Header("随机范围圆心")]
    public Transform center;
    [Header("随机范围半径")]
    public float radius = 10f;
    public FireBall fireBall;
    public GameObject startPoint;
    public GameObject papers;
    public GameObject firePaper;
    bool isAddPaper = false;
    // Animator anim;
    private void OnEnable() {
        InvokeRepeating("FireBall",1,1);
        papers = GameObject.FindWithTag("Papers");
        center = GameObject.FindGameObjectWithTag("Center").transform;
    }
    private void Update() {
        transform.position = transform.parent.position + new Vector3(0,0.8f,0);
        if(Global.lightningCount > 0 && Global.lightningCount % 20 == 0 && !isAddPaper && !Global.isChangeLevel){
            isAddPaper = true;
            var paper = Instantiate(firePaper,SetRandomPos(),Quaternion.identity);
            paper.transform.parent = papers.transform;
            // paper.transform.position = papers.transform.position;
        }
        if(Global.lightningCount % 20 != 0){
            isAddPaper = false;
        }
    }
    void ResetState(){
        isAddPaper = false;
    }
    GameObject ball;

    void FireBall(){
        var enemys = Transform.FindObjectsOfType<EnemyController>();
        List<EnemyController> enemyInArea = new List<EnemyController>();
        foreach (var item in enemys)
        {
            float distance = Vector3.Distance(item.transform.position,transform.position);
            if(distance <= 5){
                enemyInArea.Add(item);
            }
        }
        if(enemyInArea.Count > 0){
            int id = Random.Range(0,enemyInArea.Count);
            Debug.Log("火球攻击"+enemyInArea[id].name);
            if(enemyInArea[id] != null){
                ball = Instantiate(fireBall.gameObject);
                ball.transform.position = startPoint.transform.position;
                FireBall fire = ball.GetComponent<FireBall>();
                fire.MoveFire(enemyInArea[id]);
            }
        }
    }
    public Vector3 SetRandomPos(){
        if(Global.papersPosList.Count == 0){
            Vector3 pointPos;
            float angle = Random.Range(0,Mathf.PI * 2);
            float radiusRandom = Random.Range(0,radius);
            pointPos = center.position + new Vector3(Mathf.Cos(angle) * radiusRandom,Mathf.Sin(angle) * radiusRandom,0);
            Global.papersPosList.Add(pointPos);
            return pointPos;
        }else{
            Vector3 pointPos;
            
            float minDis;
            do
            {
                List<float> dis = new List<float>();
                float angle = Random.Range(0,Mathf.PI * 2);
                float radiusRandom = Random.Range(0,radius);
                pointPos = center.position + new Vector3(Mathf.Cos(angle) * radiusRandom,Mathf.Sin(angle) * radiusRandom,0);
                foreach (var item in Global.papersPosList)
                {
                    var distance = Vector3.Distance(item,pointPos);
                    dis.Add(distance);
                }
                minDis = dis.Min();
            } while (minDis < 3);
            Global.papersPosList.Add(pointPos);
            return pointPos;
        }
    }
}
