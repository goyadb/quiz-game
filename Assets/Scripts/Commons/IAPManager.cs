using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public enum Type
    {
        NOADS,          // 광고제거
        NOADS_HEART_60, // 광고제거 + 하트 60개
        HEART_20,
        HEART_60,
        HEART_150,
        HEART_320,
        HEART_450
    }

    public const string kPID_Heart20 = "heart_20";
    public const string kPID_Heart60 = "heart_60";
    public const string kPID_Heart150 = "heart_150";
    public const string kPID_Heart320 = "heart_320";
    public const string kPID_Heart450 = "heart_450";
    public const string kPID_NoAds = "noads";
    public const string kPID_NoAds_Heart60 = "noads_heart_60";

    private IStoreController _storeController;
    private IExtensionProvider _storeExtensionProvider;

    private void Start()
    {
        if (_storeController == null)
        {
            InitializePurchasing();
        }
    }

    private void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(kPID_Heart20, ProductType.Consumable);
        builder.AddProduct(kPID_Heart60, ProductType.Consumable);
        builder.AddProduct(kPID_Heart150, ProductType.Consumable);
        builder.AddProduct(kPID_Heart320, ProductType.Consumable);
        builder.AddProduct(kPID_Heart450, ProductType.Consumable);
        builder.AddProduct(kPID_NoAds, ProductType.NonConsumable);
        builder.AddProduct(kPID_NoAds_Heart60, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return _storeController != null && _storeExtensionProvider != null;
    }

    public void BuyProduct(Type type)
    {
        switch (type)
        {
            case Type.HEART_20:
                BuyProductID(kPID_Heart20);
                break;
            case Type.HEART_60:
                BuyProductID(kPID_Heart60);
                break;
            case Type.HEART_150:
                BuyProductID(kPID_Heart150);
                break;
            case Type.HEART_320:
                BuyProductID(kPID_Heart320);
                break;
            case Type.HEART_450:
                BuyProductID(kPID_Heart450);
                break;
            case Type.NOADS:
                BuyProductID(kPID_NoAds);
                break;
            case Type.NOADS_HEART_60:
                BuyProductID(kPID_NoAds_Heart60);
                break;
        }
    }

    private void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = _storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                _storeController.InitiatePurchase(product);
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


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _storeController = controller;
        _storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error + " message:" + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
            product.definition.storeSpecificId, failureReason));
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (String.Equals(purchaseEvent.purchasedProduct.definition.id, kPID_Heart20, StringComparison.Ordinal))
        {
            UserInformations.HeartCount += 20;
        }
        else if (String.Equals(purchaseEvent.purchasedProduct.definition.id, kPID_Heart60, StringComparison.Ordinal))
        {
            UserInformations.HeartCount += 60;
        }
        else if (String.Equals(purchaseEvent.purchasedProduct.definition.id, kPID_Heart150, StringComparison.Ordinal))
        {
            UserInformations.HeartCount += 150;
        }
        else if (String.Equals(purchaseEvent.purchasedProduct.definition.id, kPID_Heart320, StringComparison.Ordinal))
        {
            UserInformations.HeartCount += 320;
        }
        else if (String.Equals(purchaseEvent.purchasedProduct.definition.id, kPID_Heart450, StringComparison.Ordinal))
        {
            UserInformations.HeartCount += 450;
        }
        else if (String.Equals(purchaseEvent.purchasedProduct.definition.id, kPID_NoAds, StringComparison.Ordinal))
        {
            UserInformations.IsNoAds = true;
        }
        else if (String.Equals(purchaseEvent.purchasedProduct.definition.id, kPID_NoAds_Heart60, StringComparison.Ordinal))
        {
            UserInformations.HeartCount += 60;
            UserInformations.IsNoAds = true;
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", purchaseEvent.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }
}
