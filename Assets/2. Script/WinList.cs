using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinList : MonoBehaviour
{
    public GameObject runnerWin;
    public GameObject taggerWin;
    public GameObject lobbyLodingPanel;
    private void Start()
    {
        ClearMgr.Instance.lobbyLodingPanel = lobbyLodingPanel;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (ClearMgr.Instance.role == Role.Tagger)
        {
            runnerWin.SetActive(false);
            taggerWin.SetActive(true);
        }
        else if(ClearMgr.Instance.role == Role.Runner)
        {
            runnerWin.SetActive(true);
            taggerWin.SetActive(false);
        }
    }

    public void OnClickedLobbyBtn()
    {
        ClearMgr.Instance.lobbyLodingPanel = null;
        ClearMgr.Instance.MoveLobby();
    }


}
