using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Rocks : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Material material; 
    public UnityEngine.XR.ARSubsystems.TrackableId planeID;
    PlaneObjectStateMachine statemachine = new PlaneObjectStateMachine();
    float densityCheckAmt = .2f;
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
    }

    void Initialize(){
        if (Physics.OverlapSphere(this.transform.position, densityCheckAmt).Length > 2){
            Destroy(this.gameObject);
        }
        //setting color
        meshRenderer.material.color  = PlaneObjectData.singleton.rockColors[Random.Range(0, PlaneObjectData.singleton.rockColors.Length-1)];
        //setting scale
        var randomScale = Random.Range(PlaneObjectData.singleton.rockScaleRange.x, PlaneObjectData.singleton.rockScaleRange.y);
        gameObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}
