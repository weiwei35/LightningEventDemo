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
        StartCoroutine(SetScene());
    }
    IEnumerator SetScene(){
        fadeAnim.SetTrigger("fade");
        lightEvent.SetActive(false);
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
}
