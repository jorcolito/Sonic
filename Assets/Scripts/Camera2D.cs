using UnityEngine;

public class Camera2D : MonoBehaviour
{
    public Transform targetPlayer;
    public float fixedY = 0f; // La altura a la que quieres que se quede la cámara

    void LateUpdate()
    {
        if (targetPlayer == null) return;
        
        // Mantenemos X (con el offset de 6), pero dejamos Y fijo en 0
        transform.position = new Vector3(targetPlayer.position.x + 6f, fixedY, -10);
    }
}