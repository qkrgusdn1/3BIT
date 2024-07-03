using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpRunner : MonoBehaviour
{
    public float destroyTime;
    private int shooterID;
    List<Player> playerRange = new List<Player>();

    public void SetShooterID(int id)
    {
        shooterID = id;
        StartCoroutine(CoDestroy());
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        playerRange.Add(player);
        if (player.CompareTag("Tagger"))
        {
            player.moveSpeed = 1;
        }
        else
        {
            player.moveSpeed = player.maxMoveSpeed * 3;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        player.moveSpeed = player.maxMoveSpeed;
        playerRange.Remove(player);
    }

    private IEnumerator CoDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        for(int i = 0; i < playerRange.Count; i++)
        {
            playerRange[i].moveSpeed = playerRange[i].maxMoveSpeed;
        }
        Destroy(gameObject);
    }
}
