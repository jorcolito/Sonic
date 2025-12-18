using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;

public class SignPost : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite faceRobotnik; 
    public Sprite faceSonic;    
    public Sprite[] spinFrames; 

    [Header("Configuración")]
    public float spinDuration = 2.0f;
    public string nombreEscenaMenu = "MenuPrincipal"; 
    public float velocidadCorrer = 8f; 

    private SpriteRenderer sr;
    private bool isActivated = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null && faceRobotnik != null) sr.sprite = faceRobotnik;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            StartCoroutine(EndGameSequence(collision.gameObject));
        }
    }

    IEnumerator EndGameSequence(GameObject player)
    {
        // --- 1. CONGELAR LA CÁMARA (NUEVO) ---
        if (Camera.main != null)
        {
            // A. Si la cámara es "hija" del jugador, la separamos
            Camera.main.transform.parent = null;

            // B. Buscamos scripts en la cámara (como "CameraFollow") y los apagamos
            MonoBehaviour[] camScripts = Camera.main.GetComponents<MonoBehaviour>();
            foreach (var script in camScripts)
            {
                // Apagamos todos los scripts de la cámara para que deje de moverse
                // (Menos el AudioListener para que no se corte el sonido)
                if (!script.GetType().Name.Contains("Audio"))
                {
                    script.enabled = false;
                }
            }
        }
        
        // --- 2. QUITAR CONTROL A SONIC ---
        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
        foreach(var s in scripts) 
        {
            if (s != this) s.enabled = false;
        }

        // --- 3. HACER QUE CORRA A LA DERECHA ---
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if(rb != null) 
        {
            rb.linearVelocity = new Vector2(velocidadCorrer, rb.linearVelocity.y);
        }

        // --- 4. ANIMACIÓN DEL CARTEL ---
        float timer = 0f;
        int frameIndex = 0;

        while (timer < spinDuration)
        {
            // Mantenemos a Sonic corriendo
            if(rb != null) rb.linearVelocity = new Vector2(velocidadCorrer, rb.linearVelocity.y);

            if (spinFrames.Length > 0)
            {
                sr.sprite = spinFrames[frameIndex % spinFrames.Length];
                frameIndex++;
            }
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        // --- 5. CARA DE SONIC Y FIN ---
        if(faceSonic != null) sr.sprite = faceSonic;

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}