using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 150; 

    private Vector3 targetPosition; 
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isMoving && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W))
                direction = Vector3.up;
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = Vector3.down;
                
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                direction = Vector3.left;
                spriteRenderer.flipX = true;
            }               
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction = Vector3.right;
                spriteRenderer.flipX = false;
            }

            targetPosition = transform.position + (direction * 2);

            StartCoroutine(MoveToPosition(targetPosition));
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Base"))
        {
            rb.velocity = Vector2.zero;
        }
    }
    private IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(target, new Vector2(1f, 1f), 0f); 

        bool canMoveToPosition = true;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Base"))
            {
                canMoveToPosition = false;
                break;
            }
        }

        if (canMoveToPosition)
        {
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        transform.position = target;
        isMoving = false;
    }
}
