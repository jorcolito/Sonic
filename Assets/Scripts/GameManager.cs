using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Data")]
    public int totalRings = 0; 
    public int totalLives = 3; // Empezamos con 3 vidas
    private float timer = 0f;
    private bool isGameOver = false;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip ringSound;
    public AudioClip jumpSound;
    public AudioClip deathSound; // Por si tienes sonido de muerte

    [Header("HUD References")]
    public SpriteNumberDisplay ringDisplay; 
    public SpriteNumberDisplay livesDisplay; // NUEVO: Arrastra el display de vidas aquí
    public SpriteNumberDisplay timeMinutesDisplay; 
    public SpriteNumberDisplay timeSecondsDisplay;
    public SpriteNumberDisplay timeMillisecondsDisplay;

    [Header("GameOver UI")]
    public GameObject gameOverPanel; // Arrastra tu Panel de Game Over aquí

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start() 
    {
        UpdateRingUI(totalRings); 
        UpdateLivesUI(totalLives);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver)
        {
            timer += Time.deltaTime; 
            UpdateTimerUI(timer);    
        }
    }

    // --- LÓGICA DE VIDAS ---

    public void LoseLife()
    {
        totalLives--;
        UpdateLivesUI(totalLives);

        if (totalLives <= 0)
        {
            TriggerGameOver();
        }
        else
        {
            // Reinicia la escena después de un pequeño delay
            Invoke("RestartLevel", 1.5f);
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        if (musicSource != null) musicSource.Stop();
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        
        // Opcional: Congelar el tiempo
        // Time.timeScale = 0f; 
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Método para el botón de "Retry" del Panel de Game Over
    public void ResetGame()
    {
        Time.timeScale = 1f;
        totalLives = 3;
        totalRings = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- MÉTODOS DE SONIDO ---

    public void PlayRingSound() { if (sfxSource && ringSound) sfxSource.PlayOneShot(ringSound); }
    public void PlayJumpSound() { if (sfxSource && jumpSound) sfxSource.PlayOneShot(jumpSound); }

    // --- MÉTODOS DE UI ---

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

    public void UpdateRingUI(int ringCount)
    {
        if (ringDisplay != null) ringDisplay.SetNumber(ringCount);
    }

    public void UpdateTimerUI(float currentTime)
    {
        if (timeMinutesDisplay == null || timeSecondsDisplay == null || timeMillisecondsDisplay == null)
            return;

        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        int milliseconds = (int)((currentTime * 100) % 100);

        timeMinutesDisplay.SetNumber(minutes); 
        timeSecondsDisplay.SetNumber(seconds); 
        timeMillisecondsDisplay.SetNumber(milliseconds);
    }
}