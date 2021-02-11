using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPDelayWindowManager : MonoBehaviour
{   
    //[SerializeField]
    //protected GameObject unpaidBanner;
    //[SerializeField]
    //protected Text countDownText;
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

    [System.Serializable]
    public class OrientationLayout
    {
        //for wait panel
        public Text unpaidWaitCountdownText;
        public GameObject unpaidWaitPanel;

        //for unpaid banner
        public GameObject unpaidBanner;
        public Text unpaidBannerCountDownText;
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
        currentOrientationLayout.unpaidBanner.SetActive(false);
        unpaidWaitPanel.SetActive(false);
        restoreButton.SetActive(false);
        fullVersionButton.SetActive(false);
        UIOrientationManager.instance.OnSwitchedToLandscape -= SwitchToLandscapeLayout;
        UIOrientationManager.instance.OnSwitchedToPortrait -= SwitchToPortraitLayout;
    }

    void StartCountdownCycle (){
        currentOrientationLayout.unpaidBanner.SetActive(true);
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
            float timePassed = PlayerSettings.instance.areSettingsOpen ? 0f : Time.deltaTime;
            time -= timePassed;
            currentTimeString = Mathf.RoundToInt(time).ToString();
            currentOrientationLayout.unpaidBannerCountDownText.text = currentTimeString;
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
            float timePassed = PlayerSettings.instance.areSettingsOpen ? 0f : Time.deltaTime;
            time -= timePassed;
            currentTimeString = Mathf.RoundToInt(time).ToString();
            currentOrientationLayout.unpaidWaitCountdownText.text = currentTimeString;
            yield return null;
        }
        panelCountDownActive = false;
        currentOrientationLayout.unpaidWaitPanel.SetActive(false);
    }

    void SwitchToLandscapeLayout()
    {
        currentOrientationLayout = landscape;

        if (panelCountDownActive)
        {
            landscape.unpaidWaitPanel.gameObject.SetActive(true);
            portrait.unpaidWaitPanel.gameObject.SetActive(false);
        }

        if (bannerCountDownActive)
        {
            landscape.unpaidBanner.gameObject.SetActive(true);
            landscape.unpaidBannerCountDownText.text = portrait.unpaidBannerCountDownText.text;
            portrait.unpaidBanner.gameObject.SetActive(false);
            
        }

            
        Debug.LogError("Switched to Landscape -- IAP UI");
    }

    void SwitchToPortraitLayout()
    {
        currentOrientationLayout = portrait;

        if (panelCountDownActive)
        {
            portrait.unpaidWaitPanel.gameObject.SetActive(true);
            landscape.unpaidWaitPanel.gameObject.SetActive(false);
        }

        if(bannerCountDownActive)
        {
            portrait.unpaidBanner.SetActive(true);
            portrait.unpaidBannerCountDownText.text = landscape.unpaidBannerCountDownText.text;
            landscape.unpaidBanner.SetActive(false);
        }

        Debug.LogError("Switched To Portrait Layout -- IAP UI");
    }
}
