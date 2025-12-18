using UnityEngine;
using System.Collections; // Necesario para las corrutinas (la animación)

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))] // Asegura que tenga SpriteRenderer
public class Spring : MonoBehaviour
{
    [Header("Configuración del Salto")]
    public float springForce = 25f;

    [Header("Sprites del Resorte")]
    [Tooltip("El sprite del resorte en reposo (abajo)")]
    public Sprite spriteDown;
    [Tooltip("El sprite del resorte extendido (arriba)")]
    public Sprite spriteUp;
    [Tooltip("Cuánto tiempo se queda extendido antes de bajar")]
    public float extensionTime = 0.3f;

    [Header("Audio")]
    public AudioClip springSound;

    private AudioSource audioSource;
    private SpriteRenderer sr;
    private bool isAnimating = false; // Para evitar que se active doble si lo tocas muy rápido

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        sr = GetComponent<SpriteRenderer>();

        // Asegurarnos de que empiece con el sprite de reposo
        if (spriteDown != null)
        {
            sr.sprite = spriteDown;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAnimating) return; // Si ya se está moviendo, ignoramos

        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            PlayerController playerCtrl = collision.GetComponent<PlayerController>();

            if (playerRb != null && playerCtrl != null)
            {
                LaunchPlayer(playerRb, playerCtrl);
            }
        }
    }

    private void LaunchPlayer(Rigidbody2D rb, PlayerController controller)
    {
        // 1. Impulso físico
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical
        rb.AddForce(Vector2.up * springForce, ForceMode2D.Impulse);

        // 2. Avisar a Sonic para que cambie su animación a salto
        controller.ActivateSpringJump();

        // 3. Sonido
        if (springSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(springSound);
        }

        // 4. Iniciar la animación visual del resorte
        StartCoroutine(AnimateSpringSequence());
    }

    // Esta es la rutina que hace el cambio de sprites
    IEnumerator AnimateSpringSequence()
    {
        isAnimating = true;

        // Cambiar al sprite extendido (ARRIBA)
        if (spriteUp != null) sr.sprite = spriteUp;

        // Esperar un momentito
        yield return new WaitForSeconds(extensionTime);

        // Volver al sprite de reposo (ABAJO)
        if (spriteDown != null) sr.sprite = spriteDown;

        isAnimating = false;
    }
}