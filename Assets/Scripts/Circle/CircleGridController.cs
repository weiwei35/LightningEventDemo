using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CircleGridController : MonoBehaviour
{
    public int id;
    SpriteRenderer grid;
    public GameObject lightBG;
    public Material light_m;
    public Material dark_m;
    bool inDoor = false;
    CirclePanelController circlePanel;
    MeshCollider meshCollider;

    private void Start() {
        grid = transform.parent.GetComponent<SpriteRenderer>();
        circlePanel = GameObject.FindWithTag("CirclePanel").GetComponent<CirclePanelController>();
        meshCollider = GetComponent<MeshCollider>();
    }

    private void OnEnable() {
        if(lightBG != null && lightBG.activeInHierarchy == true) {
            lightBG.SetActive(false);
        }
        grid = transform.parent.GetComponent<SpriteRenderer>();
        grid.material = dark_m;
        inDoor = false;
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.enabled = true;
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == 9){
            grid = transform.parent.GetComponent<SpriteRenderer>();
            lightBG.SetActive(true);
            grid.material = light_m;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 9){
            grid = transform.parent.GetComponent<SpriteRenderer>();
            lightBG.SetActive(true);
            grid.material = light_m;
        }
    }
    private void OnCollisionExit(Collision other) {
        if(other.gameObject.layer == 9){
            grid = transform.parent.GetComponent<SpriteRenderer>();
            lightBG.SetActive(false);
            grid.material = dark_m;
            inDoor = false;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == 9){
            grid = transform.parent.GetComponent<SpriteRenderer>();
            lightBG.SetActive(false);
            grid.material = dark_m;
            inDoor = false;
        }
    }
    private void OnCollisionStay(Collision other) {
        if(other.gameObject.layer == 9){
            grid = transform.parent.GetComponent<SpriteRenderer>();
            lightBG.SetActive(true);
            grid.material = light_m;
            inDoor = true;
        }
    }
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == 9){
            grid = transform.parent.GetComponent<SpriteRenderer>();
            lightBG.SetActive(true);
            grid.material = light_m;
            inDoor = true;
        }
    }
    bool startFade = false;

    public void SetFadeIn(){
        startFade = true;
        grid = transform.parent.GetComponent<SpriteRenderer>();
        grid.DOFade(1,1f).OnComplete(()=>{
            circlePanel.isFadeIn = false;
            startFade = false;
        });
    }
    public void SetFadeOut(){
        startFade = true;
        meshCollider.enabled = true;
        grid = transform.parent.GetComponent<SpriteRenderer>();
        grid.DOFade(0,1f).OnComplete(()=>{
            circlePanel.isFadeOut = false;
            startFade = false;
        });
    }
    private void Update() {
        if(circlePanel.isFadeIn && !startFade){
            SetFadeIn();
        }
        if(circlePanel.isFadeOut && !startFade){
            SetFadeOut();
        }
        if(inDoor){
            switch (id)
            {
                case 1:
                    circlePanel.inDoor_DU = true;
                    circlePanel.inDoor_JING = false;
                    circlePanel.inDoor_SI = false;
                    circlePanel.inDoor_JING_Bad = false;
                    circlePanel.inDoor_KAI = false;
                    circlePanel.inDoor_XIU = false;
                    circlePanel.inDoor_SHENG = false;
                    circlePanel.inDoor_SHANG = false;
                    circlePanel.checkingDoor = false;
                    return;
                case 2:
                    circlePanel.inDoor_DU = false;
                    circlePanel.inDoor_JING = true;
                    circlePanel.inDoor_SI = false;
                    circlePanel.inDoor_JING_Bad = false;
                    circlePanel.inDoor_KAI = false;
                    circlePanel.inDoor_XIU = false;
                    circlePanel.inDoor_SHENG = false;
                    circlePanel.inDoor_SHANG = false;
                    circlePanel.checkingDoor = false;
                    return;
                case 3:
                    circlePanel.inDoor_DU = false;
                    circlePanel.inDoor_JING = false;
                    circlePanel.inDoor_SI = true;
                    circlePanel.inDoor_JING_Bad = false;
                    circlePanel.inDoor_KAI = false;
                    circlePanel.inDoor_XIU = false;
                    circlePanel.inDoor_SHENG = false;
                    circlePanel.inDoor_SHANG = false;
                    circlePanel.checkingDoor = false;
                    return;
                case 4:
                    circlePanel.inDoor_DU = false;
                    circlePanel.inDoor_JING = false;
                    circlePanel.inDoor_SI = false;
                    circlePanel.inDoor_JING_Bad = true;
                    circlePanel.inDoor_KAI = false;
                    circlePanel.inDoor_XIU = false;
                    circlePanel.inDoor_SHENG = false;
                    circlePanel.inDoor_SHANG = false;
                    circlePanel.checkingDoor = false;
                    return;
                case 5:
                    circlePanel.inDoor_DU = false;
                    circlePanel.inDoor_JING = false;
                    circlePanel.inDoor_SI = false;
                    circlePanel.inDoor_JING_Bad = false;
                    circlePanel.inDoor_KAI = true;
                    circlePanel.inDoor_XIU = false;
                    circlePanel.inDoor_SHENG = false;
                    circlePanel.inDoor_SHANG = false;
                    circlePanel.checkingDoor = false;
                    return;
                case 6:
                    circlePanel.inDoor_DU = false;
                    circlePanel.inDoor_JING = false;
                    circlePanel.inDoor_SI = false;
                    circlePanel.inDoor_JING_Bad = false;
                    circlePanel.inDoor_KAI = false;
                    circlePanel.inDoor_XIU = true;
                    circlePanel.inDoor_SHENG = false;
                    circlePanel.inDoor_SHANG = false;
                    circlePanel.checkingDoor = false;
                    return;
                case 7:
                    circlePanel.inDoor_DU = false;
                    circlePanel.inDoor_JING = false;
                    circlePanel.inDoor_SI = false;
                    circlePanel.inDoor_JING_Bad = false;
                    circlePanel.inDoor_KAI = false;
                    circlePanel.inDoor_XIU = false;
                    circlePanel.inDoor_SHENG = true;
                    circlePanel.inDoor_SHANG = false;
                    circlePanel.checkingDoor = false;
                    return;
                case 8:
                    circlePanel.inDoor_DU = false;
                    circlePanel.inDoor_JING = false;
                    circlePanel.inDoor_SI = false;
                    circlePanel.inDoor_JING_Bad = false;
                    circlePanel.inDoor_KAI = false;
                    circlePanel.inDoor_XIU = false;
                    circlePanel.inDoor_SHENG = false;
                    circlePanel.inDoor_SHANG = true;
                    circlePanel.checkingDoor = false;
                    return;
                default:
                return;
            }
        }
    }
}
