using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Grass : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Material material; 
    public UnityEngine.XR.ARSubsystems.TrackableId planeID;
    PlaneObjectStateMachine statemachine = new PlaneObjectStateMachine();
    float densityCheckAmt = .1f;

    void Start(){
        meshRenderer = GetComponent<MeshRenderer>();
        Initialize();
    }

    void Update(){
        //moved to active state later 
        ARPlane objectPlane; 
        if (PlaneObjectData.singleton.planeManager.trackables.TryGetTrackable(planeID, out objectPlane)){
            var newHeight = new Vector3(gameObject.transform.position.x, objectPlane.transform.position.y, gameObject.transform.position.z);
            gameObject.transform.position = newHeight;
        } else {
            Destroy(gameObject);
        }

        transform.LookAt(PlaneObjectData.singleton.arCam.transform.position);
        Quaternion lookingAtCam = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        transform.rotation = lookingAtCam;
    }

    void Initialize()
    {
        //local density check
        if (Physics.OverlapSphere(this.transform.position, densityCheckAmt).Length > 2){
            Destroy(this.gameObject);
        }
        //setting color
        meshRenderer.material.color  = PlaneObjectData.singleton.grassColors[Random.Range(0, PlaneObjectData.singleton.grassColors.Length)];
        //setting scale
        var randomScale = Random.Range(PlaneObjectData.singleton.grassScaleRange.x, PlaneObjectData.singleton.grassScaleRange.y);
        gameObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

    }
}
