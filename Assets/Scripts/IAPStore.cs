using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPStore : MonoBehaviour
{
    
    public void OnPurchaseComplete (Product product){
        SaveManager.instance.ChangeVersion_Full("FullAppVersion");
    }

    public void OnPurchaseFailure (Product product, PurchaseFailureReason reason){
        Debug.Log("Purchase of Product" + product.definition.id + "failed due to" + reason);
    }

}
