using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public void PurchaseFullVersion()
    {
        IAPStore.instance.PurchaseFullVersion();
    }
}
