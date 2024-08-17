using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PaperModel : MonoBehaviour
{
    public Animator anim;
    public GameObject effect_over;
    [HideInInspector]
    public bool isOverLoad = false;

    public virtual void OverLoadFun(){
        if(effect_over != null){
            ParticleSystemScaler particle = effect_over.GetComponent<ParticleSystemScaler>();
            particle.particlesScale = 1;
            effect_over.SetActive(true);
        }
    }
    public virtual void EndOverLoadFun(){
        if(effect_over != null){
            ParticleSystemScaler particle = effect_over.GetComponent<ParticleSystemScaler>();
            DOTween.To(()=>particle.particlesScale, x =>particle.particlesScale = x,0,1);
            effect_over.SetActive(false);
        }
    }
}
