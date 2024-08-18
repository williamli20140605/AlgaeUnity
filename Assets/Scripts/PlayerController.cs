using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public variables
    StartManager startManager;
    CheatManager cheatManager;
    public float speed = 10f;                // Speed of the player movement
    public GameObject bulletPrefab;          // Prefab of the bullet GameObject
    public Transform bulletSpawnPoint;       // Transform where bullets will spawn
    public float shootCooldown = 0.2f;       // Cooldown between shots
    public float spaceButtonOffset = 1f;     // Offset to the left of the player where space button is located

    // Private variables
    private float lastShootTime;             // Timestamp of the last shot
    private Vector3 mousePosition;           // Mouse position in world space
    private bool isMouseHeld = false;        // Flag to check if mouse is held down

    // Screen boundaries
    private float screenHalfWidth;

    void Start()
    {
        // Calculate half width of the screen in world units
        float halfPlayerWidth = transform.localScale.x / 2f;
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize - halfPlayerWidth;
        cheatManager = GameObject.Find("CheatManager").GetComponent<CheatManager>();
        startManager = GameObject.Find("StartManager").GetComponent<StartManager>();
    }

    void Update()
    {
        if (startManager.isStarted == false || startManager.isShowingPhases)
        {
            return;
        }

        if (cheatManager.isCheating)
        {
            shootCooldown = 0.0025f;
        }
        else if (!startManager.infiniteMode)
        {
            shootCooldown = 0.2f;
        }

        // Handle mouse input
        if (Input.GetMouseButton(0)) // Mouse button is held down
        {
            isMouseHeld = true;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z; // Keep the z-position the same
        }
        else
        {
            isMouseHeld = false;
        }

        // Movement input
        float horizontalInput = Input.GetAxis("Horizontal");

        if (isMouseHeld)
        {
            // Move towards the mouse position
            Vector3 direction = (mousePosition - transform.position).normalized;
            direction.y = 0; // Ensure movement is horizontal only
            Vector3 targetPosition = transform.position + direction * speed * Time.deltaTime;

            // Clamp the player's X position within screen boundaries
            float clampedX = Mathf.Clamp(targetPosition.x, -screenHalfWidth, screenHalfWidth);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            Shoot();
        }
        else
        {
            // Calculate the position where the player would move to
            Vector3 targetPosition = transform.position + Vector3.right * horizontalInput * speed * Time.deltaTime;

            // Clamp player's X position within screen boundaries
            float clampedX = Mathf.Clamp(targetPosition.x, -screenHalfWidth, screenHalfWidth);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }

        // Shooting input
        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time - lastShootTime > shootCooldown)
        {
            // Instantiate a bullet prefab
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            lastShootTime = Time.time;  // Update last shoot time
        }
    }
}
