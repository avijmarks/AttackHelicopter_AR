using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPStore : MonoBehaviour, IStoreListener
{
    public static IAPStore instance;
    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;

    private string fullVersion = "full_version";

    public void InitializePurchasing()
    {
        if (IsInitialized()) return;
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(fullVersion, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && extensionProvider != null;
    }

    public void PurchaseFullVersion ()
    {
        BuyProductID(fullVersion);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //???????? will this change work
        if (string.Equals(args.purchasedProduct.definition.id, fullVersion))
        {
            //do code here n stuff
            SaveManager.instance.ChangeVersion_Full();
            Debug.Log("Full Version Purchased Successfully");
        }
        else
        {
            Debug.Log("Purchase Failed at ProcessPurchase()");
        }
        return PurchaseProcessingResult.Complete;
    }



    /// <summary>
    /// //////////////////
    /// </summary>

    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (storeController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = extensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        storeController = controller;
        extensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    //    public void OnPurchaseComplete (Product product){
    //#if UNITY_EDITOR
    //        StartCoroutine(SwitchToPaidEditor());
    //#else
    //        SaveManager.instance.ChangeVersion_Full("FullAppVersion");
    //#endif       
    //    }

    //    public void OnPurchaseFailure (Product product, PurchaseFailureReason reason){
    //        Debug.Log("Purchase of Product" + product.definition.id + "failed due to" + reason);
    //    }

    //    private IEnumerator SwitchToPaidEditor (){
    //        //fixes error testing IAP in editor
    //        yield return new WaitForEndOfFrame();
    //        SaveManager.instance.ChangeVersion_Full("FullAppVersion");
    //    }

}
