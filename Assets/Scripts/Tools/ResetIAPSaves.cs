using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetIAPSaves : MonoBehaviour
{
    void Start()
    {
        SaveManager.instance.ClearPlayerPrefs(); 
    }
}
