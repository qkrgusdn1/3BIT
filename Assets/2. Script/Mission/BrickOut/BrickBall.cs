using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BrickBall : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public LayerMask wallLayerMask;
    Vector3 position;
    public int breakBrickCount;

    private void Start()
    {
        position = transform.position;
    }
    private void Update()
    {
        direction = direction.normalized;
        
        RaycastHit2D hit2d = Physics2D.Raycast(transform.position, direction, Time.deltaTime * speed, wallLayerMask);

        if (hit2d)
        {
            if (hit2d.collider.CompareTag("Brick"))
            {
                hit2d.collider.gameObject.SetActive(false);
                breakBrickCount++;
            }
            else if (hit2d.collider.CompareTag("ResetBall"))
            {
                transform.position = position;
            }
            direction = Vector2.Reflect(direction, hit2d.normal);
        }
        else
        {
            transform.position += (Vector3)direction * Time.deltaTime * speed;
        }
    }
}
