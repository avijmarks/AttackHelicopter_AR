using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliMove : MonoBehaviour
{
    GameObject moveHere;
    public GameObject testMovePoint;
    public GameObject arMovePoint;
    public GameObject arCam;

    float time;
    Vector2 horizontalRefVel = Vector3.zero;
    float vertRefVel = 0f;

    public float rollSpeed = .2f;

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
            MoveRotation();
            RollRotation();
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

    void MoveRotation(){
        Quaternion thisRot = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        Quaternion camRot = Quaternion.Euler(0f, arCam.transform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(thisRot, camRot, .05f);
        
    }

    void RollRotation(){
        //giving new variable so we can mess with it variably every frame based on how far target Pose is 
        float rollSlerpValue = rollSpeed;

        //how close the target position has to be for the roll amount to start scaling
        float rollCutoff = 2f;

        //we use vector3 positions for calculating distance with no y value because otherwise this distance scaling method of calculating roll causes 
        //problems with vertical movement
        var flatTargetPos = new Vector3(moveHere.transform.position.x, 0f, moveHere.transform.position.z);
        var flatThisPos = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        var distance = Vector3.Distance(flatTargetPos, flatThisPos);
        //calculating whether or not roll needs to be scaled/scaling if necessary
        if (distance < rollCutoff){
            rollSlerpValue = rollSlerpValue * (distance/rollCutoff);
        }

        var direction = (moveHere.transform.position - this.transform.position).normalized;
        var newDirection = Quaternion.Slerp(this.transform.rotation, Quaternion.FromToRotation(Vector3.up, direction), rollSlerpValue);
        transform.rotation = Quaternion.Euler(newDirection.eulerAngles.x, transform.eulerAngles.y, newDirection.eulerAngles.z);
    }
}
