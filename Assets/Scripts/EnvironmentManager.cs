using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class EnvironmentManager : MonoBehaviour
{
    public ARRaycastManager aRRaycastManager;
    // Start is called before the first frame update
    public PlaneEnvironmentObject testPrefab;
    public List<PlaneEnvironmentObject> planeEnvironmentObjects = new List<PlaneEnvironmentObject>();
    public ARPlaneManager planeManager;
    
   
    void Update()
    {
        TryCreatePlaneEnvironment();
        UpdatePlaneObjects();
    }

    bool TryGetInitialRaycast (out Vector3 initialPosition) {
        //trys a raycast from screen center forward to where user is looking
        //if it hits a trackable plane that position is set as the initialposition around which to test for environment placement positions 
        Vector2 screenCenter = new Vector2(Screen.width/2f, Screen.height/2f);
        var hits = new List<ARRaycastHit>();
        if (aRRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            initialPosition = hits[0].pose.position;
            return true; 
        } else 
        {
            initialPosition = default;
            return false;
        }
    }

    void TryCreatePlaneEnvironment (){
        Debug.Log("trying to create environment");
        Vector3 initialPosition;
        bool DidInitialRaycastHit = TryGetInitialRaycast(out initialPosition);
        if (!DidInitialRaycastHit){
            return;
        } else 
        {
            //max tries is 20 per frame
            for (int i = 0; i < 19; i++)
            {
                //first population check goes here
                var regionPopulation = Physics.OverlapSphere(initialPosition, 20f);
                if (regionPopulation.Length < 20)
                {
                    var randomRelativePos = Random.insideUnitCircle * 20;
                    var testPosition = new Vector3 (initialPosition.x + randomRelativePos.x, initialPosition.y, initialPosition.z + randomRelativePos.y);
                    var hits = new List<ARRaycastHit>();
                    Ray testRay = new Ray(this.transform.position, testPosition - this.transform.position);
                    
                    if (aRRaycastManager.Raycast(testRay, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)){
                        //second population check using testposition goes here
                        //spawning perlin and typeofobject functions go here
                        var instanceOfPlaneObject = Instantiate(testPrefab, hits[0].pose.position, Quaternion.identity);
                        planeEnvironmentObjects.Add(instanceOfPlaneObject); //adding to list of PlaneEnvironmentObjects
                        instanceOfPlaneObject.planeID = hits[0].trackableId; //assigning ID of plane to variable on instance of PlaneEnvironmentObject
                        Debug.Log(instanceOfPlaneObject.planeID.ToString());
                        
                    }
                }
            }
        }

        
    }

    void UpdatePlaneObjects(){
        //fixes object heights as planes move (LOL)
        //this currently only fixes height every time it is called -- should eventually manage object removal/plane removal as well
        foreach (PlaneEnvironmentObject item in planeEnvironmentObjects){
            ARPlane plane;
            if (planeManager.trackables.TryGetTrackable(item.planeID, out plane)){
                var newHeight = new Vector3(item.gameObject.transform.position.x, plane.transform.position.y, item.gameObject.transform.position.z);
                item.gameObject.transform.position = newHeight;
            } else {
                Destroy(item.gameObject);
                planeEnvironmentObjects.Remove(item);
            }
            
        }
    }
}
