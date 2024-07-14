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
    public Texture2D cusor;
    public bool win;
    public Role role;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Cursor.SetCursor(cusor, Vector2.zero, CursorMode.ForceSoftware);
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
        SoundMgr.Instance.startMusic.gameObject.SetActive(false);
        SoundMgr.Instance.inGameMusic.gameObject.SetActive(false);

        PhotonNetwork.LeaveRoom();
    }
}

public enum Role
{
    Tagger,
    Runner,
    Start
}

