using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;

public class SignPost : MonoBehaviour
{
    [Header("Configuración de Destino")]
    [Tooltip("Escribe aquí el nombre EXACTO de la escena a cargar (Ej: 'MarbleZone' o 'MenuPrincipal')")]
    public string nombreEscenaSiguiente = "MenuPrincipal"; 

    [Header("Configuración de Animación")]
    public float spinDuration = 2.0f;
    public float velocidadCorrer = 8f; 

    [Header("Sprites")]
    public Sprite faceRobotnik; 
    public Sprite faceSonic;    
    public Sprite[] spinFrames; 

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
        // 1. Congelar cámara
        if (Camera.main != null)
        {
            Camera.main.transform.parent = null;
            MonoBehaviour[] camScripts = Camera.main.GetComponents<MonoBehaviour>();
            foreach (var script in camScripts)
            {
                if (!script.GetType().Name.Contains("Audio")) script.enabled = false;
            }
        }
        
        // 2. Quitar control
        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
        foreach(var s in scripts) if (s != this) s.enabled = false;

        // 3. Correr
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if(rb != null) rb.linearVelocity = new Vector2(velocidadCorrer, rb.linearVelocity.y);

        // 4. Girar cartel
        float timer = 0f;
        int frameIndex = 0;
        while (timer < spinDuration)
        {
            if(rb != null) rb.linearVelocity = new Vector2(velocidadCorrer, rb.linearVelocity.y);
            if (spinFrames.Length > 0)
            {
                sr.sprite = spinFrames[frameIndex % spinFrames.Length];
                frameIndex++;
            }
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        // 5. Cara de Sonic y Fin
        if(faceSonic != null) sr.sprite = faceSonic;
        yield return new WaitForSeconds(1.5f);
        
        // --- CAMBIO CLAVE: Carga la escena que hayas escrito en el Inspector ---
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }
}