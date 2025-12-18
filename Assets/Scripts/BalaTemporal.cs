using UnityEngine;

public class BalaTemporal : MonoBehaviour
{
    public float speed = 8f;
    
    // Esta variable define hacia dónde va. El Spawner la modificará.
    public Vector2 direction = Vector2.left; 

    void Start()
    {
        // REQUISITO: "Que se borren después de 5 segundos"
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Moverse en la dirección establecida
        transform.Translate(direction * speed * Time.deltaTime);
    }
}