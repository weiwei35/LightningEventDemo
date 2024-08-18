using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySkipBtn : MonoBehaviour
{
    public TW_MultiStrings_Regular story;
    private void OnEnable() {
        PlayerPrefs.SetInt("Story",1);
        GameObject[] gameObjects = getDontDestroyOnLoadGameObjects();
        foreach (var item in gameObjects)
        {
            if(item.layer == 19)
                Destroy(item);
        }
    }
    private GameObject[] getDontDestroyOnLoadGameObjects()
    {
        var allGameObjects = new List<GameObject>();
        allGameObjects.AddRange(FindObjectsOfType<GameObject>());
        //移除所有场景包含的对象
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            for (var j = 0; j < objs.Length; j++)
            {
                allGameObjects.Remove(objs[j]);
            }
        }
        //移除父级不为null的对象
        int k = allGameObjects.Count;
        while (--k >= 0)
        {
            if (allGameObjects[k].transform.parent != null)
            {
                allGameObjects.RemoveAt(k);
            }
        }
        return allGameObjects.ToArray();
    }
    public void CheckSkip(){
        if(story.сharIndex > 0 && story.сharIndex!= story.ORIGINAL_TEXT.Length + 1){
            //skip
            story.SkipTypewriter();
        }else if(story.сharIndex == story.ORIGINAL_TEXT.Length + 1){
            if(story.index_of_string+1 >= story.MultiStrings.Length){
                SceneManager.LoadSceneAsync("LightningMainScene");
            }else{
                //next
                story.NextString();
            }
        }
    }
}
