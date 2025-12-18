using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si lo que tocó los pinchos es el Jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // Llamamos directamente a tu corrutina de muerte
                player.MorirPorPinchos();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lo mismo, por si tienes los pinchos configurados como "Is Trigger"
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.MorirPorPinchos();
            }
        }
    }
}