using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("Data")]
    public int totalRings = 0; 

    [Header("HUD References")]
    public SpriteNumberDisplay ringDisplay; 
    public SpriteNumberDisplay timeMinutesDisplay; 
    public SpriteNumberDisplay timeSecondsDisplay;
    public SpriteNumberDisplay timeMillisecondsDisplay;


    void Start() {
        UpdateRingUI(0); // Esto fuerza al HUD a mostrar "000" al iniciar
    }
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
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