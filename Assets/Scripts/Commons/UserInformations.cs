using UnityEngine;

public static class UserInformations
{
    private const string HEART_COUNT = "HeartCount";
    private const string LAST_STAGE_INDEX = "LastStageIndex";

    // 하트 수
    public static int HeartCount
    {
        get
        { return PlayerPrefs.GetInt(HEART_COUNT, 5); }
        set
        { PlayerPrefs.SetInt(HEART_COUNT, value); }
    }
    
    // 스테이지 클리어 정보
    public static int LastStageIndex
    {
        get { return PlayerPrefs.GetInt(LAST_STAGE_INDEX, 0); }
        set { PlayerPrefs.SetInt(LAST_STAGE_INDEX, value); }
    }
    
    // 효과음 재생 여부
    public static bool IsPlaySFX
    {
        get { return PlayerPrefs.GetInt("IsPlaySFX", 1) == 1; }
        set { PlayerPrefs.SetInt("IsPlaySFX", value ? 1 : 0); }
    }
    
    // 배경음악 재생 여부
    public static bool IsPlayBGM
    {
        get { return PlayerPrefs.GetInt("IsPlayBGM", 1) == 1; }
        set { PlayerPrefs.SetInt("IsPlayBGM", value ? 1 : 0); }
    }

    // 광고 제거 여부
    public static bool IsNoAds
    {
        get { return PlayerPrefs.GetInt("IsNoAds", 0) == 1; }
        set { PlayerPrefs.SetInt("IsNoAds", value ? 1 : 0); }
    }
}