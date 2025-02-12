using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelController : MonoBehaviour
{

    public void OnClickGameOverButton()
    {
        GameManager.Instance.QuitGame();
    }
}
