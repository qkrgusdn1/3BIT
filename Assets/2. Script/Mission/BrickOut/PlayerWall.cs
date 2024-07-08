using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWall : MonoBehaviour
{
    public float moveSpeed;
    Vector3 dir;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        Vector3 dir = new Vector3(horInput * moveSpeed, rb.velocity.y);
        rb.velocity = dir;
    }
}
