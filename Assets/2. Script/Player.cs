using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks
{
    public Transform bodyTr;
    public Transform shootTr;
    public float horSensitivity;
    public float vertSensitivity;
    public float rotationY;
    public float rotationX;
    public Rigidbody rb;

    public bool stun;

    public AudioSource attackSound;

    public float moveSpeed;
    public float maxMoveSpeed;

    protected float jumpForce;
    public float maxJumpForce;
    protected bool isJumping;
    public bool isGrounded;

    public bool esc;
    public bool mission;

    public LayerMask groundLayer;

    protected Animator animator;
    protected Vector3 dir;

    public float hp;
    public float maxHp;
    public GameObject hpBarCanvas;
    public Image hpBar;
    public Image hpBarMine;

    public Image hpBarBgMine;

    public Skill currentSkill;
    public float stunTimer;
    public float maxStunTimer;
    Coroutine moveBackCoroutine;
    public bool back;

    public GameObject canvas;
    public GameObject allSkillTime;

    public float skillTimer;
    public float maxSkillTimer;
    public Image skillTime;

    public bool startPlayer;

    public bool die;

    public bool threePlayer;

    public TMP_Text skillTimerText;
    bool isMoving;
    public virtual void Start()
    {
        if (!photonView.IsMine)
        {
            canvas.SetActive(false);
            hpBarCanvas.SetActive(true);
            rb.isKinematic = true;
        }
        else
        {
            hpBarCanvas.SetActive(false);
            photonView.RPC("RPCAddPlayerList", RpcTarget.All);
            GameMgr.Instance.AddPlayer();
            if (!startPlayer)
            {
                canvas.SetActive(true);
                
            }
            rb.isKinematic = false;

        }
        animator = GetComponentInChildren<Animator>();
        if (photonView.IsMine)
        {
            CameraMgr.Instance.SetTarget(this);
        }



        skillTimer = maxSkillTimer;
        skillTime.fillAmount = 1;
        
    }
    public virtual void Awake()
    {
        stunTimer = maxStunTimer;
        currentSkill = Skill.Default;
        moveSpeed = maxMoveSpeed;
        jumpForce = maxJumpForce;
        hp = maxHp;
        hpBar.fillAmount = 1;
        hpBarMine.fillAmount = 1;
    }

    public virtual void Update()
    {
        dir = new Vector3(0, 0, 0);
        if (PhotonNetwork.InRoom)
        {
            if (photonView == null)
                return;

            if (!photonView.IsMine)
                return;

            if (die)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                animator.SetBool("IsRunning", false);
                return;
            }

            if (!canvas.activeSelf && !startPlayer)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                animator.SetBool("IsRunning", false);
                return;
            }

        }

        if (esc || mission)
        {
            animator.SetBool("IsRunning", false);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }
            

        if (Physics.Raycast(bodyTr.position, Vector3.down, 0.1f, groundLayer))
        {
            isGrounded = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            isJumping = true;
            photonView.RPC("RpcJump", RpcTarget.All);
        }
        Die();


        Rotation();
        Move();
    }

    public void Die()
    {
        if (hp <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameMgr.Instance.diePanel.SetActive(true);
            GameMgr.Instance.players.Remove(this);

            
            if (gameObject.CompareTag("Tagger"))
            {
                photonView.RPC("RpcDIe", RpcTarget.All);
                GameMgr.Instance.MoveClearScenes();
                return;
            }
            photonView.RPC("RpcDIe", RpcTarget.All);
        }
    }

    public virtual void Attack(Player target, float damage)
    {
        if (!photonView.IsMine)
            return;
        target.TakeDamage(damage);
    }

    [PunRPC]
    public void RPCAddPlayerList()
    {
        GameMgr.Instance.players.Add(this);
    }

    IEnumerator MoveBackCoroutine()
    {
        float timer = 1;
        while (true)
        {
            if (timer < 0)
                break;
            yield return null;
            rb.velocity = -bodyTr.forward * maxMoveSpeed * 3;
            timer -= Time.deltaTime;
        }
        back = false;
        currentSkill = Skill.Default;
    }

    [PunRPC]
    public void RPCApplySkill(Skill skill)
    {
        currentSkill = skill;
        if (currentSkill == Skill.Default)
        {
            animator.SetTrigger("EndStun");
            
        }
        else if (currentSkill == Skill.Stun && !back)
        {
            animator.Play("Stun");
            stun = true;
        }
        else if (currentSkill == Skill.Back && !stun)
        {
            back = true;
            moveBackCoroutine = StartCoroutine(MoveBackCoroutine());
        }
    }



    public virtual void TakeDamage(float damage)
    {
        Debug.Log("ss");
        
        photonView.RPC("RPCTakeDamage", RpcTarget.All, damage);
        //if(photonView.IsMine)
        //    photonView.RPC("RpcTakeDamage", RpcTarget.All);

    }

    public void Rotation()
    {
        rotationY += Input.GetAxis("Mouse X") * horSensitivity * SliderControl.sensitivityValue;
        rotationX -= Input.GetAxis("Mouse Y") * vertSensitivity * SliderControl.sensitivityValue;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        bodyTr.localEulerAngles = new Vector3(0, rotationY, 0);
        //CameraMgr.Instance.transform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }

    public void SetPower(string power)
    {
        photonView.RPC("RPCSetPower", RpcTarget.All, power);
    }
    [PunRPC]
    public void RPCSetPower(string power)
    {
        GameMgr.Instance.players.Remove(this);
        if (photonView.IsMine == false)
            return;
        
        if (power == "LightningMan")
        {
            GameObject lightningMan = PhotonNetwork.Instantiate(power, transform.position, transform.rotation);
        }
        else if(power == "Tagger")
        {
            GameObject tagger = PhotonNetwork.Instantiate(power, transform.position, transform.rotation);
        }
        else if(power == "HammerMan")
        {
            GameObject hammerMan = PhotonNetwork.Instantiate(power, transform.position, transform.rotation);
        }
        else if (power == "Speeder")
        {
            GameObject speeder = PhotonNetwork.Instantiate(power, transform.position, transform.rotation);
        }

        PhotonNetwork.Destroy(gameObject);
    }
    void Move()
    {

        isMoving = false;
        if (Input.GetKey(KeyCode.W))
        {
            dir += bodyTr.forward * moveSpeed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir += -bodyTr.right * moveSpeed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += bodyTr.right * moveSpeed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += -bodyTr.forward * moveSpeed;
            isMoving = true;
        }
        if (!isMoving && !isJumping)
        {
            animator.SetBool("IsRunning", false);
        }
        else if(isMoving && !isJumping)
        {
            animator.SetBool("IsRunning", true);
        }
        rb.velocity = new Vector3(dir.x, rb.velocity.y, dir.z);

    }

    [PunRPC]
    public void RpcDIe()
    {
        if (gameObject.CompareTag("Tagger"))
        {
            ClearMgr.Instance.win = true;
            return;
        }
        
        Destroy(gameObject);
    }

    //[PunRPC]
    //public void RpcMove(bool move)
    //{
    //    if (animator == null) 
    //        return;
    //    if (!move && !isJumping)
    //    {
    //        animator.CrossFade("Idle", 0, 0);
    //    }
    //    else if (move && !isJumping)
    //    {
    //        animator.CrossFade("Run", 0, 0);
    //    }
    //}

    

    

    [PunRPC]
    public void RPCTakeDamage(float damage)
    {
        hp -= damage;
        hpBar.fillAmount = hp / maxHp;
        if (photonView.IsMine)
        {
            hpBarMine.fillAmount = hp / maxHp;
        }

    }

    public virtual void Shoot()
    {
        
    }
    [PunRPC]
    public virtual void RpcShoot(Vector3 direction, int shooterID)
    {
        
    }
    [PunRPC]
    public void RpcJump()
    {
        animator.Play("Jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
public enum Skill
{
    Default,
    Stun,
    Back
}
