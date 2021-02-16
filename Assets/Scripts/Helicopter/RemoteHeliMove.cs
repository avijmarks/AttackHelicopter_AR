using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RemoteHeliMove : MonoBehaviour, IHeliMoveMode
{

    public FixedJoystick landscapeJoystick;
    public FixedJoystick portraitJoystick;
    FixedJoystick currentJoystick;

    GameObject moveHere;
    public MovePoint arMovePoint;
    public GameObject remoteMovePoint;
    //an empty slightly in front of movepoint -- used for keeping rotation correct
    public GameObject arRotatePoint;
    public GameObject arCam;

    float time;
    Vector2 horizontalRefVel = Vector3.zero;
    float vertRefVel = 0f;

    //max distance the remoteMovePoint can be from the camera
    public float maxDistance = 100;
    public float verticalPointDistance = .35f;
    public float remotePointMoveSpeed = .1f;
    //time it takes to get to position horizontally -- should be lower than vertical speed
    public float horizontalSpeed = 70f;
    //time it takes to get to postion vertically -- should be higher than horizontal speed
    public float verticalSpeed = 80f;
    //amount of angle towards the point we wish to roll towards
    public float rollSpeed = .2f;
    //distance cutoff within which the roll amount will be a percentage of the standard rolltowardspoint angle (damps rolling when close)
    public float rollCutoff = 2f; 
    //rotation speed around y axis
    public float rotationSpeed = .05f;

    public bool useTestMovePoint;

    public bool useHeliCompare = false; 
    public float forwardTotalPercent = .9f;
    public bool useRotatePoint = false;
    public GameObject boundaryGridPrefab;
    bool currentlyDisplayingBoundary = false;
    GameObject currentBoundaryBeingDisplayed;
    

    // Start is called before the first frame update
    void Start()
    {
        if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Landscape)
        {
            currentJoystick = landscapeJoystick;
        }
        else if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Portrait)
        {
            currentJoystick = portraitJoystick;
        }
        //only here because of old syntax from helimove.cs
        moveHere = remoteMovePoint;
        //currentJoystick.gameObject.SetActive(false);

        UIOrientationManager.instance.OnSwitchedToLandscape += SwitchToLandscapeLayout;
        UIOrientationManager.instance.OnSwitchedToPortrait += SwitchToPortraitLayout;
    }

    public void ResetRemotePoint(){
        //resets remotepoint position to same position as the attached movepoint in front of the camera
        remoteMovePoint.transform.position = arMovePoint.transform.position;
        remoteMovePoint.transform.rotation = arMovePoint.transform.rotation;
    }

    public void StartHeliMoveMode(){
        currentJoystick.gameObject.SetActive(true);
        arMovePoint.distanceToCamera = verticalPointDistance;
        ResetRemotePoint();
    }

    public void ExecuteOnHeliMoveModeUpdate(){
        UpdateRemotePointLocation();
        HorizontalMove();
        VerticalMove();
        MoveRotation();
        RollRotation();
    }

    public void EndHeliMoveMode(){
        currentJoystick.gameObject.SetActive(false);
    }

    public void SwitchToLandscapeLayout ()
    {
        currentJoystick = landscapeJoystick;

        if (PlayerSettings.instance.heliMoveManager.useRemoteMode)
        {
            portraitJoystick.gameObject.SetActive(false);
            landscapeJoystick.gameObject.SetActive(true);
        }
    }

    public void SwitchToPortraitLayout()
    {
        currentJoystick = portraitJoystick;

        if (PlayerSettings.instance.heliMoveManager.useRemoteMode)
        {
            landscapeJoystick.gameObject.SetActive(false);
            portraitJoystick.gameObject.SetActive(true);
        }
    }
   
    void UpdateRemotePointLocation (){
        //move remotepoint based on joystick (limited by max distance)
        //distance checks needed
        float relativeMoveSpeed = remotePointMoveSpeed * Time.deltaTime;
        Vector3 localMovePosition = new Vector3(currentJoystick.Horizontal * relativeMoveSpeed, 0f, currentJoystick.Vertical * relativeMoveSpeed);
        Vector3 moveInDirection = arCam.transform.TransformVector(localMovePosition).normalized;
        
        //checks if new movement would exceed max distance and returns vector3.zero if it will
        moveInDirection = CheckMaxPointDistance(moveInDirection);
        
        remoteMovePoint.transform.position += moveInDirection;
        
        remoteMovePoint.transform.position = new Vector3(remoteMovePoint.transform.position.x, arMovePoint.transform.position.y, remoteMovePoint.transform.position.z);
        if (moveInDirection != Vector3.zero){
            Quaternion remotePointRotation = Quaternion.LookRotation(moveInDirection, Vector3.up);
            remoteMovePoint.transform.rotation = remotePointRotation;
        }
    }

    Vector3 CheckMaxPointDistance(Vector3 moveInDirection){
       
        var currentDistanceToPoint = Vector3.Distance(arCam.transform.position, remoteMovePoint.transform.position);
        var newDistanceToPoint = Vector3.Distance(arCam.transform.position, (remoteMovePoint.transform.position + moveInDirection));
        if (newDistanceToPoint > maxDistance && newDistanceToPoint > currentDistanceToPoint){
            //maxdistance check
            moveInDirection = Vector3.zero;
            
            
            MaxDistanceExceededVisualizer(moveInDirection);
        }
        return moveInDirection;
    }

    

    void MaxDistanceExceededVisualizer(Vector3 beyondMaxPoint){
        if (currentlyDisplayingBoundary){
                return;
        }
        //creates a plane/grid visualizer at location helicopter is exceeding max distance
        beyondMaxPoint = remoteMovePoint.transform.TransformPoint(beyondMaxPoint);
        GameObject boundaryGridInstance = Instantiate(boundaryGridPrefab);
        currentlyDisplayingBoundary = true;
        currentBoundaryBeingDisplayed = boundaryGridInstance;
    
        boundaryGridInstance.transform.position = beyondMaxPoint;
        Vector3 gridLookPoint = beyondMaxPoint - arCam.transform.position;
        boundaryGridInstance.transform.rotation = Quaternion.LookRotation(gridLookPoint, Vector3.up);
        StartCoroutine("DisplayBoundary");

       
    }
     IEnumerator DisplayBoundary () {
            float time = 0f;
            MeshRenderer renderer = currentBoundaryBeingDisplayed.GetComponentsInChildren<MeshRenderer>()[0];
            
            while (time < 2.0f){
                time += Time.deltaTime;
                Color c = renderer.material.color;
                c.a -= (Time.deltaTime / 2f);
                renderer.material.color = c;
                yield return null;
            }
        Destroy(currentBoundaryBeingDisplayed);
        currentlyDisplayingBoundary = false;
    }

    void HorizontalMove (){
        //smoothdamp function for purely horizontal movement (lower speed value than vertical speed)
        Vector2 thisHorizPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetHorizPos = new Vector2(moveHere.transform.position.x, moveHere.transform.position.z);
        
        var newHorizPos = Vector2.SmoothDamp(thisHorizPos, targetHorizPos, ref horizontalRefVel, (horizontalSpeed * Time.deltaTime));
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
}
