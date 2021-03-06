﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedHeliMove : MonoBehaviour, IHeliMoveMode
{
    GameObject moveHere;
    public GameObject testMovePoint;
    public MovePoint arMovePoint;
    public GameObject arRotatePoint;
    public GameObject arCam;

    float time;
    Vector2 horizontalRefVel = Vector3.zero;
    float vertRefVel = 0f;
    

    public float movePointDistance = .35f;
    //time it takes to get to position horizontally -- should be lower than vertical speed
    public float horizontalSpeed = 70f;
    //time it takes to get to postion vertically -- should be higher than horizontal speed
    public float verticalSpeed = 80f;
    //amount of angle towards the point we wish to roll towards
    public float rollSpeed = .2f;
    //distance cutoff within which the roll amount will be a percentage of the standard rolltowardspoint angle (damps rolling when close)
    public float rollCutoff = 2; 
    //rotation speed around y axis
    public float rotationSpeed = .05f;

    public bool useTestMovePoint;

    public bool useHeliCompare = false; 
    public float forwardTotalPercent = .9f;
    public bool useRotatePoint = false;
    

    // Start is called before the first frame update
    void Start()
    {
       if (useTestMovePoint){
           moveHere = testMovePoint;
       } else if (!useTestMovePoint){
           moveHere = arMovePoint.gameObject;
       }
       arMovePoint.distanceToCamera = movePointDistance;
    }

    public void StartHeliMoveMode(){

    }

    public void ExecuteOnHeliMoveModeUpdate(){
        HorizontalMove();
        VerticalMove();
        MoveRotation();
        RollRotation();
    }

    public void EndHeliMoveMode(){

    }

    // Update is called once per frame
    // void Update()
    // {
    //     time += Time.deltaTime;
    //     if (time > 2f){
    //         HorizontalMove();
    //         VerticalMove();
    //         MoveRotation();
    //         RollRotation();
    //     }
        
    // }

    void HorizontalMove (){
        //smoothdamp function for purely horizontal movement (lower speed value than vertical speed)
        Vector2 thisHorizPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetHorizPos = new Vector2(moveHere.transform.position.x, moveHere.transform.position.z);
        //HERE
        var forwardBoost = GetForwardPercentage();
        var newHorizPos = Vector2.SmoothDamp(thisHorizPos, targetHorizPos, ref horizontalRefVel, (horizontalSpeed - forwardBoost) * Time.deltaTime);
        Vector3 newVector3Pos = new Vector3(newHorizPos.x, transform.position.y, newHorizPos.y);
        transform.position = newVector3Pos;
    }
    void VerticalMove(){
        //moves purely on the y axis -- has seperate SmoothDamp function (speed should be higher than horizontal speed)
        var thisVertPos = transform.position.y;
        var targetVertPos = moveHere.transform.position.y;
        var newVertPos = Mathf.SmoothDamp(thisVertPos, targetVertPos, ref vertRefVel, verticalSpeed * Time.deltaTime);
        Vector3 newVector3Pos = new Vector3(transform.position.x, newVertPos, transform.position.z);
        transform.position = newVector3Pos;
    }

    void MoveRotation(){
        //creates purely y rotation eulerangle vector3 sets for the rotation of the helicopter and rotation of the camera
        //then slerps using quaternion versions of these angle sets 
        Quaternion thisRot = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        Quaternion camRot = Quaternion.Euler(0f, arCam.transform.eulerAngles.y, 0f);
        var pointDir = arRotatePoint.transform.position - this.transform.position;
        Quaternion pointRot = Quaternion.LookRotation(Vector3.up, -pointDir) * Quaternion.AngleAxis(90f, Vector3.right);
        Quaternion theRot = useRotatePoint ? pointRot : camRot;
        transform.rotation = Quaternion.Slerp(thisRot, theRot, rotationSpeed);
        
    }

    void RollRotation(){
        //giving new variable so we can mess with it variably every frame based on how far target Pose is 
        float rollSlerpValue = rollSpeed;
        //how close the target position has to be for the roll amount to start scaling

        //we use vector3 positions for calculating distance with no y value because otherwise this distance scaling method of calculating roll causes 
        //problems with vertical movement
        var flatTargetPos = new Vector3(moveHere.transform.position.x, 0f, moveHere.transform.position.z);
        var flatThisPos = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        var distanceToTargetPos = Vector3.Distance(flatTargetPos, flatThisPos);

        //calculating whether or not roll needs to be scaled/scaling if necessary
        if (distanceToTargetPos < rollCutoff){
            rollSlerpValue = rollSlerpValue * (distanceToTargetPos/rollCutoff);
        }

        var direction = (moveHere.transform.position - this.transform.position).normalized;
        var newDirection = Quaternion.Slerp(this.transform.rotation, Quaternion.FromToRotation(Vector3.up, direction), rollSlerpValue);
        transform.rotation = Quaternion.Euler(newDirection.eulerAngles.x, transform.eulerAngles.y, newDirection.eulerAngles.z);
    }

    float GetForwardPercentage(){
        Quaternion camDirection = Quaternion.Euler(new Vector3(0f, arCam.transform.eulerAngles.y, 0f));

        Vector3 offset = transform.TransformVector(new Vector3(0f, 0f, 1f));
        var movePointVector3 = (arMovePoint.gameObject.transform.position + offset) - this.transform.position; 

        

       

        
        Quaternion movePointDirection = Quaternion.LookRotation(Vector3.up, -movePointVector3.normalized) * Quaternion.AngleAxis(90f, Vector3.right);
        Quaternion heliDirection = Quaternion.Euler(new Vector3(0f, this.transform.eulerAngles.y, 0f));
        // Debug.Log("heli dir" + heliDirection);
        

        Quaternion theDir = movePointDirection;
        
        float percentage;
        if(useHeliCompare){
            percentage = Mathf.Pow((1- (Quaternion.Angle(theDir, heliDirection)/180f)), 2f);
        } else {
            percentage = Mathf.Pow((1f - (Quaternion.Angle(camDirection, theDir)/180f)), 2f);
        }

        
        // Debug.Log("percentage" + percentage);
        var increaseAmt = (percentage * (horizontalSpeed  * forwardTotalPercent));
        // Debug.Log("possible increase amt" + (horizontalSpeed * forwardTotalPercent));
        
        return increaseAmt;
    }
}
