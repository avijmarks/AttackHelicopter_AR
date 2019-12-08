using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPStore : MonoBehaviour
{
    
    public void OnPurchaseComplete (Product product){
#if UNITY_EDITOR
        StartCoroutine(SwitchToPaidEditor());
#else
        SaveManager.instance.ChangeVersion_Full("FullAppVersion");
#endif       
    }

    public void OnPurchaseFailure (Product product, PurchaseFailureReason reason){
        Debug.Log("Purchase of Product" + product.definition.id + "failed due to" + reason);
    }

    private IEnumerator SwitchToPaidEditor (){
        //fixes error testing IAP in editor
        yield return new WaitForEndOfFrame();
        SaveManager.instance.ChangeVersion_Full("FullAppVersion");
    }

}
