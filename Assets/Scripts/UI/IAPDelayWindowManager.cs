﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPDelayWindowManager : MonoBehaviour
{   
    [SerializeField]
    protected GameObject unpaidBanner;
    [SerializeField]
    protected Text countDownText;
    [SerializeField]
    protected GameObject unpaidWaitPanel;
    [SerializeField]
    protected Text unpaidWaitCountdownText;
    [SerializeField]
    protected GameObject restoreButton;
    [SerializeField]
    protected GameObject fullVersionButton;
    

    bool bannerCountDownActive = false;
    bool panelCountDownActive = false;
    IEnumerator coroutineToStart;
    int promptDisplayCount = 0;
    float initialBannerCountTime = 30f;
    float normalBannerCountTime = 60f;
    float panelDisplayTime = 10f;

    [SerializeField]
    protected GameObject playerSettings;

    [System.Serializable]
    public class OrientationLayout
    {
        public Text unpaidWaitCountdownText;
        public GameObject unpaidWaitPanel;
    }

    public OrientationLayout landscape = new OrientationLayout();
    public OrientationLayout portrait = new OrientationLayout();
    private OrientationLayout currentOrientationLayout;

    public void Start()
    {
        if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Landscape)
        {
            currentOrientationLayout = landscape;
        }
        else if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Portrait)
        {
            currentOrientationLayout = portrait;
        }

        UIOrientationManager.instance.OnSwitchedToPortrait += SwitchToPortraitLayout;
        UIOrientationManager.instance.OnSwitchedToLandscape += SwitchToLandscapeLayout;
    }

    //ALL THIS LOGIC FUNCTIONS AS A BIG LOOP THAT RUNS 2 COROUTINES(ONE FOR BANNER ONE FOR PANEL COUNTDOWNS)
    //CONSTANTLY CHECKS PAID VERSION BOOLEAN IN GAMEMANAGER.CS AND CAN STOP ANY TIME THATS CHANGE (CHECK IN UPDATE)
    //CAN CHANGE INITIAL BANNER COUNTDOWN, THE NORMAL COUNTDOWN, AND THE PANEL DISPLAY COUNTDOWN IN VARIABLES ABOVE

    void Update()
    {
        if (GameManager.instance.paidVersion == true){
            StopAllCoroutines();
            DisableAllIAPPrompts();
        }

        if (!bannerCountDownActive && !panelCountDownActive && !GameManager.instance.paidVersion){
            StartCountdownCycle();
        }

        
    }

    void DisableAllIAPPrompts(){
        unpaidBanner.SetActive(false);
        unpaidWaitPanel.SetActive(false);
        restoreButton.SetActive(false);
        fullVersionButton.SetActive(false);
    }

    void StartCountdownCycle (){
        bannerCountDownActive = true;
        float bannerCountTime = promptDisplayCount > 0 ? normalBannerCountTime : initialBannerCountTime;
        promptDisplayCount += 1;
        BannerCountDown(bannerCountTime);
    }
    
    public void BannerCountDown (float initialCountTime)
    {
        coroutineToStart = IBannerCountDown(initialCountTime);
        StartCoroutine(coroutineToStart);
    }

    IEnumerator IBannerCountDown (float countTime)
    {
        float time = countTime;
        string currentTimeString;
        while (time > 0){
            float timePassed = playerSettings.activeSelf ? 0f : Time.deltaTime;
            time -= timePassed;
            currentTimeString = Mathf.RoundToInt(time).ToString();
            countDownText.text = currentTimeString;
            yield return null;
        }
        bannerCountDownActive = false;
        ShowUnpaidPanel();  
    }

    void ShowUnpaidPanel()
    {
        panelCountDownActive = true;

        if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Landscape)
        {
            currentOrientationLayout = landscape;
        }
        else if (UIOrientationManager.instance.currentOrientation == UIOrientationManager.Orientation.Portrait)
        {
            currentOrientationLayout = portrait;
        }

        coroutineToStart = IWaitPanelCountdown(panelDisplayTime);
        StartCoroutine(coroutineToStart);
        Debug.Log("when is this called");
    }

    IEnumerator IWaitPanelCountdown (float countTime)
    {
        currentOrientationLayout.unpaidWaitPanel.SetActive(true);
        float time = countTime;
        string currentTimeString;
        while (time > 0){
            float timePassed = playerSettings.activeSelf ? 0f : Time.deltaTime;
            time -= timePassed;
            currentTimeString = Mathf.RoundToInt(time).ToString();
            currentOrientationLayout.unpaidWaitCountdownText.text = currentTimeString;
            yield return null;
        }
        panelCountDownActive = false;
        unpaidWaitPanel.SetActive(false);
    }

    void SwitchToLandscapeLayout()
    {
        portrait.unpaidWaitPanel.gameObject.SetActive(false);
        currentOrientationLayout = landscape;

        if (panelCountDownActive) currentOrientationLayout.unpaidWaitPanel.gameObject.SetActive(true);
        Debug.LogError("Switched to Landscape");
    }

    void SwitchToPortraitLayout()
    {
        landscape.unpaidWaitPanel.gameObject.SetActive(false);
        currentOrientationLayout = portrait;

        if (panelCountDownActive) currentOrientationLayout.unpaidWaitPanel.gameObject.SetActive(true);
        Debug.LogError("Switched To Portrait Layout");
    }
}
