using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    public SpriteRenderer backgroundSprite; // Reference to the background sprite
    public CameraFollow cameraFollow; // Reference to the background sprite

    private Vector3 targetPosition; // Target position to move to
    private bool isMoving = false; // Tracks if the player is moving

    private GameManager gameManager; // Reference to the GameManager
    public AudioSource audioSource;
    public AudioClip gameoverSfx;
    public AudioClip tapSfx;

    public GameObject spawnSpritePrefab; // Prefab of the sprite to spawn

    private void Start()
    {
        // Get the GameManager reference
        gameManager = FindObjectOfType<GameManager>();

        // Set the initial position as the target position
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect tap/click
        {
            // Get the tap position in world space
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0; // Ensure Z remains 0 for 2D

            // Check if the position is within the bounds of the background
            if (IsInsideBackground(touchPosition))
            {
                // Set the target position
                targetPosition = touchPosition;
                isMoving = true;
                audioSource.PlayOneShot(tapSfx);
                // Spawn the sprite at the tap position
                SpawnAndDestroySprite(touchPosition);
            }
        }

        // Move the player towards the target
        if (isMoving)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Stop moving when the target is reached
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
        }
    }

    private bool IsInsideBackground(Vector3 position)
    {
        // Get the bounds of the background sprite
        Bounds bgBounds = backgroundSprite.bounds;

        // Check if the position is within the bounds
        return bgBounds.Contains(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision with "obs" tag
        if (collision.gameObject.CompareTag("obs"))
        {
            audioSource.PlayOneShot(gameoverSfx);
            // cameraFollow.EndGame();
            gameManager.GameOver(); // Trigger Game Over in the GameManager

        }
    }

    private void SpawnAndDestroySprite(Vector3 position)
    {
        // Instantiate the sprite prefab at the given position
        GameObject spawnedSprite = Instantiate(spawnSpritePrefab, position, Quaternion.identity);

        // Destroy the sprite after 1 second
        Destroy(spawnedSprite, 0.2f);
    }
}
