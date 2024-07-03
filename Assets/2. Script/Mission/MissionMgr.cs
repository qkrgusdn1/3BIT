using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionMgr : MonoBehaviourPunCallbacks
{
    public static MissionMgr Instance;
    public Mission[] missions;

    public GameObject allMissionCountBar;

    public Image missionCountBar;

    public TMP_Text missionClearText;

    public GameObject taggerImage;

    public List<ConnectionCrystal> connectionCrystals = new List<ConnectionCrystal>();

    public List<CrystalMission> crystalMissions = new List<CrystalMission>();

    public void Awake()
    {
        Instance = this;
        missionCountBar.fillAmount = 0;
    }

    public void MissionArray()
    {
        if (connectionCrystals.Count == crystalMissions.Count)
        {
            for (int i = 0; i < crystalMissions.Count; i++)
            {
                connectionCrystals[i].crystalMission = crystalMissions[i];
                
            }
        }
        
    }

    public void UpMissionCount()
    {
        photonView.RPC("RPCUpMissionCount", RpcTarget.All);
    }
    [PunRPC]
    public void RPCUpMissionCount()
    {
        if (missionCountBar != null)
        {
            missionCountBar.fillAmount += 1f / 9f;

            if (missionCountBar.fillAmount > 1)
            {
                missionCountBar.fillAmount = 1;
            }
        }
    }

    public Mission GetMission(CrystalMission crystalMission, ConnectionCrystal connectionCrystalGameObj)
    {
        for (int i = 0; i < missions.Length; i++)
        {
            if (missions[i].crystalMission == crystalMission)
            {
                missions[i].connectionCrystal = connectionCrystalGameObj;
                return missions[i];
            }
        }
        return null;
    }
}

