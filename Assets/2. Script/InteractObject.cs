using Photon.Pun;
using System.Collections;
using UnityEngine;
using System.Linq;

public class InteractObject : MonoBehaviourPunCallbacks
{
    public GameObject text;
    public float range;
    public bool enterd;
    public bool watched;
    public float findAngle;
    public float angle;

    public LayerMask runnerLayer;
    public LayerMask taggerLayer;

    Collider[] runnersInRange;
    public virtual void Start()
    {
        text.SetActive(false);
        StartCoroutine(CoUpdate());
    }


    public void Range()
    {
        if (GameMgr.Instance == null)
            return;
        if (GameMgr.Instance.player == null)
            return;

        runnersInRange = Physics.OverlapSphere(transform.position, range, runnerLayer);

        if (!runnersInRange.Contains(GameMgr.Instance.player.GetComponent<Collider>()))
        {
            enterd = false;
            return;
        }

        if (runnersInRange.Length <= 0)
        {
            enterd = false;
        }
        else
        {
            enterd = true;
        }

    }

    public virtual IEnumerator CoUpdate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            Range();
            CheckLookAt();
            if (!GameMgr.Instance.player.gameObject.CompareTag("Tagger"))
            {
                if (enterd && watched)
                {
                    text.SetActive(true);
                }
                else
                {
                    text.SetActive(false);
                }
            }

        }
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void CheckLookAt()
    {
        if (!enterd)
        {
            watched = false;
            return;
        }


        Vector3 dir = transform.position - GameMgr.Instance.player.transform.position;
        dir.y = 0;
        dir = dir.normalized;
        angle = Vector3.Angle(GameMgr.Instance.player.GetComponent<Player>().bodyTr.forward, dir);


        watched = angle <= findAngle;
    }
}
