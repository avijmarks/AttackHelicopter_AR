using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public float distanceToCamera;

    public GameObject arCam;

    // Update is called once per frame
    void Update()
    {
        Vector3 newLocation = arCam.transform.TransformPoint(new Vector3(0f, 0f, distanceToCamera));
        gameObject.transform.position = newLocation;
    }
}
