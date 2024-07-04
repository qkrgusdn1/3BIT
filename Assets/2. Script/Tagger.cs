using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tagger : Player
{
    public Weapon weapon;
    PlayerAnimationEventHandler animationEventHandler;
    bool lockPower;
    public int defaultLayer = 0;
    public int upperLayer = 1;
    public override void Start()
    {
        base.Start();
        skillTimerText.gameObject.SetActive(false);
    }

    public override void Awake()
    {
        base.Awake();
        animationEventHandler = GetComponentInChildren<PlayerAnimationEventHandler>();

        animationEventHandler.startAttackListener += StartAttack;
        animationEventHandler.endAttackListener += EndAttack;
        animationEventHandler.finishAttackListener += FinishAttack;
    }

    public void StartAttack()
    {
        weapon.StartAttack();
    }

    public void EndAttack()
    {
        Debug.Log("Tagger EndAttack");
        weapon.EndAttack();
    }

    public void FinishAttack()
    {
        Debug.Log("Tagger FinishAttack");
        photonView.RPC("RpcFinishAttack", RpcTarget.All);
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
        if (currentSkill == Skill.Stun)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                

                stunTimer = maxStunTimer;
                photonView.RPC("RPCSetMove", RpcTarget.All, maxMoveSpeed, maxJumpForce, Vector3.zero);
                photonView.RPC("RPCEndSkill", RpcTarget.All, currentSkill);
            }
            else
            {
                photonView.RPC("RPCSetMove", RpcTarget.All, 0f, 0f, Vector3.zero);
            }
        }
        if (currentSkill == Skill.Default)
        {
            base.Update();
            
            if (Input.GetMouseButtonDown(0))
            {
                if (skillTimer >= maxSkillTimer)
                {
                    attackSound.volume = SoundMgr.Instance.attackSoundVolume;
                    photonView.RPC("RpcAttack", RpcTarget.All);
                    skillTimer = 0;
                    skillTime.fillAmount = skillTimer / maxSkillTimer;
                    skillTimerText.gameObject.SetActive(true);
                }
            }
        }

    }
    [PunRPC]
    public void RPCSetMove(float newMoveSpeed, float newJumpForce, Vector3 newVelocity)
    {
        moveSpeed = newMoveSpeed;
        jumpForce = newJumpForce;
        Vector3 currentVelocity = rb.velocity;
        rb.velocity = new Vector3(newVelocity.x, currentVelocity.y, newVelocity.z);
    }
    public override void TakeDamage(float damage)
    {
        if (MissionMgr.Instance.missionCountBar.fillAmount >= 1)
        {
            base.TakeDamage(damage);
            if (lockPower)
            {
      
                lockPower = false;
            }

        }
    }

    [PunRPC]
    public void RPCEndSkill(Skill skill)
    {
        if(skill == Skill.Stun)
        {
            stun = false;
            animator.SetTrigger("EndStun");
            currentSkill = Skill.Default;
        }
    }

    [PunRPC]
    public void RpcAttack()
    {

        attackSound.Play();
        Debug.Log("Tagger RpcAttack");
        animator.SetLayerWeight(upperLayer, 1);
        animator.Play("Attack");
    }

    [PunRPC]
    public void RpcFinishAttack()
    {
        animator.SetLayerWeight(upperLayer, 0);
    }
}
