using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonMgr : MonoBehaviourPunCallbacks
{
    public List<RoomInfo> roomList;

    private static PhotonMgr instance;
    public static PhotonMgr Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        instance = this;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "kr";
    }

    public GameObject serverLodingPanel;
    public GameObject roomLodingPanel;
    private void Start()
    {
        TryToJoinServer();
    }

    public void TryToJoinServer()
    {
        
        Debug.Log("서버 연결 시도");
        if (!PhotonNetwork.IsConnected)
        {
            serverLodingPanel.SetActive(true);
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("PhotonNetwork.CloudRegion : " + PhotonNetwork.CloudRegion);
        Debug.Log("서버 접속 완료");
        serverLodingPanel.SetActive(false);
        
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("로비로 접속 시도");
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비 접속 완료");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("룸에 참가했습니다: " + PhotonNetwork.CurrentRoom.Name);
        SoundMgr.Instance.lobbyMusic.gameObject.SetActive(false);
        SoundMgr.Instance.inGameMusic.gameObject.SetActive(true);
        PhotonNetwork.LoadLevel("InGame");

    }
    public void TryToJoinRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("PhotonMgr OnRoomListUpdate");
        this.roomList = roomList;


    }
    //생성하는 함수
    public void CreateRoom(string roomTitle, int maxPlayer)
    {
        roomLodingPanel.SetActive(true);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;
        PhotonNetwork.CreateRoom(roomTitle, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("생성 성공");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("생성 실패");
    }
    public void JoinRoom(RoomInfo info)
    {
        Debug.Log("룸 접속 시도");
        PhotonNetwork.JoinRoom(info.Name);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("룸 접속 실패");
    }
}
