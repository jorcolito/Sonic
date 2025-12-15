using UnityEngine;

public class ShotController : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public float projectileSpeed = 8f;
    public float projectileLifeTime = 3f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("ShotController: Falta projectilePrefab o firePoint", this);
            return;
        }

        InvokeRepeating(nameof(Shoot), 0f, fireRate);
    }

    void Shoot()
    {
        if (!gameObject.activeInHierarchy) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile p = proj.GetComponent<Projectile>();
        if (p == null)
        {
            Destroy(proj);
            return;
        }

        Vector2 dir = sr != null && sr.flipX ? Vector2.right : Vector2.left;

        p.speed = projectileSpeed;
        p.lifeTime = projectileLifeTime;
        p.SetDirection(dir);
    }

    void OnDestroy()
    {
        CancelInvoke();
    }
}


