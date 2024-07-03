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
        dir = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir += -transform.right * moveSpeed;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            dir += transform.right * moveSpeed;
        }
        rb.velocity = new Vector3(dir.x, rb.velocity.y);
    }
}
