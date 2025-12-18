using UnityEngine;

public class Portal : MonoBehaviour
{
    public int indiceEscena = 2; // El nivel al que vamos después de ganar

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Llamamos a la nueva función del GameManager
            GameManager.Instance.LevelComplete(indiceEscena);
        }
    }
}