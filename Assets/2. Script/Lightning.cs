using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float damage;
    private int shooterID;

    public float destroyTime;
    public void SetShooterID(int id)
    {
        shooterID = id;
    }


    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        //if (player != null && player.photonView.ViewID == shooterID)
        //{
        //    return;
        //}

        if (!player.CompareTag("Tagger"))
            return;

        if (player != null)
        {
            player.TakeDamage(damage);
            player.photonView.RPC("RPCApplySkill", Photon.Pun.RpcTarget.All, Skill.Stun);
        }

        Invoke("Destroy", destroyTime);
    }


    void Destroy()
    {
        Destroy(gameObject);
    }
}
