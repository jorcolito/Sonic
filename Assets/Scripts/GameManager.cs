using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Data")]
    public static int totalRings = 0; 
    
    // --- EL CAMBIO MAGICO: 'static' hace que este número no se resetee al reiniciar ---
    public static int totalLives = 3; 

    private float timer = 0f;
    private bool isGameOver = false;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip ringSound;
    public AudioClip jumpSound;
    public AudioClip gameOverMusic; 

    [Header("HUD References")]
    public SpriteNumberDisplay ringDisplay; 
    public SpriteNumberDisplay livesDisplay; 
    public SpriteNumberDisplay timeMinutesDisplay; 
    public SpriteNumberDisplay timeSecondsDisplay;
    public SpriteNumberDisplay timeMillisecondsDisplay;

    [Header("Game Over")]
    public GameObject gameOverContainer; 
    public MonoBehaviour cameraScript;   
    public string menuSceneName = "MenuPrincipal"; 

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start() 
    {
        // Si por error llegamos con 0 vidas (bug), reseteamos a 3
        if (totalLives <= 0) totalLives = 3;

        UpdateRingUI(totalRings); 
        UpdateLivesUI(totalLives);
        
        if (gameOverContainer != null) gameOverContainer.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver)
        {
            timer += Time.deltaTime; 
            UpdateTimerUI(timer);    
        }
    }

    public void LoseLife()
    {
        totalLives--; // Restamos vida
        UpdateLivesUI(totalLives);

        if (totalLives <= 0)
        {
            // SI LLEGAMOS A 0, EJECUTAMOS LA SECUENCIA DE GAME OVER
            StartCoroutine(SequenceGameOver());
        }
        else
        {
            // SI AUN QUEDAN VIDAS, REINICIAMOS NIVEL
            Invoke("RestartLevel", 0.5f);
        }
    }

    IEnumerator SequenceGameOver()
    {
        isGameOver = true;

        // 1. Parar música del nivel
        if (musicSource != null) musicSource.Stop();

        // 2. Congelar la cámara
        if (cameraScript != null) cameraScript.enabled = false;

        // 3. Esperar 1 segundo con la pantalla quieta
        yield return new WaitForSeconds(1f);

        // 4. Mostrar SPRITES GAME OVER y tocar música
        if (gameOverContainer != null) gameOverContainer.SetActive(true);
        if (sfxSource != null && gameOverMusic != null) sfxSource.PlayOneShot(gameOverMusic);

        // 5. Esperar 5 segundos y cargar Menú
        yield return new WaitForSeconds(5f);
        
        // IMPORTANTE: Reseteamos las vidas a 3 para la próxima vez que juegue desde el menú
        totalLives = 3; 
        SceneManager.LoadScene(menuSceneName);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- UI Y SONIDO ---

    public void UpdateLivesUI(int livesCount)
    {
        if (livesDisplay != null) livesDisplay.SetNumber(livesCount);
    }

    public void AddRing(int amount)
    {
        totalRings += amount;
        UpdateRingUI(totalRings); 
        PlayRingSound();
    }

    public void UpdateRingUI(int ringCount) { if (ringDisplay != null) ringDisplay.SetNumber(ringCount); }
    public void PlayRingSound() { if (sfxSource && ringSound) sfxSource.PlayOneShot(ringSound); }
    public void PlayJumpSound() { if (sfxSource && jumpSound) sfxSource.PlayOneShot(jumpSound); }

    public void UpdateTimerUI(float currentTime)
    {
        if (timeMinutesDisplay == null) return;
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        int milliseconds = (int)((currentTime * 100) % 100);
        timeMinutesDisplay.SetNumber(minutes); 
        timeSecondsDisplay.SetNumber(seconds); 
        timeMillisecondsDisplay.SetNumber(milliseconds);
    }
}