using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperController : MonoBehaviour
{
    [Header("随机范围圆心")]
    public Vector3 center = Vector3.zero;
    [Header("随机范围半径")]
    public float radius = 10f;
    [Header("召唤频次")]
    public int count = 10;
    [Header("召唤对象")]
    public GameObject paperPre;
    [HideInInspector]
    public int countCurrent = 0;
    // Start is called before the first frame update
    void Start()
    {
        // transform.position = SetRandomPos();
        countCurrent = count;
    }

    // Update is called once per frame
    void Update()
    {
        if(countCurrent >= count){
            countCurrent = 0;
            //生成新的
            var paper = Instantiate(paperPre);
            paper.transform.position = SetRandomPos();
            paper.transform.parent = transform;
        }
    }

    public Vector3 SetRandomPos(){
        float angle = Random.Range(0,Mathf.PI * 2);
        float radiusRandom = Random.Range(0,radius);
        Vector3 point = center + new Vector3(Mathf.Cos(angle) * radiusRandom,Mathf.Sin(angle) * radiusRandom,0);
        return new Vector3(point.x,point.y,-5);
    }

    public void DestroyChild() {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }
}
