using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public Transform camara;
    public float textureSizeX; 
    private float yFijaEnElMundo; 

    void Start()
    {
        // Guardamos la altura Y exacta en la que pusiste el fondo en Unity
        yFijaEnElMundo = transform.position.y;

        if (textureSizeX <= 0)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) textureSizeX = sr.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        if (camara == null) return;

        // Movimiento infinito en X (sigue a la cámara)
        float distance = camara.position.x;
        float temp = (camara.position.x % textureSizeX);

        // APLICACIÓN: 
        // X: Se mueve con la cámara y se repite.
        // Y: SE QUEDA EN LA POSICIÓN ORIGINAL DEL MUNDO.
        transform.position = new Vector3(distance - temp, yFijaEnElMundo, transform.position.z);
    }
}