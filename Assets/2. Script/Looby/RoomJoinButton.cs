using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomJoinButton : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text number;
    public RoomInfo roomInfo;
    public void SetRoom(RoomInfo info)
    {
        roomInfo = info;
        number.text = info.PlayerCount.ToString() + "/" + info.MaxPlayers.ToString();
        titleText.text = info.Name;
    }

    public void OnClickedJoinButton()
    {
        PhotonMgr.Instance.roomLodingPanel.SetActive(true);
        PhotonMgr.Instance.JoinRoom(roomInfo);
    }
}
