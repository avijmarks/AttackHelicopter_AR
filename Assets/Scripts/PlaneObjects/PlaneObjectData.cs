using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneObjectData : MonoBehaviour
{
    public static PlaneObjectData singleton = null;

    public ARPlaneManager planeManager;
    public GameObject arCam;

    public Grass[] grassTypes;
    public Color[] grassColors;
    public Vector2 grassScaleRange = new Vector2(.7f, 1.3f);
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public Trees[] treeTypes;
    public Color[] treeColors;
    public Color[] treeColorSecondary; 
    public Vector2 treeScaleRange = new Vector2(.6f, 1.3f);
    
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public Rocks[] rockTypes;
    public Color[] rockColors;
    public Vector2 rockScaleRange = new Vector2(.07f, .13f);


    void Awake(){
        if (singleton == null){
            singleton = this;
        } else if (singleton != this){
            Destroy(gameObject);
        }
    }

    public Vector3 RandomYRot (){
        var newRot = new Vector3(0f, Random.Range(0f, 360f), 0f);
        return newRot;
    }
}
