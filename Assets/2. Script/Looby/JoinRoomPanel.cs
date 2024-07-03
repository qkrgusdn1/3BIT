using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomPanel : MonoBehaviourPunCallbacks
{
    public RoomJoinButton joinRoomJoinButton;

    public RectTransform roomGroupTr;

    public List<RoomJoinButton> buttons = new List<RoomJoinButton>();

    public override void OnEnable()
    {
        base.OnEnable();
        if (PhotonMgr.Instance.roomList != null && PhotonMgr.Instance.roomList.Count > 0)
        {
            ListUpRoom(PhotonMgr.Instance.roomList);
        }
    }
    public void ListUpRoom(List<RoomInfo> roomList)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount <= 0)
                continue;

            RoomJoinButton button = null;

            for (int j = 0; j < buttons.Count; j++)
            {
                if (!buttons[j].gameObject.activeSelf)
                {
                    buttons[j].gameObject.SetActive(true);
                    button = buttons[j];
                    break;
                }
            }
            if(button == null)
            {
                button = Instantiate(joinRoomJoinButton, roomGroupTr);
                buttons.Add(button);
            }
            button.SetRoom(roomList[i]);
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("JoinRoomPanel OnRoomListUpdate");
        ListUpRoom(roomList);
    }
}
