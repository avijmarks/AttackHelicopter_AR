using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrientationManager : MonoBehaviour
{
    public static UIOrientationManager instance;

    public delegate void SwitchedToLandscapeHandler();
    public SwitchedToLandscapeHandler OnSwitchedToLandscape;

    public delegate void SwitchedToPortraitHandler();
    public SwitchedToPortraitHandler OnSwitchedToPortrait;

    public enum Orientation { Landscape, Portrait };
    public Orientation currentOrientation = Orientation.Portrait;
    private Orientation previousOrientation = Orientation.Portrait;


    private void Awake()
    {
        //singleton code :)
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (Input.deviceOrientation == DeviceOrientation.Unknown) Debug.LogError("Device orientation unknown: defaulting to portrait");

        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            currentOrientation = Orientation.Landscape;
        }
        else
        {
            currentOrientation = Orientation.Portrait;
        }
    }

    public void SwitchedToLandscape()
    {
        instance.OnSwitchedToLandscape();
        Debug.LogError("Landscape Switch Callback");
    }

    void SwitchedToPortrait()
    {
        instance.OnSwitchedToPortrait();
        Debug.LogError("Landscape Switch Callback");
    }

    private void FixedUpdate()
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            currentOrientation = Orientation.Portrait;
            

            if (currentOrientation != previousOrientation)
            {
                SwitchedToPortrait();
            }

        }
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {

            currentOrientation = Orientation.Landscape;

            if (currentOrientation != previousOrientation)
            {
                SwitchedToLandscape();
            }
        }
        else
        {
            //Debug.LogWarning("Device Orientation information unavailable, defaulting to portrait mode");
            currentOrientation = Orientation.Portrait; 
        }

        previousOrientation = currentOrientation;
        Debug.LogError(currentOrientation.ToString());
    }
}
