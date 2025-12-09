using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float salto = 20f;
    public float velocidad = 5f;
    public Sprite[] mySprites;
    private int index = 0;

    private Rigidbody2D myrigidbody2D;
    private SpriteRenderer mySprinteRenderer;

    void Start()
    {
        myrigidbody2D = GetComponent<Rigidbody2D>();
        mySprinteRenderer = GetComponent<SpriteRenderer>();

        // StartCoroutine(WalkCoRutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myrigidbody2D.linearVelocity = new Vector2(myrigidbody2D.linearVelocity.x, salto);
        }
            /*
                Si se aplasta espacio:
                en X: la velocidad no cambia, por eso linearvelocity.x
                en Y: se aplica la fuerza de salto
            */

    }

    // un IEnumerator es como un void pero con pausas (2, 3, 4 Seg)
}
