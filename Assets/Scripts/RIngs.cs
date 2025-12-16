using UnityEngine;

// Asegúrate de que este script esté adjunto a tu Prefab de Anillo
public class Ring : MonoBehaviour
{
    // Función llamada automáticamente cuando algo entra en el Collider marcado como Is Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // El objeto que entró en el trigger
        GameObject hitObject = other.gameObject;

        // Comprobamos si el objeto que tocó el anillo es el Jugador (Sonic)
        // ASUNCIÓN: Tu personaje principal (Sonic) tiene la etiqueta "Player".
        if (hitObject.CompareTag("Player"))
        {
            // 1. Llama a la función de recolección en el script del jugador.
            // ASUNCIÓN: El script de control de Sonic se llama 'PlayerController'.
            
            PlayerController player = hitObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.CollectRing();
            }

            // 2. Destruye el objeto anillo.
            Destroy(gameObject);
            
            // Opcional: Si tienes efectos de sonido o partículas, actívalos aquí.
        }
    }
}