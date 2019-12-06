using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevSettingsMenu : MonoBehaviour
{
    
    public GameObject devSettingsPanel;
    public GameObject settingsPanel;
    public Text horizVText;
    public Text vertVText;
    public Text rollVtext;
    public Text cutoffVText;
    public Text rotationVText;
    public Text distanceVText;
    public Text forwardPercentText;
    public Text useRotatePointVariableText;
    public Text useHeliCompareVariableText;
    public Text remoteSpeedVText;
    public Text paidVersionVText;
    public MovePoint arMovePoint;
    public AttachedHeliMove attachedMoveController;
    public RemoteHeliMove remoteMoveController;
    public HeliMoveModeManager heliMoveModeManager;

    //delegate to make it easier/cleaner for me to see which mode's settings we're affecting
    delegate void Mode();


    // Start is called before the first frame update
    void Awake()
    {
        devSettingsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    //OPENING AND CLOSING SETTINGS PANEL FUNCTIONS -- used by button triggers
    public void OpenDevSettings (){
        AudioManager.instance.ClickSound();
        settingsPanel.SetActive(false);
        devSettingsPanel.SetActive(true);
        InitVariableText();
    }

    public void CloseDevSettings (){
        AudioManager.instance.ClickSound();
        devSettingsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public void HorizontalChanged (float newValue){
        //50-150
        Mode currentMode = heliMoveModeManager.useRemoteMode ? currentMode = Remote : currentMode = Attached;
        currentMode();
        void Remote(){
            remoteMoveController.horizontalSpeed = newValue;
        }
        void Attached (){
            attachedMoveController.horizontalSpeed = newValue;
        }

        horizVText.text = newValue.ToString();
    }
    public void VerticalChanged(float newValue){
        //50-150
        Mode currentMode = heliMoveModeManager.useRemoteMode ? currentMode = Remote : currentMode = Attached;
        currentMode();
        void Remote(){
            remoteMoveController.verticalSpeed = newValue;
        }
        void Attached (){
            attachedMoveController.verticalSpeed = newValue;
        }
        vertVText.text = newValue.ToString();
    }
    public void RollChanged(float newValue){
        //0-1
        Mode currentMode = heliMoveModeManager.useRemoteMode ? currentMode = Remote : currentMode = Attached;
        currentMode();
        void Remote(){
            remoteMoveController.rollSpeed = newValue;
        }
        void Attached (){
            attachedMoveController.rollSpeed = newValue;
        }
        rollVtext.text = newValue.ToString();
    }
    public void CutoffChanged(float newValue){
        //0-5
        Mode currentMode = heliMoveModeManager.useRemoteMode ? currentMode = Remote : currentMode = Attached;
        currentMode();
        void Remote(){
            remoteMoveController.rollCutoff = newValue;
        }
        void Attached (){
            attachedMoveController.rollCutoff = newValue;
        }
        cutoffVText.text = newValue.ToString();
    }
    public void RotationChanged(float newValue){
        //0-0.25
        Mode currentMode = heliMoveModeManager.useRemoteMode ? currentMode = Remote : currentMode = Attached;
        currentMode();
        void Remote(){
            remoteMoveController.rotationSpeed = newValue;
        }
        void Attached (){
            attachedMoveController.rotationSpeed = newValue;
        }
        rotationVText.text = newValue.ToString();
    }
    public void DistanceChanged(float newValue){
        //theoretically works with either as the diff modes set the distance in their startmode() functions
        arMovePoint.distanceToCamera = newValue;
        distanceVText.text = newValue.ToString();
    }

    public void ForwardPercentChanged (float newValue){
        //purely for attached mode -- does not exist in remote
        attachedMoveController.forwardTotalPercent = newValue;
        forwardPercentText.text = newValue.ToString();
    }

    public void UseRotatePointChanged (){
        //purely attached -- forward% variable
        attachedMoveController.useRotatePoint = attachedMoveController.useRotatePoint ? false : true; 
        useRotatePointVariableText.text = attachedMoveController.useRotatePoint.ToString();
    }

    public void UseHeliCompareChanged (){
        //purely attached -- forward% variable
        attachedMoveController.useHeliCompare = attachedMoveController.useHeliCompare ? false : true; 
        useHeliCompareVariableText.text = attachedMoveController.useHeliCompare.ToString();
    }

    public void RemoteSpeedChanged(float newValue){
        //purely remote
        remoteMoveController.remotePointMoveSpeed = newValue;
        remoteSpeedVText.text = newValue.ToString();
        Debug.Log("move speed change called");
    }

    public void ChangePaidVersion()
    {
        PlayerPrefs.SetString("FullAppVersion", "False");
        PlayerPrefs.Save();
        SaveManager.instance.SetGameVersion();
        paidVersionVText.text = PlayerPrefs.GetString("FullAppVersion");
    }

    void InitVariableText(){
        paidVersionVText.text = PlayerPrefs.GetString("FullAppVersion"); //setting variable text for button for switching saved app version to false
        Mode currentMode = heliMoveModeManager.useRemoteMode ? currentMode = Remote : currentMode = Attached;
        currentMode();
        void Remote(){
            horizVText.text = remoteMoveController.horizontalSpeed.ToString();
            vertVText.text = remoteMoveController.verticalSpeed.ToString();
            rollVtext.text = remoteMoveController.rollSpeed.ToString();
            cutoffVText.text = remoteMoveController.rollCutoff.ToString();
            rotationVText.text = remoteMoveController.rotationSpeed.ToString();
            distanceVText.text = arMovePoint.distanceToCamera.ToString();
            
            
            remoteSpeedVText.text = remoteMoveController.remotePointMoveSpeed.ToString();
        }
        void Attached (){
            horizVText.text = attachedMoveController.horizontalSpeed.ToString();
            vertVText.text = attachedMoveController.verticalSpeed.ToString();
            rollVtext.text = attachedMoveController.rollSpeed.ToString();
            cutoffVText.text = attachedMoveController.rollCutoff.ToString();
            rotationVText.text = attachedMoveController.rotationSpeed.ToString();
            distanceVText.text = arMovePoint.distanceToCamera.ToString();
            forwardPercentText.text = attachedMoveController.forwardTotalPercent.ToString();
            useRotatePointVariableText.text = attachedMoveController.useRotatePoint.ToString();
            useHeliCompareVariableText.text = attachedMoveController.useHeliCompare.ToString();
        }
       
    }
}
