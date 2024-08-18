using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBarUI : MonoBehaviour
{
    public GameController game;
    public Slider slider;
    GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(game.isBossLevel){
            enemy = GameObject.FindGameObjectWithTag("Boss");
            if(enemy == null) return;
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if(enemyController.canHurt){
                slider.value = enemyController.ShowHPBar();
                transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position) + new Vector3(0,70,0);
            }
        }
    }
}
