﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Text horizVText;
    public Text vertVText;
    public Text rollVtext;
    public Text cutoffVText;
    public Text rotationVText;
    public Text distanceVText;
    public Text forwardPercentText;
    public GameObject arMovePoint;
    public HeliMove heliMoveController;


    // Start is called before the first frame update
    void Start()
    {
        settingsPanel.SetActive(false);
    }


    public void OpenSettings (){
        settingsPanel.SetActive(true);
        InitVariableText();
    }

    public void CloseSettings (){
        settingsPanel.SetActive(false);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public void HorizontalChanged (float newValue){
        //50-150
        heliMoveController.horizontalSpeed = newValue;
        horizVText.text = newValue.ToString();
    }
    public void VerticalChanged(float newValue){
        //50-150
        heliMoveController.verticalSpeed = newValue;
        vertVText.text = newValue.ToString();
    }
    public void RollChanged(float newValue){
        //0-1
        heliMoveController.rollSpeed = newValue;
        rollVtext.text = newValue.ToString();
    }
    public void CutoffChanged(float newValue){
        //0-5
        heliMoveController.rollCutoff = newValue;
        cutoffVText.text = newValue.ToString();
    }
    public void RotationChanged(float newValue){
        //0-0.25
        heliMoveController.rotationSpeed = newValue;
        rotationVText.text = newValue.ToString();
    }
    public void DistanceChanged(float newValue){
        var newDistance = new Vector3(0f, 0f, newValue);
        arMovePoint.transform.localPosition = newDistance;
        distanceVText.text = newValue.ToString();
    }

    public void ForwardPercentChanged (float newValue){
        heliMoveController.forwardTotalPercent = newValue;
        forwardPercentText.text = newValue.ToString();
    }

    void InitVariableText(){
        horizVText.text = heliMoveController.horizontalSpeed.ToString();
        vertVText.text = heliMoveController.verticalSpeed.ToString();
        rollVtext.text = heliMoveController.rollSpeed.ToString();
        cutoffVText.text = heliMoveController.rollCutoff.ToString();
        rotationVText.text = heliMoveController.rotationSpeed.ToString();
        distanceVText.text = arMovePoint.transform.localPosition.z.ToString();
        forwardPercentText.text = heliMoveController.forwardTotalPercent.ToString();
    }
}
