using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlayerSettings : MonoBehaviour
{
    //saved settings here:
    public bool devSettingsEnabled = false;
    public bool environmentGenEnabled = true;
    public bool soundEnabled = true;

    //orientation independent references here
    public ARSession aRSession;
    public RemoteHeliMove remoteHeliMove;
    public GameObject devSettingsPanel;
    public HeliMoveModeManager heliMoveManager;

    //stuff that needs to be put in a struct/class for portrait/landscape alternation



    public GameObject devSettingsButton;
    public Button attachedModeButton;
    public Button remoteControlButton;
    public Text attachedButtonText;
    public Text remoteButtonText;
    public Button soundEffectButton; 
    public Image soundOffSprite;
    public Image environmentDisabledSprite;

    [System.Serializable]
    public class OrientationReferences
    {
        public GameObject panel; 
        public Button attachedModeButton;
        public Button remoteControlButton;
        public Text attachedButtonText;
        public Text remoteButtonText;
        public Button soundEffectButton;
        public Image soundOffSprite;
        public Image environmentDisabledSprite;
        public GameObject devSettingsButton;
    }

   
public OrientationReferences portrait = new OrientationReferences();
    public OrientationReferences landscape = new OrientationReferences();
    
    
    
    void Start()
    {
        InitializeUI();
        Debug.LogError(remoteHeliMove.ToString());
    }

    private void FixedUpdate()
    {
        if (devSettingsEnabled == true) GameManager.instance.paidVersion = true; 
    }

    void InitializeUI(){
        //initializes UI graphics based on saved/current settings
        ChangeFlightModeUI();
        ChangeSoundEffectSettingsUI();
        ChangeEnvironmentUI();
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
        //change sound stuff 
        soundEnabled = !soundEnabled;
        if (soundEnabled){
            AudioManager.instance.EnableSound();
        } else if (!soundEnabled) {
            AudioManager.instance.DisableSound();
        }
        ChangeSoundEffectSettingsUI();
        //play click sound after so if disabling sound we dont get any noise and vice versa
        AudioManager.instance.ClickSound();
    }
    void ChangeSoundEffectSettingsUI(){
        soundOffSprite.enabled = soundEnabled ? false : true;
    }

    public void ChangeEnvironmentEnabled(){
        AudioManager.instance.ClickSound();

        //changing bool
        environmentGenEnabled = !environmentGenEnabled;

        //actually enabling or disabling spawning and associated UI
        if (environmentGenEnabled){
            //do environment gen things
            PlaneObjectData.singleton.EnableEnvironmentSpawning();
        } else if (!environmentGenEnabled){
            //do environment gen disabled things
            PlaneObjectData.singleton.DisableEnvironmentSpawning();
        }
        ChangeEnvironmentUI();
    }
    void ChangeEnvironmentUI(){
        environmentDisabledSprite.enabled = environmentGenEnabled ? false : true;
    }

    public void ShowCredits(){
        AudioManager.instance.ClickSound();
    }

    public void SetUIOnLoad(){
        //this function needs to set appropriate UI on load given player settings in place

    }

    public void RestartSession(){
        //restarts ARSession -- clears current objects... maintains settings from backend
        //needs to put helicopter back in front of camera as though it is at attached cam movepoint
        AudioManager.instance.ClickSound();
        aRSession.Reset();
        PlaneObjectData.singleton.DestroyCurrentEnvironment();
        remoteHeliMove.ResetRemotePoint();
    }

}
