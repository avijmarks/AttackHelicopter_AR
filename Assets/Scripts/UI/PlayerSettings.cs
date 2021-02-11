using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings instance; 
    //saved settings here:
    public bool devSettingsEnabled = false;
    public bool environmentGenEnabled = true;
    public bool soundEnabled = true;

    //orientation independent references here
    public ARSession aRSession;
    public RemoteHeliMove remoteHeliMove;
    public GameObject devSettingsPanel;
    public HeliMoveModeManager heliMoveManager;


    //variables for switching between screen orientations

    [System.Serializable]
    public class OrientationLayout
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

   
    public OrientationLayout portrait = new OrientationLayout();
    public OrientationLayout landscape = new OrientationLayout();
    private OrientationLayout currentOrientationLayout;
    private bool wasPreviousPortrait;
    bool isCurrentModePortrait = true;
    bool areSettingsOpen = false;

    //joystick orientation layouts
    private FixedJoystick landscapeJoystick;
    private FixedJoystick portraitJoystick;
    public FixedJoystick currentJoystick;

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
    }

    void Start()
    {
        if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Landscape)
        {
            currentOrientationLayout = landscape;
        }
        else if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Portrait)
        {
            currentOrientationLayout = portrait;
        }

        if (devSettingsEnabled == true) GameManager.instance.paidVersion = true;

        UIOrientationManager.instance.OnSwitchedToPortrait += SwitchToPortraitLayout;
        UIOrientationManager.instance.OnSwitchedToLandscape += SwitchToLandscapeLayout;
        InitializeUI();
    }

    //receives layout change events from UIOrientationManager
    void SwitchToLandscapeLayout ()
    {
        portrait.panel.gameObject.SetActive(false);
        currentOrientationLayout = landscape;

        if (areSettingsOpen) currentOrientationLayout.panel.gameObject.SetActive(true);
        InitializeUI();
        Debug.LogError("Switched to Landscape");
    }

    void SwitchToPortraitLayout ()
    {
        landscape.panel.gameObject.SetActive(false);
        currentOrientationLayout = portrait;

        if (areSettingsOpen) currentOrientationLayout.panel.gameObject.SetActive(true);
        InitializeUI();
        Debug.LogError("Switched To Portrait Layout");
    }


    void InitializeUI(){
        //initializes UI graphics based on saved/current settings
        ChangeFlightModeUI();
        ChangeSoundEffectSettingsUI();
        ChangeEnvironmentUI();
        ChangeFlightModeUI();
    }


    public void OpenSettings (){
        //this.gameObject.SetActive(true);
        AudioManager.instance.ClickSound();

        if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Landscape)
        {
            currentOrientationLayout = landscape;
        }
        else if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Portrait)
        {
            currentOrientationLayout = portrait; 
        }

        currentOrientationLayout.panel.gameObject.SetActive(true);
        currentOrientationLayout.devSettingsButton.SetActive(devSettingsEnabled);
        areSettingsOpen = true;
    }

    public void CloseSettings(){
        AudioManager.instance.ClickSound();
        currentOrientationLayout.panel.gameObject.SetActive(false);
        areSettingsOpen = false; 
        //this.gameObject.SetActive(false);
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
            currentOrientationLayout.attachedModeButton.interactable = false;
            currentOrientationLayout.attachedButtonText.color = Color.white;

            currentOrientationLayout.remoteControlButton.interactable = true;
            currentOrientationLayout.remoteButtonText.color = currentOrientationLayout.remoteControlButton.colors.disabledColor;
        } else if (heliMoveManager.useRemoteMode){
            currentOrientationLayout.remoteControlButton.interactable = false;
            currentOrientationLayout.remoteButtonText.color = Color.white;

            currentOrientationLayout.attachedModeButton.interactable = true;
            currentOrientationLayout.attachedButtonText.color = currentOrientationLayout.attachedModeButton.colors.disabledColor;
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
        currentOrientationLayout.soundOffSprite.enabled = soundEnabled ? false : true;
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
        currentOrientationLayout.environmentDisabledSprite.enabled = environmentGenEnabled ? false : true;
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
