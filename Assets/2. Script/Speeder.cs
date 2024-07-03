using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speeder : Player
{
    public SpeedUpRunner speedUpRunnerPrefab;
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
    public override void Shoot()
    {
        int shooterID = photonView.ViewID;

        if (skillTimer >= maxSkillTimer)
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
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;
        SpeedUpRunner speedUpRunner = Instantiate(speedUpRunnerPrefab, currentPosition, currentRotation);
        speedUpRunner.SetShooterID(shooterID);
    }
}
