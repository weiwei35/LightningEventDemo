using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Sword : MonoBehaviour
{
    PlayerController player;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.moveX > 0){
            transform.localPosition = new Vector3(-1.46f,0.8f,0f);
        }else if(player.moveX < 0){
            transform.localPosition = new Vector3(1.46f,0.8f,0f);
        }

        if(Global.swordFollowEnemy.Count > 0){
            Fire();
        }
    }
    public GameObject sword;

    public void Fire(){
        animator.SetTrigger("attack");
        for (int i = 0; i < Global.swordFollowEnemy.Count; i++)
        {
            StartCoroutine("SetAnim",Global.swordFollowEnemy[i]);
            Global.swordFollowEnemy.Remove(Global.swordFollowEnemy[i]);
        }
    }

    IEnumerator SetAnim(GameObject pos){
        yield return new WaitForSeconds(0.2f);
        var fireWay = Instantiate(sword);
        fireWay.transform.position = transform.position;
        SwordController swordController = fireWay.GetComponent<SwordController>();
        swordController.SetCircleLine(transform.position, pos);
    }
}
