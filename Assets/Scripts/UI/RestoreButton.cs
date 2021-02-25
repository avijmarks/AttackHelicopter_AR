using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreButton : MonoBehaviour
{
    public void RestorePurchases()
    {
        IAPStore.instance.RestorePurchases();
    }
}
