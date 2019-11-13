using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    //true == remote control
    public bool devSettingsEnabled = false;
    public bool environmentGenEnabled = true;
    public GameObject devSettingsButton;
    public GameObject devSettingsPanel;
    public HeliMoveModeManager heliMoveManager;
    public bool standInForFlightBool = false;
    public bool soundEnabled = true;
    public Button attachedModeButton;
    public Button remoteControlButton;
    public Text attachedButtonText;
    public Text remoteButtonText;
    public Button soundEffectButton; 
    public Image soundOffSprite;
    public Image environmentDisabledSprite;

    
    
    
    void Start()
    {
        ChangeFlightModeUI();
    }


    public void OpenSettings (){
        AudioManager.instance.ClickSound();
        this.gameObject.SetActive(true);
        devSettingsButton.SetActive(devSettingsEnabled);
    }

    public void CloseSettings(){
        AudioManager.instance.ClickSound();
        this.gameObject.SetActive(false);
    }

    //set stand in bool for flight mode
    //set color and text color based on mode
    public void ChangeFlightMode(){
        AudioManager.instance.ClickSound();
        //just swappin boolean values
        heliMoveManager.useRemoteMode = !heliMoveManager.useRemoteMode;
        ChangeFlightModeUI();
        IHeliMoveMode newMode = heliMoveManager.useRemoteMode ? heliMoveManager.remoteHeliMove : heliMoveManager.attachedHeliMove;
        heliMoveManager.ChangeHeliMoveMode(newMode);
    }

    void ChangeFlightModeUI(){
        //changes color and text color of flightmode ui buttons
        if (!heliMoveManager.useRemoteMode){
            attachedModeButton.interactable = false;
            attachedButtonText.color = Color.white;

            remoteControlButton.interactable = true;
            remoteButtonText.color = remoteControlButton.colors.disabledColor;
        } else if (heliMoveManager.useRemoteMode){
            remoteControlButton.interactable = false;
            remoteButtonText.color = Color.white;

            attachedModeButton.interactable = true;
            attachedButtonText.color = attachedModeButton.colors.disabledColor;
        }
        
    }

    public void ChangeSoundEffectSettings (){
        
        //change sound stuff here when implemented (based on bool)
        soundEnabled = !soundEnabled;
        soundOffSprite.enabled = soundEnabled ? false : true;
        if (soundEnabled){
            soundOffSprite.enabled = false;
            AudioManager.instance.EnableSound();
        } else if (!soundEnabled) {
            soundOffSprite.enabled = true;
            AudioManager.instance.DisableSound();
        }
        //play click sound after so if disabling sound we dont get any noise and vice versa
        AudioManager.instance.ClickSound();
    }

    public void ChangeEnvironmentEnabled(){
        AudioManager.instance.ClickSound();

        //changing bool
        environmentGenEnabled = !environmentGenEnabled;

        //actually enabling or disabling spawning and associated UI
        if (environmentGenEnabled){
            //do environment gen things
            environmentDisabledSprite.enabled = false;
            PlaneObjectData.singleton.EnableEnvironmentSpawning();
        } else if (!environmentGenEnabled){
            //do environment gen disabled things
            environmentDisabledSprite.enabled = true;
            PlaneObjectData.singleton.DisableEnvironmentSpawning();
        }

    }

    public void ShowCredits(){
        AudioManager.instance.ClickSound();
    }

    public void SetUIOnLoad(){
        //this function needs to set appropriate UI on load given player settings in place

    }

}
