using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Trees : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Material material; 
    public UnityEngine.XR.ARSubsystems.TrackableId planeID;
    PlaneObjectStateMachine statemachine = new PlaneObjectStateMachine();
    float densityCheckAmt = .2f;
    public bool isTwoTonedTree = false;
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
        if (isTwoTonedTree){
            int treeColor = Random.Range(0, PlaneObjectData.singleton.treeColors.Length-1);
            meshRenderer.materials[1].color  = PlaneObjectData.singleton.treeColors[treeColor];
            meshRenderer.materials[2].color  = PlaneObjectData.singleton.treeColorSecondary[treeColor];
        } else {
            meshRenderer.materials[1].color  = PlaneObjectData.singleton.treeColors[Random.Range(0, PlaneObjectData.singleton.treeColors.Length-1)];
        }
        
        //setting scale
        var randomScale = Random.Range(PlaneObjectData.singleton.treeScaleRange.x, PlaneObjectData.singleton.treeScaleRange.y);
        gameObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}
