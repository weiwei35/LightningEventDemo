using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance{get;set;}
    public Animator fadeAnim;
    public GameObject canvas;
    public GameObject hideUI;
    public GameObject lightEvent;
    public AudioSource audioBgm;
    public GameObject heroPanel;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(canvas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void StartGame () {
        StartCoroutine(ShowHeroPanel());
        // StartCoroutine(SetScene());
    }
    IEnumerator ShowHeroPanel(){
        fadeAnim.SetTrigger("fade");
        lightEvent.SetActive(false);
        yield return new WaitForSeconds(1);
        hideUI.SetActive(false);
        heroPanel.SetActive(true);
        fadeAnim.SetTrigger("fade");
    }
    public void StartMainGame () {
        StartCoroutine(SetScene());
    }
    IEnumerator SetScene(){
        fadeAnim.SetTrigger("fade");
        lightEvent.SetActive(false);
        DOTween.To(()=>audioBgm.volume, x =>audioBgm.volume = x,0,1);
        yield return new WaitForSeconds(1);
        hideUI.SetActive(false);
        AsyncOperation async = SceneManager.LoadSceneAsync("LightningMainScene");
        async.completed += UnloadScene;
    }

    private void UnloadScene(AsyncOperation operation)
    {
        fadeAnim.SetTrigger("fade");
    }

    public void SetUI() {
        fadeAnim.SetTrigger("fade");
        StartCoroutine(SetUIScene());
    }

    IEnumerator SetUIScene(){
        hideUI.SetActive(true);
        SceneManager.UnloadSceneAsync("LightningMainScene");
        yield return new WaitForSeconds(1);
        fadeAnim.SetTrigger("fade");
    }

    public void CountinueGame() {
        Global.continueGame = true;
        StartMainGame();
    }

    public void ExitGame() {
        Time.timeScale = 0;
        Application.Quit();
    }
}
