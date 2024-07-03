using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearMgr : MonoBehaviourPunCallbacks
{
    private static ClearMgr instance;
    public static ClearMgr Instance
    {
        get { return instance; }
    }
    public GameObject lobbyLodingPanel;

    public bool win;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("LeftRoom ¹æ¶°³²");
        if(lobbyLodingPanel != null)
            lobbyLodingPanel.SetActive(true);
        
        PhotonNetwork.LoadLevel("SampleScene");

    }
    public void MoveLobby()
    {
        SoundMgr.Instance.lobbyMusic.gameObject.SetActive(true);
        SoundMgr.Instance.inGameMusic.gameObject.SetActive(false);

        PhotonNetwork.LeaveRoom();
    }
}
