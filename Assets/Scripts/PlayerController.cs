using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private int ringCount = 0;
    private float startTime;

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

    [Header("Idle anim")]
    public float idleTimeToAnimate = 4f;
    public Sprite[] longIdleSprites;

    [Header("Ajustes base")]
    public float speedThresholdOffset = 3f;
    public float jumpBoost = 3f;

    [Header("Sprites Muerte")]
    public Sprite deathSprite; 

    [Header("Drop Off")]
    public float deathJumpForce = 10f; 
    public float fallSpeed = -20f;     
    public float respawnDelay = 1f;    

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D playerCollider;

    private float currentSpeed = 0f;
    private int spriteIndex = 0;

    private bool isGrounded = true;
    private bool facingRight = true;
    private bool inLongIdle = false;
    private float idleTimer = 0f;

    private bool isDead = false;    
    private bool isJumping = false; 
    private bool inJumpAnimation = false; 

    private Coroutine animateCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();

        sr.sprite = baseSprite;
        animateCoroutine = StartCoroutine(Animate());
        
        startTime = Time.time; 
        
        GameManager.Instance?.UpdateRingUI(ringCount);
    }

    void Update()
    {
        if (!isDead)
        {
            HandleMovement();
            HandleJump();
            HandleFlip();
            HandleIdleTimer();
            GameManager.Instance?.UpdateTimerUI(Time.time - startTime); 
        }
    }

    void HandleMovement()
    {
        float input = 0f;
        if (Input.GetKey(KeyCode.RightArrow)) input = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) input = -1f;

        float targetSpeed = input * maxSpeed;
        if (Mathf.Abs(input) > 0)
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        else
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);

        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            float speedAbs = Mathf.Abs(currentSpeed);
            float jump = jumpForce;
            if (speedAbs >= maxSpeed - speedThresholdOffset) jump += jumpBoost;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            isGrounded = false;
            isJumping = true; 
            GameManager.Instance?.PlayJumpSound();
        }
    }

    void HandleIdleTimer()
    {
        if (Mathf.Abs(currentSpeed) < 0.1f && isGrounded)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTimeToAnimate) inLongIdle = true;
        }
        else
        {
            idleTimer = 0f;
            inLongIdle = false;
        }
    }

    void HandleFlip()
    {
        if (currentSpeed > 0 && !facingRight) Flip();
        else if (currentSpeed < 0 && facingRight) Flip();
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

            if (inLongIdle && longIdleSprites.Length > 0)
            {
                sr.sprite = longIdleSprites[spriteIndex % longIdleSprites.Length];
                spriteIndex++;
                inJumpAnimation = false;
            }
            else if (!isGrounded && jumpSprites.Length > 0)
            {
                sr.sprite = jumpSprites[spriteIndex % jumpSprites.Length];
                spriteIndex++;
                inJumpAnimation = true; 
            }
            else if (speedAbs >= speedThreshold && maxSpeedSprites.Length > 0)
            {
                sr.sprite = maxSpeedSprites[spriteIndex % maxSpeedSprites.Length];
                spriteIndex++;
                inJumpAnimation = false;
            }
            else if (speedAbs > 0.1f && runSprites.Length > 0)
            {
                sr.sprite = runSprites[spriteIndex % runSprites.Length];
                spriteIndex++;
                inJumpAnimation = false;
            }
            else
            {
                sr.sprite = baseSprite;
                spriteIndex = 0;
                inJumpAnimation = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            isJumping = false;
        }

        if (isDead) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (inJumpAnimation)
            {
                Destroy(collision.gameObject);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.5f);
                GameManager.Instance?.PlayJumpSound();
            }
            else StartCoroutine(PlayerDeath());
        }
        else if (collision.gameObject.CompareTag("Enemy2"))
        {
            StartCoroutine(PlayerDeath());
        }
    }

    public void CollectRing()
    {
        ringCount++;
        GameManager.Instance?.UpdateRingUI(ringCount); 
    }

    public void MorirPorPinchos()
    {
        if (!isDead) StartCoroutine(PlayerDeath());
    }

    IEnumerator PlayerDeath()
    {
        isDead = true;
        if (animateCoroutine != null) StopCoroutine(animateCoroutine);
        if (deathSprite != null) sr.sprite = deathSprite;
        if (playerCollider != null) playerCollider.enabled = false;

        // Salto hacia arriba
        rb.linearVelocity = new Vector2(0, deathJumpForce);
        yield return new WaitForSeconds(0.2f);

        // Caída hacia abajo
        rb.linearVelocity = new Vector2(0, fallSpeed);

        // Esperamos el tiempo que definiste para que Sonic salga de la pantalla
        yield return new WaitForSeconds(respawnDelay);

        // Llamamos al GameManager para ver si quedan vidas o es Game Over
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife();
        }
    }

    public void ActivateSpringJump()
{
    // Forzamos los estados para que la corrutina 'Animate' sepa que debe poner el sprite de salto
    isGrounded = false;
    isJumping = true;
}
}