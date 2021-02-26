using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 
    public bool paidVersion = false;

    public bool tipDisplayed = false;

    [SerializeField]
    protected IAPDelayWindowManager iAPDelayWindowManager;

    public GameObject controlModeTip;
    
    void Awake()
    {
        //singleton code :)
        if (instance == null){
            instance = this; 
        } else if (instance != this){
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetString("TipDisplayed") == "True")
        {
            tipDisplayed = true; 
        } else {
            StartCoroutine(DisplayFirstLoadTips());
        }
    }

    IEnumerator DisplayFirstLoadTips()
    {
        float tipDisplayTime = 45f;
        float tipDisplayLength = 8f;
        float t = 0; 
        Debug.Log("Displaying tooltip in" + tipDisplayTime + "seconds");
        while (t < tipDisplayTime)
        {
            t += Time.deltaTime;
            yield return null;

        }

        float windowOpenTime = 0; 
        controlModeTip.SetActive(true);

        while (windowOpenTime < tipDisplayLength)
        {
            windowOpenTime += Time.deltaTime;
            yield return null;
        }

        controlModeTip.SetActive(false);
        SaveManager.instance.NoLongerDisplayTips();
    }




    


    
    
    
}
