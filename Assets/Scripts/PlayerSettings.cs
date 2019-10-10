using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    //true == remote control
    public bool standInForFlightBool = false;
    public bool standInForSoundBool = true;
    public Button attachedModeButton;
    public Button remoteControlButton;
    public Text attachedButtonText;
    public Text remoteButtonText;
    public Button soundEffectButton; 
    // Start is called before the first frame update
    void Start()
    {
        ChangeFlightModeUI();
    }


    //set stand in bool for flight mode
    //set color and text color based on mode
    public void ChangeFlightMode(){
        //just swappin boolean values
        standInForFlightBool = !standInForFlightBool;
        ChangeFlightModeUI();
    }

    void ChangeFlightModeUI(){
        //changes color and text color of flightmode ui buttons
        if (!standInForFlightBool){
            attachedModeButton.interactable = false;
            attachedButtonText.color = Color.white;

            remoteControlButton.interactable = true;
            remoteButtonText.color = remoteControlButton.colors.disabledColor;
        } else if (standInForFlightBool){
            remoteControlButton.interactable = false;
            remoteButtonText.color = Color.white;

            attachedModeButton.interactable = true;
            attachedButtonText.color = attachedModeButton.colors.disabledColor;
        }
        
    }

    public void ChangeSoundEffectSettings (){
        //change sound stuff here when implemented
        standInForSoundBool = !standInForSoundBool;
        soundEffectButton. = standInForSoundBool;
    }

}
