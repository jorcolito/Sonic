using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    public float jumpForce = 20f;

    [Header("Sprites")]
    public Sprite baseSprite;
    public Sprite[] runSprites;
    public Sprite[] jumpSprites;
    public Sprite[] maxSpeedSprites;

    [Header("Idle prolongado")]
    public float idleTimeToAnimate = 4f;
    public Sprite[] longIdleSprites;

    [Header("Ajustes base")]
    public float speedThresholdOffset = 3f;
    public float jumpBoost = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float currentSpeed = 0f;
    private int spriteIndex = 0;

    private bool isGrounded = true;
    private bool facingRight = true;

    private float idleTimer = 0f;
    private bool inLongIdle = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = baseSprite;
        StartCoroutine(Animate());
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleFlip();
        HandleIdleTimer();
    }

    void HandleMovement()
    {
        float input = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
            input = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow))
            input = -1f;

        float targetSpeed = input * maxSpeed;

        if (Mathf.Abs(input) > 0)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                targetSpeed,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0,
                deceleration * Time.deltaTime
            );
        }

        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            float speedAbs = Mathf.Abs(currentSpeed);
            float jump = jumpForce;

            if (speedAbs >= maxSpeed - speedThresholdOffset)
                jump += jumpBoost;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            isGrounded = false;
        }
    }

    void HandleIdleTimer()
    {
        if (Mathf.Abs(currentSpeed) < 0.1f && isGrounded)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTimeToAnimate)
                inLongIdle = true;
        }
        else
        {
            idleTimer = 0f;
            inLongIdle = false;
        }
    }

    void HandleFlip()
    {
        if (currentSpeed > 0 && !facingRight)
            Flip();
        else if (currentSpeed < 0 && facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator Animate()
    {
        while (true)
        {
            float speedAbs = Mathf.Abs(currentSpeed);
            float speedThreshold = maxSpeed - speedThresholdOffset;

            // Idle anim
            if (inLongIdle && longIdleSprites.Length > 0)
            {
                sr.sprite = longIdleSprites[spriteIndex % longIdleSprites.Length];
                spriteIndex++;
            }
            // Salto
            else if (!isGrounded && jumpSprites.Length > 0)
            {
                sr.sprite = jumpSprites[spriteIndex % jumpSprites.Length];
                spriteIndex++;
            }
            // Velocidad alta
            else if (speedAbs >= speedThreshold && maxSpeedSprites.Length > 0)
            {
                sr.sprite = maxSpeedSprites[spriteIndex % maxSpeedSprites.Length];
                spriteIndex++;
            }
            // Velocidad normal
            else if (speedAbs > 0.1f && runSprites.Length > 0)
            {
                sr.sprite = runSprites[spriteIndex % runSprites.Length];
                spriteIndex++;
            }
            // Idle
            else
            {
                sr.sprite = baseSprite;
                spriteIndex = 0;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }
}

