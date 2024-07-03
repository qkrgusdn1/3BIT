using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public bool canDamage;


    public Vector3 size;
    public Transform centerTr;

    public LayerMask hitLayerMask;

    List<GameObject> hittedList = new List<GameObject>();

    Player owner;

    private void Start()
    {
        owner = GetComponentInParent<Player>();
    }

    public void StartAttack()
    {
        hittedList.Clear();
        canDamage = true;
    }

    public void EndAttack()
    {
        canDamage = false;
    }

    public void Update()
    {
        if (!canDamage)
            return;
        Collider[] colliders = Physics.OverlapBox(centerTr.position, size / 2, transform.rotation, hitLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (!hittedList.Contains(colliders[i].gameObject))
            {
                hittedList.Add(colliders[i].gameObject);
                owner.Attack(colliders[i].gameObject.GetComponent<Player>(), damage);
            }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        if (centerTr == null)
            return;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(centerTr.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}
