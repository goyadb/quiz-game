using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainPanelController : MonoBehaviour
{
    [SerializeField] private TMP_Text _heartText;           // 남은 하트 수
    [SerializeField] private TMP_Text _stageText;           // 현재 스테이지

    /// <summary>
    /// Play Button 눌렀을 때 호출되는 메서드
    /// </summary>
    public void OnClickPlayButton()
    {
        GameManager.Instance.StartGame();
    }

    #region Main Menu 버튼 클릭 함수
    /// <summary>
    /// Shop 아이콘 터치시 호출되는 메서드
    /// </summary>
    public void OnClickShopButton()
    {
    }

    /// <summary>
    /// Stage 아이콘 터치시 호출되는 메서드
    /// </summary>
    public void OnClickStageButton()
    {
    }
    
    /// <summary>
    /// Leaderboard 아이콘 터치시 호출되는 메서드
    /// </summary>
    public void OnClickLeaderboardButton()
    {
    }

    /// <summary>
    /// Settings 아이콘 터치시 호출되는 메서드
    /// </summary>
    public void OnClickSettingsButton()
    {
    }
    #endregion
}
