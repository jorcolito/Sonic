using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    [Header("Configuración de Balas")]
    public GameObject[] bulletPrefabs; // Arrastra aquí tus prefabs de balas (bola de fuego, pincho, etc.)

    [Header("Tiempos de Disparo")]
    public float minTime = 1.0f; // Tiempo mínimo entre disparos
    public float maxTime = 3.0f; // Tiempo máximo entre disparos

    void Start()
    {
        // Inicia la rutina inmediatamente
        StartCoroutine(SpawnCoroutine(0));
    }

    IEnumerator SpawnCoroutine(float waitTime)
    {
        // Espera el tiempo aleatorio
        yield return new WaitForSeconds(waitTime);

        // Si hay balas asignadas, dispara una al azar
        if (bulletPrefabs.Length > 0)
        {
            Instantiate(
                bulletPrefabs[Random.Range(0, bulletPrefabs.Length)],
                transform.position,
                Quaternion.identity
            );
        }

        // Se llama a sí mismo para el siguiente disparo
        StartCoroutine(SpawnCoroutine(Random.Range(minTime, maxTime)));
    }
}