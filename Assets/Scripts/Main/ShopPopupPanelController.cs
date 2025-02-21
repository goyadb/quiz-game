using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PopupPanelController))]
public class ShopPopupPanelController : MonoBehaviour
{
    [SerializeField] private GameObject restorePurchaseButtonObject;
    
    private void Start()
    {
        GetComponent<PopupPanelController>().SetTitleText("SHOP");
        
#if UNITY_IOS
        restorePurchaseButtonObject.SetActive(true);
#else
        restorePurchaseButtonObject.SetActive(false);
#endif
    }

    public void OnClickShopItemButton(int index)
    {
        switch (index)
        {
            case 0:
                // TODO: 하트 60개 + 광고 제거
                break;
            case 1:
                // TODO: 광고보고 하트 3개 받기
                break;
            case 2:
                // TODO: 하트 20개
                break;
            case 3:
                // TODO: 하트 60개
                break;
            case 4:
                // TODO: 하트 150개
                break;
            case 5:
                // TODO: 하트 320개
                break;
            case 6:
                // TODO: 하트 450개
                break;
            case 7:
                // TODO: 광고 제거
                break;
            case 8:
                // TODO: 구매 복원
                break;
        }
    }
}
