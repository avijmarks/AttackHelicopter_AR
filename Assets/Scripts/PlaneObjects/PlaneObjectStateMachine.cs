using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneObjectStateMachine : MonoBehaviour
{
    private IPlaneObjectState currentlyRunningState;
    private IPlaneObjectState previousState;
    public void ChangeState(IPlaneObjectState newState)
    {
        if (this.currentlyRunningState != null)
        {
            this.currentlyRunningState.Exit(); 
        }
        this.previousState = this.currentlyRunningState;
        this.currentlyRunningState = newState;
        this.currentlyRunningState.Enter();
    }
    public void ExecuteStateUpdate()
    {
        var runningState = this.currentlyRunningState;
        if (runningState != null)
        {
            runningState.Execute();
        }
    }
    public void SwitchToPreviousState()
    {
        this.currentlyRunningState.Exit();
        this.currentlyRunningState = this.previousState;
        this.currentlyRunningState.Enter();

    }
}
