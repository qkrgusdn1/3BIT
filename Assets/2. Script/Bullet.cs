using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 targetDir;
    public float moveSpeed;
    public float damage;
    private int shooterID;
    Color bulletColor;
    public void Shoot(Vector3 dir)
    {
        targetDir = dir.normalized;
    }
    public void SetShooterID(int id)
    {
        shooterID = id;
    }
    private void Update()
    {
        transform.position = transform.position + targetDir * moveSpeed * Time.deltaTime;
    }
    public void SetColor(Color color)
    {
        bulletColor = color;
        GetComponent<Renderer>().material.color = bulletColor;
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player.photonView.ViewID == shooterID)
        {
            return;
        }

        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }


}
