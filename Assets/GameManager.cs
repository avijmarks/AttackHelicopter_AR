using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 
    public bool paidVersion = false;

    [SerializeField]
    protected Text countDownText;
    [SerializeField]
    protected GameObject unpaidVersionPanel;
    [SerializeField]
    protected PostProcessVolume blackAndWhiteEffect;
    // Start is called before the first frame update
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
        InitialCheckPaidVersion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitialCheckPaidVersion()
    {
        if (!paidVersion){
            CountDown();
        } else if (paidVersion){
            unpaidVersionPanel.SetActive(false);
        }
        
    }

    public void CountDown ()
    {
        StartCoroutine("ICountDown");
       
    }
    IEnumerator ICountDown ()
    {
        float time = 30f;
        string currentTimeString;
        while (time > 0){
            time -= Time.deltaTime;
            currentTimeString = Mathf.RoundToInt(time).ToString();
            countDownText.text = currentTimeString;
            yield return null;
        }

        BlackWhiteScreen();   
    }
    void BlackWhiteScreen (){
        ColorGrading colorGradingLayer;
        blackAndWhiteEffect.profile.TryGetSettings(out colorGradingLayer);
        colorGradingLayer.enabled.value = true;
    }
    
    
}
