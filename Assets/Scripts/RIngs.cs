using UnityEngine;

public class Ring : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GameManager.Instance.AddRing(1); // Suma 1 al contador
            Destroy(gameObject); // El anillo desaparece
        }
    }
}