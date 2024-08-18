using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float speed = 4.5f;       // Speed of the boss
    public float minX = -8f;          // Minimum x position the boss can move to
    public float maxX = 10f;           // Maximum x position the boss can move to
    public float moveThreshold = 0.1f; // Threshold distance to consider boss arrived at target

    private Vector3 targetPosition;    // Target position the boss is moving towards
    private bool isMoving;             // Flag to track if boss is currently moving
    private float moveTimer;           // Timer to control movement delay

    StartManager startManager;
    CheatManager cheatManager;

    void Start()
    {
        isMoving = false;
        cheatManager = GameObject.Find("CheatManager").GetComponent<CheatManager>();
        startManager = GameObject.Find("StartManager").GetComponent<StartManager>();
        SetNewTargetPosition();
    }

    void Update()
    {
        if(startManager.isStarted == false || startManager.isShowingPhases)
        {
            return;
        }
        if (!isMoving)
        {
            SetNewTargetPosition();
        }
        else
        {
            if (!cheatManager.isCheating)
            {
                // Move towards the target position
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                // Check if boss has reached the target position
                if (Vector3.Distance(transform.position, targetPosition) < moveThreshold)
                {
                    isMoving = false;
                }
            }
        }
    }

    internal void SetNewTargetPosition()
    {
        // Generate a random x position within the specified range
        float randomX = Random.Range(minX, maxX);

        // Set the new target position
        targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);

        // Start moving towards the new target
        isMoving = true;
    }
}
