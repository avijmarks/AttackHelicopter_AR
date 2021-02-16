using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliMoveModeManager : MonoBehaviour
{
    public IHeliMoveMode attachedHeliMove;
    public IHeliMoveMode remoteHeliMove;
    IHeliMoveMode currentMode;
    IHeliMoveMode disabledMode;
    //bool used to control which mode its in; true = remote mode (remote/true is default & startup)
    public bool useRemoteMode = false;

    void Awake(){
        attachedHeliMove = GetComponent<AttachedHeliMove>();
        remoteHeliMove = GetComponent<RemoteHeliMove>();
    }

    void Start(){
        
        //defaults to attached, just like bool in playersettings.cs
        currentMode = attachedHeliMove;
    }

    

    public void ChangeHeliMoveMode(IHeliMoveMode newMode){
        currentMode.EndHeliMoveMode();
        currentMode = newMode;
        currentMode.StartHeliMoveMode();
    }

    void Update(){
        currentMode.ExecuteOnHeliMoveModeUpdate();
    }


}
