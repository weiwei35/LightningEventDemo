using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePanelController : MonoBehaviour
{
    public PlayerController player;
    public GameObject center;
    public GameObject start1, start2, start3;
    public float radius1, radius2, radius3;
    public SpriteRenderer sprite1;
    public Material light_m;
    public Material dark_m;
    //根据角色距离圆心的距离判断所在环数，根据角色与圆心直线夹角判断所在区域
    public void GetPartByPlayer(){
        float distance = Vector3.Distance(player.transform.position,center.transform.position);
        if(distance < radius1 && distance > 8){
            Vector3 PtoC = player.transform.position - center.transform.position;
            Vector3 StoC = start1.transform.position - center.transform.position;
            float angle = Vector3.Angle(PtoC, StoC);
            float atan2 = Mathf.Atan2(PtoC.y, PtoC.x) - Mathf.Atan2(StoC.y, StoC.x);
            bool isClockwise = atan2 > 0; // 如果atan2大于0，则是顺时针，否则是逆时针

            if(isClockwise){
                if(angle < 45 && angle > 0){
                    sprite1.material = light_m;
                }else{
                    sprite1.material = dark_m;
                }
            }else{
                sprite1.material = dark_m;
            }
        }else{
            sprite1.material = dark_m;
        }
    }

    private void Update() {
        GetPartByPlayer();
    }
}
