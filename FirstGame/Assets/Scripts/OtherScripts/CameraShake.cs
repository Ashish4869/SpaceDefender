using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Responsible for camera shakes and move ments
/// </summary>
public class CameraShake : MonoBehaviour
{
    [SerializeField]
    Transform TalkingPoeple;
    float CamMovespeed = 5f,CamZoomSpeed = 2.5f, ZoomedOrthograhpicSize = 2f ;
    

  //calls the courintes neccessary for starting the camera effects
    public void MoveCam()
    {
        StartCoroutine(MoveToFocus());
        StartCoroutine(ZoomIn());
    }

   //shake the camera as per the parameters passed
    public IEnumerator Shake(float duration , float magnitude)
    {
        Vector2 OriginalCameraPos = transform.localPosition;
        float elaspedTime = 0.0f;

        while(elaspedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector2(x, y);

            elaspedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = OriginalCameraPos;
    }

    //moves the camera to the main characters
    public IEnumerator MoveToFocus()
    {
        
        while (transform.position != TalkingPoeple.position)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(TalkingPoeple.position.x, TalkingPoeple.position.y, -1f) ,
                CamMovespeed * Time.deltaTime);
            yield return null;
        }

    }

    //reduces the orthographics size to make the character fit in screen
    IEnumerator ZoomIn()
    {
        while(Camera.main.orthographicSize != ZoomedOrthograhpicSize)
        {
            Camera.main.orthographicSize = Mathf.MoveTowards(
            Camera.main.orthographicSize, ZoomedOrthograhpicSize, CamZoomSpeed * Time.deltaTime);
            yield return null;
        } 
    }  


    
   
}
