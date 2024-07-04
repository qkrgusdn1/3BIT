using Photon.Pun;
using UnityEngine;

public class HammerMan : Player
{
    public Weapon weapon;
    PlayerAnimationEventHandler animationEventHandler;

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
        Debug.Log("HammerMan StartAttack");
        weapon.StartAttack();
    }

    public void EndAttack()
    {
        Debug.Log("HammerMan EndAttack");
        weapon.EndAttack();
    }

    public void FinishAttack()
    {
        Debug.Log("HammerMan FinishAttack");
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

    public override void Attack(Player target, float damage)
    {
        base.Attack(target, damage);
        
        Debug.Log("HammerMan Attack1");
        if (target.CompareTag("Tagger"))
        {
            Debug.Log("HammerMan Attack2");
            if(!target.stun)
                target.photonView.RPC("RPCApplySkill", Photon.Pun.RpcTarget.All, Skill.Back);
        }
    }
    [PunRPC]
    public void RpcAttack()
    {
        attackSound.Play();
        animator.SetLayerWeight(upperLayer, 1);
        animator.Play("Attack");
    }

    [PunRPC]
    public void RpcFinishAttack()
    {
        animator.SetLayerWeight(upperLayer, 0);
    }


}
