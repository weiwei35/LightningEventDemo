using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    public GameObject center;

    private float oldPosition;
    bool isOut = false;

    void Start()
    {
        oldPosition = transform.position.x;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position,center.transform.position) >= 6){
            if(!isOut){
                oldPosition = transform.position.x;
                isOut = true;
            }
            if (transform.position.x != oldPosition)
            {
                if (onCameraTranslate != null)
                {
                    float delta = oldPosition - transform.position.x;
                    onCameraTranslate(delta);
                }

                oldPosition = transform.position.x;
            }
        }
        if(Vector3.Distance(transform.position,center.transform.position) < 8){
            isOut = false;
        }
    }
}