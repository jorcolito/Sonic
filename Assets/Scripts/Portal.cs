using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int indiceEscena = 2; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Opcional: Si quieres guardar puntaje o parar el tiempo antes de salir:
            // if (GameManager.Instance != null) {
            //     GameManager.Instance.LevelComplete(); // Tendrías que crear este método
            // }

            SceneManager.LoadScene(indiceEscena);
        }
    }
}