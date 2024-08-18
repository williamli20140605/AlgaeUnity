using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;        // Movement speed
    private float stoppingX;        // Random x position to stop at

    StartManager startManager;

    void Start()
    {
        startManager = GameObject.Find("StartManager").GetComponent<StartManager>();
        InitializeMovement();
    }

    public void InitializeMovement()
    {
        stoppingX = Random.Range(-10f, 10f);
    }

    void Update()
    {
        if (startManager.isStarted == false)
        {
            return;
        }
        MoveEnemy();
    }

    void MoveEnemy()
    {
        if (transform.position.y > -5f) // Stop moving when enemy reaches y = -5
        {
            float step = speed * Time.deltaTime;
            Vector3 targetPosition = new Vector3(stoppingX, -5f, 0f);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
        else
        {
            startManager.gameOverType = StartManager.GameOverType.Lose;
            startManager.OnGameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            startManager.gameOverType = StartManager.GameOverType.Lose;
            startManager.OnGameOver();
        }
    }
}
