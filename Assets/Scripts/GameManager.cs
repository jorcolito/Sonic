using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Data")]
    public int totalRings = 0;
    public static int totalLives = 3;

    private float timer = 0f;
    private bool isGameOver = false;
    private bool isLevelComplete = false; // Nueva variable para saber si ganamos

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip ringSound;
    public AudioClip jumpSound;
    public AudioClip gameOverMusic;
    public AudioClip victoryMusic; // ¡Arrastra aquí tu música de victoria!

    [Header("HUD References (Juego)")]
    public SpriteNumberDisplay ringDisplay;
    public SpriteNumberDisplay livesDisplay;
    public SpriteNumberDisplay timeMinutesDisplay;
    public SpriteNumberDisplay timeSecondsDisplay;
    public SpriteNumberDisplay timeMillisecondsDisplay;

    [Header("Game Over & Victoria")]
    public GameObject gameOverContainer;
    
    // --- NUEVAS REFERENCIAS PARA VICTORIA ---
    public GameObject victoryContainer; // Arrastra aquí tu objeto "PantallaVictoria"
    public SpriteNumberDisplay finalRingsDisplay; // Los números dentro de la pantalla victoria
    public SpriteNumberDisplay finalMinutesDisplay;
    public SpriteNumberDisplay finalSecondsDisplay;

    public MonoBehaviour cameraScript;
    public string menuSceneName = "MenuPrincipal";

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        if (totalLives <= 0) totalLives = 3;
        UpdateRingUI(totalRings);
        UpdateLivesUI(totalLives);
        
        if (gameOverContainer != null) gameOverContainer.SetActive(false);
        if (victoryContainer != null) victoryContainer.SetActive(false); // Aseguramos que empiece apagado
    }

    void Update()
    {
        // Solo contamos tiempo si no es Game Over Y tampoco hemos ganado
        if (!isGameOver && !isLevelComplete)
        {
            timer += Time.deltaTime;
            UpdateTimerUI(timer);
        }
    }

    // --- NUEVO MÉTODO: LLAMADO POR EL PORTAL ---
    public void LevelComplete(int nextSceneIndex)
    {
        if (isLevelComplete) return; // Evitar que pase dos veces
        StartCoroutine(SequenceLevelComplete(nextSceneIndex));
    }

    IEnumerator SequenceLevelComplete(int nextSceneIndex)
    {
        isLevelComplete = true;

        // 1. Tocar música victoria YA
        if (musicSource != null) musicSource.Stop();
        if (sfxSource != null && victoryMusic != null) sfxSource.PlayOneShot(victoryMusic);

        // 2. Mostrar UI de "SONIC HAS PASSED" YA
        if (victoryContainer != null) 
        {
            victoryContainer.SetActive(true);
            
            // Poner los números...
            if(finalRingsDisplay != null) finalRingsDisplay.SetNumber(totalRings);
            // ... (resto de lógica de tiempo igual que antes)
        }

        // 3. AQUÍ es donde esperamos para que el jugador lea sus puntos
        yield return new WaitForSeconds(6f); // Esperar 6 segundos viendo el puntaje

        // 4. Ir al menú (o siguiente nivel)
        SceneManager.LoadScene(nextSceneIndex);
    }

    // ... (El resto de tus métodos LoseLife, Restart, etc. siguen igual abajo) ...
    public void LoseLife()
    {
        totalLives--; 
        UpdateLivesUI(totalLives);

        if (totalLives <= 0) StartCoroutine(SequenceGameOver());
        else Invoke("RestartLevel", 0.5f);
    }

    IEnumerator SequenceGameOver()
    {
        isGameOver = true;
        if (musicSource != null) musicSource.Stop();
        if (cameraScript != null) cameraScript.enabled = false;
        yield return new WaitForSeconds(1f);
        if (gameOverContainer != null) gameOverContainer.SetActive(true);
        if (sfxSource != null && gameOverMusic != null) sfxSource.PlayOneShot(gameOverMusic);
        yield return new WaitForSeconds(5f);
        totalLives = 3; 
        SceneManager.LoadScene(menuSceneName);
    }

    private void RestartLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

    public void UpdateLivesUI(int livesCount) { if (livesDisplay != null) livesDisplay.SetNumber(livesCount); }

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