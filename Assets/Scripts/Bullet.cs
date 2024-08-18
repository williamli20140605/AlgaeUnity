using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f; // Adjust this as needed

    private BossHealth bossHealth;  // Reference to BossHealth script

    StartManager startManager;

    ScoreManager scoreManager;

    void Start()
    {
        startManager = GameObject.Find("StartManager").GetComponent<StartManager>();
        scoreManager = GameObject.Find("Score").GetComponent<ScoreManager>();
    }
    void Update()
    {
        if(startManager.isStarted == false)
        {
            return;
        }
        // Move the bullet upwards continuously
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Check if the bullet has gone outside the screen bounds
        if (!IsVisible())
        {
            Destroy(gameObject); // Destroy the bullet
        }
    }

    bool IsVisible()
    {
        Vector3 positionInViewport = Camera.main.WorldToViewportPoint(transform.position);
        return positionInViewport.y > 0 && positionInViewport.y < 1; // Only destroy if completely outside top of screen
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ScoreManager.instance.AddScore(1);
            Destroy(collision.gameObject); // Destroy the enemy
            Destroy(gameObject); // Destroy the bullet
        }
        if (collision.CompareTag("Boss"))
        {
            ScoreManager.instance.AddScore(1);
            bossHealth = collision.gameObject.GetComponent<BossHealth>();
            BossMovement bossMovement = collision.GetComponent<BossMovement>();
            DealDamage(1);
            if (bossHealth.isBossAlive == false)
            {
                startManager.gameOverType = StartManager.GameOverType.Win;
                startManager.OnGameOver();
            }

            // Destroy the bullet upon hitting the boss
            Destroy(gameObject);
        }
    }

    public void DealDamage(int damageAmount)
    {
        bossHealth.TakeDamage(damageAmount);
    }
}
