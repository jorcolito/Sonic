using UnityEngine;
using System.Collections;

public class SpawnerDeBalas : MonoBehaviour
{
    [Header("Tus Prefabs Nuevos")]
    public GameObject[] bulletPrefabs; // Arrastra aquí los prefabs que tengan 'BalaTemporal'

    [Header("Configuración")]
    public float minTime = 0.5f;
    public float maxTime = 1.5f;

    [Tooltip("Dirección de la bala: X=-1 (Izq), X=1 (Der), Y=-1 (Abajo)")]
    public Vector2 direction = Vector2.left; 

    void Start()
    {
        StartCoroutine(SpawnCoroutine(0));
    }

    IEnumerator SpawnCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (bulletPrefabs.Length > 0)
        {
            // 1. Crear la bala
            GameObject newBullet = Instantiate(
                bulletPrefabs[Random.Range(0, bulletPrefabs.Length)],
                transform.position,
                Quaternion.identity
            );

            // 2. Buscar el script NUEVO y pasarle la dirección del Spawner
            BalaTemporal balaScript = newBullet.GetComponent<BalaTemporal>();
            if (balaScript != null)
            {
                balaScript.direction = this.direction; // Aquí conectamos el movimiento
            }
        }

        // 3. Siguiente disparo
        StartCoroutine(SpawnCoroutine(Random.Range(minTime, maxTime)));
    }
    
    // Dejar Update vacío o borrarlo si no se usa
    void Update() { }
}