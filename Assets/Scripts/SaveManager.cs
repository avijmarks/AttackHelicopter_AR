using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    ///<summary> Values are strings "True" and "False" </summary>
    string FullAppVersion_Key = "FullAppVersion";





     void Awake(){
        //singleton code :)
        if (instance == null){
            instance = this; 
        } else if (instance != this){
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CheckPrefsEntry();
        SetGameVersion();
    }

    ///<summary>
    ///Checks if the setting has a KVP on file and sets default if there is none. All saved playerprefs items must have an entry here. 
    ///</summary>
    void CheckPrefsEntry(){
        if (!PlayerPrefs.HasKey(FullAppVersion_Key))
        {
            PlayerPrefs.SetString(FullAppVersion_Key, "False");
            PlayerPrefs.Save();   
        }
        if (!PlayerPrefs.HasKey("TipDisplayed"))
        {
            PlayerPrefs.SetString("TipDisplayed", "False");
            PlayerPrefs.Save();
        }
        
    }

    ///<summary>
    ///saves to file that the player has the paid version of the app and makes appropriate adjustments in game via SetAppVersion()
    ///</summary>
    public void ChangeVersion_Full(){
        PlayerPrefs.SetString(FullAppVersion_Key, "True");
        PlayerPrefs.Save();
        SetGameVersion();
    }

    ///<summary> 
    ///Sets the boolean that actually controls the game (paidVersion on GameManager.cs) to true or false based on current app version on file
    ///</summary>
    public void SetGameVersion (){
        if (PlayerPrefs.GetString(FullAppVersion_Key) == "True"){
            GameManager.instance.paidVersion = true;
        } else if (PlayerPrefs.GetString(FullAppVersion_Key) == "False"){
            GameManager.instance.paidVersion = false;
        }
        
    }


    ///<summary>
    ///Sets the game to no longer display the tool tip as it has already been displayed once locally
    ///</summary>
    public void NoLongerDisplayTips (){
        PlayerPrefs.SetString("TipDisplayed", "True");
        PlayerPrefs.Save();
    }


}
