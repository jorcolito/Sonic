using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Data")]
    public int totalRings = 0; 
    private float timer = 0f;

    [Header("HUD References")]
    public SpriteNumberDisplay ringDisplay; 
    public SpriteNumberDisplay timeMinutesDisplay; 
    public SpriteNumberDisplay timeSecondsDisplay;
    public SpriteNumberDisplay timeMillisecondsDisplay;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start() 
    {
        UpdateRingUI(0); // Inicia el HUD en 000
    }

    void Update()
    {
        timer += Time.deltaTime; // Suma el tiempo real
        UpdateTimerUI(timer);    // Actualiza los sprites del tiempo
    }

    public void AddRing(int amount)
    {
        totalRings += amount;
        UpdateRingUI(totalRings); 
    }

    public void UpdateRingUI(int ringCount)
    {
        if (ringDisplay != null)
        {
            ringDisplay.SetNumber(ringCount);
        }
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