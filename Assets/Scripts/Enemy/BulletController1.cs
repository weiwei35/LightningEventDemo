using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController1 : MonoBehaviour
{
    public void DestroyAllBullets(){
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }
}
