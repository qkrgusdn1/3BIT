using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class StartGame : MonoBehaviourPunCallbacks
{
    private static StartGame instance;
    public static StartGame Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        instance = this;
    }

    public TMP_Text countTxt;
    public TMP_Text gameTimerText;

    public int minutes;
    public int seconds;

    public bool taggerWin;

    float count;
    public float maxCount;

    public AudioSource music;

    public List<string> powers = new List<string>();

    public ConnectionCrystalPosition connectionCrystalPosition;
    public List<Player> players = new List<Player>();
    private void Start()
    {
        photonView.RPC("RPCEnteredPlayer", RpcTarget.All);
        music = SoundMgr.Instance.inGameMusic;
    }


    [PunRPC]
    public void RPCEnteredPlayer()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4 && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPCCountDown", RpcTarget.All);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            Debug.Log("LogLog");
        }
        
    }

    //private void Start()
    //{

    //    photonView.RPC("RPCCountDown", RpcTarget.All);
    //    music = SoundMgr.Instance.inGameMusic;
    //}


    IEnumerator CountDown()
    { 
        count = maxCount;
        while (true)
        {
            yield return null;
            if (count >= 0)
            {
                count -= Time.deltaTime;

                countTxt.text = count.ToString("F0");
            }
            else
            {
                countTxt.gameObject.SetActive(false);
                gameTimerText.gameObject.SetActive(true);
                if (PhotonNetwork.IsMasterClient)
                {
                    RandomPowers();
                    CountingPlayerPowers();
                    connectionCrystalPosition.StartGame();
                }
                StartCoroutine(GameMgr.Instance.ExecutePlayerCountAction());
                StartCoroutine(CoTimer());
                music.gameObject.SetActive(false);
                break;

            }
        }

    }

    IEnumerator CoTimer()
    {
        while(true)
        {
            if (minutes <= 0 && seconds <= 0)
                break;
            seconds--;
            if(seconds < 0)
            {
                minutes--;
                seconds = 59;
            }
            string secondsString = seconds.ToString("D2");
            gameTimerText.text = minutes.ToString() + ":" + secondsString;
            yield return new WaitForSeconds(1);
            
        }
        if (taggerWin)
        {
            ClearMgr.Instance.win = false;
        }
        else
        {
            ClearMgr.Instance.win = true;
        }

        if (PhotonNetwork.IsMasterClient)
            GameMgr.Instance.MoveClearScenes();
    }

    //private void FindAllPlayers()
    //{
    //    players.Clear();
    //    Player[] foundPlayers = FindObjectsOfType<Player>();
    //    players.AddRange(foundPlayers);
    //}
    public void RandomPowers()
    {
        for (int i = powers.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            string changePower = powers[i];
            powers[i] = powers[randomIndex];
            powers[randomIndex] = changePower;
        }
    }

    [PunRPC]
    public void RPCCountDown()
    {
        Debug.Log("Log");
        StartCoroutine(CountDown());
    }
    public void CountingPlayerPowers()
    {
        Player[] players = FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++)
        {
            if (i < powers.Count)
            {
                players[i].SetPower(powers[i]);
            }
        }
    }
}
