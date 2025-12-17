using UnityEngine;

public class Sonic1Parallax : MonoBehaviour
{
    public Transform cam;
    [Range(0f, 1f)]
    public float parallaxSpeedX = 0.5f; // Cuánto se mueve horizontalmente

    private float fixedY; // Aquí guardaremos la altura inicial
    private float startZ;

    void Start()
    {
        if (cam == null) cam = Camera.main.transform;
        
        // Guardamos la posición Y inicial para que NUNCA cambie
        fixedY = transform.position.y;
        startZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // X: Se mueve proporcionalmente a la cámara
        float targetX = cam.position.x * (1 - parallaxSpeedX);

        // Y: SE QUEDA ESTÁTICO (usa el valor que guardamos al inicio)
        float targetY = fixedY;

        transform.position = new Vector3(targetX, targetY, startZ);
    }
}