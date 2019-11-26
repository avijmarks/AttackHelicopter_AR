using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 
    public bool paidVersion = false;

    [SerializeField]
    protected IAPDelayWindowManager iAPDelayWindowManager;
    

    
    void Awake()
    {
        //singleton code :)
        if (instance == null){
            instance = this; 
        } else if (instance != this){
            Destroy(gameObject);
        }
    }

    void Start ()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    


    //old function for b/w screen effect, DELETE AND DELETE ALL POSTPROCESSING STUFF FROM ASSETS
    // void BlackWhiteScreen (){
    //     ColorGrading colorGradingLayer;
    //     blackAndWhiteEffect.profile.TryGetSettings(out colorGradingLayer);
    //     colorGradingLayer.enabled.value = true;
    // }
    
    
}
