using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliMove : MonoBehaviour
{
    GameObject moveHere;
    public GameObject testMovePoint;
    public GameObject arMovePoint;

    float time;
    Vector2 horizontalRefVel = Vector3.zero;
    float vertRefVel = 0f;

    public bool useTestMovePoint;

    // Start is called before the first frame update
    void Start()
    {
       if (useTestMovePoint){
           moveHere = testMovePoint;
       } else if (!useTestMovePoint){
           moveHere = arMovePoint;
       }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 3f){
            HorizontalMove();
            VerticalMove();
        }
        
    }

    void HorizontalMove (){
        Vector2 thisHorizPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetHorizPos = new Vector2(moveHere.transform.position.x, moveHere.transform.position.z);
        var newHorizPos = Vector2.SmoothDamp(thisHorizPos, targetHorizPos, ref horizontalRefVel, .5f);
        Vector3 newVector3Pos = new Vector3(newHorizPos.x, transform.position.y, newHorizPos.y);
        transform.position = newVector3Pos;
    }
    void VerticalMove(){
        var thisVertPos = transform.position.y;
        var targetVertPos = moveHere.transform.position.y;
        var newVertPos = Mathf.SmoothDamp(thisVertPos, targetVertPos, ref vertRefVel, .7f);
        Vector3 newVector3Pos = new Vector3(transform.position.x, newVertPos, transform.position.z);
        transform.position = newVector3Pos;
    }
}
