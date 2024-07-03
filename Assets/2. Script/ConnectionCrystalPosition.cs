using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class ConnectionCrystalPosition : MonoBehaviourPunCallbacks
{
    public InteractObject crystalPrefab;
    public List<Transform> crystalPositions = new List<Transform>();
    public const int CRYSTAL_SPAWN_COUNT = 3;

    void Start()
    {
        crystalPositions = GetComponentsInChildren<Transform>().ToList();
        // 첫 번째는 자신의 Transform이므로 제외
        crystalPositions.RemoveAt(0);
        //테스트 할때만
        //StartGame();
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = crystalPositions.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                Transform change = crystalPositions[i];
                crystalPositions[i] = crystalPositions[randomIndex];
                crystalPositions[randomIndex] = change;
            }

            for (int i = 0; i < CRYSTAL_SPAWN_COUNT; i++)
            {
                Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
                GameObject crystal = PhotonNetwork.Instantiate("ConnectionCrystal", crystalPositions[i].position, rotation);
                int crystalViewID = crystal.GetComponent<PhotonView>().ViewID;
                int crystalPositionViewID = crystalPositions[i].GetComponent<PhotonView>().ViewID;
                photonView.RPC("RPCCrystalPositions", RpcTarget.All, crystalViewID, crystalPositionViewID);
            }
        }
    }
    [PunRPC]
    void RPCCrystalPositions(int crystalViewID, int crystalPositionViewID)
    {
        GameObject crystal = PhotonView.Find(crystalViewID).gameObject;
        Transform crystalPosition = PhotonView.Find(crystalPositionViewID).transform;
        crystal.transform.SetParent(crystalPosition);
    }

}
