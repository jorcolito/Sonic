using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("Sprites")]
    public Sprite[] walkSprites;
    public float animationSpeed = 0.2f;

    private bool movingRight = true;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private int spriteIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (walkSprites.Length > 0)
            StartCoroutine(Animate());
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

            if (transform.position.x >= rightPoint.position.x)
                movingRight = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);

            if (transform.position.x <= leftPoint.position.x)
                movingRight = true;
        }

        sr.flipX = movingRight; 
    }

    IEnumerator Animate()
    {
        while (true)
        {
            sr.sprite = walkSprites[spriteIndex % walkSprites.Length];
            spriteIndex++;
            yield return new WaitForSeconds(animationSpeed);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}



