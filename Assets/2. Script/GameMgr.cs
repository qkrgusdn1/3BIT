using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameMgr : MonoBehaviourPunCallbacks
{
    private static GameMgr instance;
    public static GameMgr Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        instance = this;
    }
    public GameObject escPanel;
    public GameObject settingPanel;

    public TMP_Text playerCountText;

    public List<Player> players = new List<Player>();
    public Player player;
    public GameObject diePanel;
    public GameObject connection;
    public GameObject lobbyLodingPanel;

    public bool threePlayer;
    void Start()
    {
        SliderControl.sensitivityValue = PlayerPrefs.GetFloat("Sensitivity", 1);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
        StartCoroutine(CoUpdate());
        UpdatePlayerCountText();
    }
    public void MoveClearScenes()
    {
        photonView.RPC("RPCMoveClearScenes", RpcTarget.All);
    }

    [PunRPC]
    public void RPCMoveClearScenes()
    {
        PhotonNetwork.LoadLevel("ClearScenes");
    }
    public void AddPlayer()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            if (player.photonView.IsMine)
            {
                this.player = player;
                break;
            }
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("MultiGameMgr 현재 방의 접속한 플레이어 수 확인 {PhotonNetwork.PlayerList.Length}");
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("LeftRoom 방떠남");

        lobbyLodingPanel.SetActive(true);
        PhotonNetwork.LoadLevel("SampleScene");
    }

    public void OnClickedLobbyBtn()
    {
        SoundMgr.Instance.lobbyMusic.gameObject.SetActive(true);
        SoundMgr.Instance.inGameMusic.gameObject.SetActive(false);


        PhotonNetwork.LeaveRoom();
    }

    public IEnumerator ExecutePlayerCountAction()
    {
        while (true)
        {
            bool haveStartPlayer = false;
            for(int i = 0; i < players.Count; i++)
            {
                if (players[i].startPlayer)
                {
                    haveStartPlayer = true;
                    break;
                }
            }
            if (haveStartPlayer)
            {
                yield return new WaitForSeconds(1);
                continue;
            }
            else
            {
                break;
            }
        }

        while (true)
        {
            Debug.Log("PlayerCount");
            yield return new WaitForSeconds(1);
            if (players.Count == 1)
            {
                if (players[0].gameObject.CompareTag("Runner"))
                {
                    ClearMgr.Instance.win = true;
                    MoveClearScenes();
                }
                else if (players[0].gameObject.CompareTag("Tagger"))
                {
                    ClearMgr.Instance.win = false;
                    MoveClearScenes();
                }
            }
            else if (players.Count <= 3 && !player.threePlayer)
            {
                MissionMgr.Instance.allMissionCountBar.SetActive(false);
                MissionMgr.Instance.taggerImage.gameObject.SetActive(true);
                if (player.gameObject.CompareTag("Tagger"))
                {
                    player.maxMoveSpeed = player.maxMoveSpeed + 5;
                    player.moveSpeed = player.maxMoveSpeed;

                }
                else if (player.gameObject.CompareTag("Runner"))
                {
                    player.maxMoveSpeed = player.maxMoveSpeed - 3;
                    player.moveSpeed = player.maxMoveSpeed;

                }
                player.threePlayer = true;
            }
            yield return new WaitForSeconds(1);

            bool taggerExists = false;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].gameObject.CompareTag("Tagger"))
                {
                    taggerExists = true;
                    break;
                }
            }
            if (!taggerExists)
            {
                ClearMgr.Instance.win = true;
                MoveClearScenes();
            }

        }
    }

    public IEnumerator CoUpdate()
    {

        while (true)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                    i--;
                    if(!player.startPlayer)
                        UpdatePlayers();
                }
            }
            
            yield return new WaitForSeconds(1);
        }

    }


    void UpdatePlayers()
    {
        bool inTagger = false;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject.CompareTag("Tagger"))
            {
                inTagger = true;
                break;
            }
        }

        if (!inTagger)
        {
            ClearMgr.Instance.win = true;
            MoveClearScenes();
            return;
        }
        if (players.Count == 1)
        {
            if (players[0].gameObject.CompareTag("Runner"))
            {
                ClearMgr.Instance.win = true;
                MoveClearScenes();
            }
            else if (players[0].gameObject.CompareTag("Tagger"))
            {
                ClearMgr.Instance.win = false;
                MoveClearScenes();
            }
        }
        else if (players.Count <= 3 && !player.threePlayer)
        {
            MissionMgr.Instance.allMissionCountBar.SetActive(false);
            MissionMgr.Instance.taggerImage.gameObject.SetActive(true);
            if (player.gameObject.CompareTag("Tagger"))
            {
                player.maxMoveSpeed = player.maxMoveSpeed + 5;
                player.moveSpeed = player.maxMoveSpeed;

            }
            else if (player.gameObject.CompareTag("Runner"))
            {
                player.maxMoveSpeed = player.maxMoveSpeed - 3;
                player.moveSpeed = player.maxMoveSpeed;

            }
            player.threePlayer = true;
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdatePlayerCountText();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !player.esc)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            player.esc = true;
            escPanel.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && player.esc)
        {
            if (!player.mission)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            player.esc = false;
            settingPanel.SetActive(false);
            escPanel.SetActive(false);
        }
    }


    void UpdatePlayerCountText()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPCUpdatePlayerCountText", RpcTarget.All, PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    [PunRPC]
    void RPCUpdatePlayerCountText(int count)
    {
        playerCountText.text = "(" + count + "/4)";
    }


}
