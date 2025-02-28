using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using JetBrains.Annotations;

public class GPGSManager : Singleton<GPGSManager>
{
#if UNITY_ANDROID
    private const string leaderboardId = "CgkIw_uD_pgCEAIQAQ";
#elif UNITY_IOS

#endif

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("GPGS Authentication Success");

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string profileImage = PlayGamesPlatform.Instance.GetUserImageUrl();
        }
        else
        {
            Debug.Log("GPGS Authentication Failed");
        }
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    // 스코어를 리더보드에 전달
    public void ReportScore(int score)
    {
        Social.ReportScore(score, leaderboardId, success =>
        {
            if (success)
            {
                Debug.Log("Leaderboard report score success.");
            }
            else
            {
                Debug.Log("Leaderboard report score failed.");
            }
        });
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

}
