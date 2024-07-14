using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviourPunCallbacks
{
    public ConnectionCrystal connectionCrystal;
    public CrystalMission crystalMission;
    public Camera missionCamera;
    Color hpBarColor;
    Color hpBarBgColor;
    public SpriteRenderer taggerImage;

    private void Start()
    {
        hpBarColor = GameMgr.Instance.player.hpBarMine.color;
        hpBarBgColor = GameMgr.Instance.player.hpBarBgMine.color;
    }

    public virtual void StartMission()
    {
        GameMgr.Instance.player.mission = true;
        SetAlpha(GameMgr.Instance.player.hpBarMine, 0);
        SetAlpha(GameMgr.Instance.player.hpBarBgMine, 0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraMgr.Instance.gameObject.SetActive(false);
        gameObject.SetActive(true);
        if (GameMgr.Instance.player != null)
        {
            GameMgr.Instance.player.allSkillTime.SetActive(false);
        }
    }
    public virtual void EndMission()
    {
        GameMgr.Instance.player.mission = false;
        SetAlpha(GameMgr.Instance.player.hpBarMine, 1);
        SetAlpha(GameMgr.Instance.player.hpBarBgMine, 1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CameraMgr.Instance.gameObject.SetActive(true);
        if (GameMgr.Instance.player != null)
        {
            GameMgr.Instance.player.allSkillTime.SetActive(true);
        }

        if (MissionMgr.Instance.missionCountBar.fillAmount >= 1)
        {
            photonView.RPC("RpcEndMission", RpcTarget.All);
        }

        gameObject.SetActive(false);
    }

    [PunRPC]
    public void RpcEndMission()
    {
        GameMgr.Instance.connection.gameObject.SetActive(false);

    }

    public virtual void CheckClear()
    {
        Debug.Log("Clear");
        MissionMgr.Instance.UpMissionCount();
        MissionMgr.Instance.missionClearText.gameObject.SetActive(true);
        connectionCrystal.gameObject.SetActive(false);
        EndMission();
    }

    public virtual void Update()
    {
        if (connectionCrystal == null)
            return;

        if (connectionCrystal.taggerCome == true)
        {
            
            taggerImage.gameObject.SetActive(true);
        }
        else
        {
            taggerImage.gameObject.SetActive(false);
        }
    }

    void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
