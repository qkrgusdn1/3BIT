using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ConnectionCrystal : InteractObject
{
    public CrystalMission crystalMission;

    public bool taggerCome;

    public float taggerRange;

    Collider[] taggerInRange;

    bool isMissionStarted = false;

    public override void Start()
    {
        base.Start();
        MissionMgr.Instance.connectionCrystals.Add(this);
        MissionMgr.Instance.MissionArray();
    }

    public override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, taggerRange);
    }

    public void TaggerRange()
    {
        taggerInRange = Physics.OverlapSphere(transform.position, taggerRange, taggerLayer);

        if (taggerInRange.Length > 0)
        {
            taggerCome = true;
        }
        else
        {
            taggerCome = false;
        }
    }

    public override IEnumerator CoUpdate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            Range();
            TaggerRange();
            CheckLookAt();

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

    private void Update()
    {
        if (GameMgr.Instance.player == null)
            return;
        if (!GameMgr.Instance.player.gameObject.CompareTag("Tagger"))
        {
            if (watched && enterd && Input.GetKeyDown(KeyCode.F))
            {
                Mission mission = MissionMgr.Instance.GetMission(crystalMission, this);
                mission.StartMission();
            }
        }
    }

    IEnumerator ResetMissionFlag()
    {
        yield return new WaitForSeconds(0.5f);
        isMissionStarted = false;
    }
} 

public enum CrystalMission
{
    Pattern,
    BrickOutMission,
    LinkLine
}
