using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeeHpBar : MonoBehaviour
{
    public Transform target;
    bool targetNull = true;

    private void Start()
    {
        target = GameMgr.Instance.player.transform;
    }

    void Update()
    {
        if (targetNull && target != null)
        {
            target = GameMgr.Instance.player.transform;
            targetNull = false;
        }
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(-direction);
            transform.rotation = rotation;
        }
    }
}
