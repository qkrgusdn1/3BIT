using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightningMan : Player
{
    public Lightning lightningPrefab;
    public float atkRange;


    public override void Start()
    {
        base.Start();
    }

    public override void Awake()
    {
        base.Awake();
    }
    public override void Shoot()
    {
        int shooterID = photonView.ViewID;

        if(skillTimer >= maxSkillTimer)
        {
            attackSound.volume = SoundMgr.Instance.attackSoundVolume;
            photonView.RPC("RpcShoot", RpcTarget.All, shootTr.forward, shooterID);
            skillTimer = 0;
            skillTime.fillAmount = skillTimer / maxSkillTimer;
            skillTimerText.gameObject.SetActive(true);
        }
        
    }
    [PunRPC]
    public override void RpcShoot(Vector3 direction, int shooterID)
    {
        
        attackSound.Play();
        Collider[] cols = Physics.OverlapSphere(transform.position, atkRange);

        foreach (Collider col in cols)
        {
            Player player = col.GetComponent<Player>();
            if (player != null)
            {
                if (col.CompareTag("Tagger"))
                {
                    Vector3 spawnPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z); 
                    Quaternion spawnRotation = player.transform.rotation;
                    Lightning lightning = Instantiate(lightningPrefab, spawnPosition, spawnRotation);
                    lightning.SetShooterID(shooterID);
                }
            }
        }
    }
    public override void Update()
    {
        if (!photonView.IsMine)
            return;
        if (skillTimer <= maxSkillTimer)
        {
            skillTimer += Time.deltaTime;
            skillTime.fillAmount = skillTimer / maxSkillTimer;

            float count = maxSkillTimer - skillTimer;
            skillTimerText.text = count.ToString("F0");
        }
        else
        {
            skillTimerText.gameObject.SetActive(false);
        }
        base.Update();

    }



    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }

}
