using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneController : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject mainCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame () {
        CanvasGroup startGroup = startPanel.GetComponent<CanvasGroup>();
        CanvasGroup canvasGroup = mainCanvas.GetComponent<CanvasGroup>();
        DOTween.To(()=>startGroup.alpha, x =>startGroup.alpha = x,0,1).OnComplete(()=>
        {
            DOTween.To(()=>canvasGroup.alpha, x =>canvasGroup.alpha = x,0,1).OnComplete(()=>
            {
                SceneManager.LoadSceneAsync("LightningMainScene");
            });
        });
        
    }
}
