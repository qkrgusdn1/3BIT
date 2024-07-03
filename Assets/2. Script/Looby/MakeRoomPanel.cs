using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakeRoomPanel : MonoBehaviour
{
    public TMP_InputField roomTitleInputField;
    public void OnClickedCreateRoomBtn()
    {
        if (string.IsNullOrEmpty(roomTitleInputField.text))
        {
            Debug.Log("¾Ï°Íµµ ¾È¾¸");
            return;
        }

        PhotonMgr.Instance.CreateRoom(roomTitleInputField.text, 4);
        Debug.Log(roomTitleInputField.text);
    }
}
