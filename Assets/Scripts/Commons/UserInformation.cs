using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInformation : MonoBehaviour
{
    private const string HEART_COUNT = "HeartCount";
    private const string LAST_STAGE_INDEX = "LastStageIndex";

    public static int HeartCount
    {
        get
        {
            return PlayerPrefs.GetInt(HEART_COUNT, 5);
        }
        set
        {
            PlayerPrefs.SetInt(HEART_COUNT, value);
        }
    }

    public static int LastStageIndex
    {
        get
        {
            return PlayerPrefs.GetInt(LAST_STAGE_INDEX, 0);
        }
        set
        {
            PlayerPrefs.SetInt(LAST_STAGE_INDEX, value);
        }
    }
    
}
