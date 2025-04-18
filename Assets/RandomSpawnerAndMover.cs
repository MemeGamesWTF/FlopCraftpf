using UnityEngine;

public class RandomSpawnerAndMover : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] prefabs; // Array of prefabs to spawn
    public int maxPrefabs = 10; // Maximum number of prefabs
    public Vector2 spawnBounds = new Vector2(2f, 2f); // Spawn area bounds
    public Vector2 moveBounds = new Vector2(2f, 2f); // Movement area bounds
    public float movementSpeed = 1f; // Speed of the prefab movement

    private GameObject[] spawnedPrefabs; // Array to store spawned prefabs
    private Vector2[] directions; // Array to store movement directions of prefabs

    private void Start()
    {
        // Initialize arrays
        spawnedPrefabs = new GameObject[maxPrefabs];
        directions = new Vector2[maxPrefabs];

        // Spawn all prefabs
        SpawnPrefabs();
    }

    private void SpawnPrefabs()
    {
        for (int i = 0; i < maxPrefabs; i++)
        {
            // Randomly select a prefab and position
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            Vector2 randomPosition = new Vector2(
                Random.Range(-spawnBounds.x, spawnBounds.x),
                Random.Range(-spawnBounds.y, spawnBounds.y)
            );

            // Instantiate the prefab
            spawnedPrefabs[i] = Instantiate(prefab, randomPosition, Quaternion.identity);

            // Assign a random direction for movement
            directions[i] = GetRandomDirection();
        }
    }

    private void Update()
    {
        // Move all spawned prefabs
        for (int i = 0; i < spawnedPrefabs.Length; i++)
        {
            if (spawnedPrefabs[i] != null) // Ensure the prefab exists
            {
                MovePrefab(i);
            }
        }
    }

    private void MovePrefab(int index)
    {
        // Move the prefab
        spawnedPrefabs[index].transform.Translate(directions[index] * movementSpeed * Time.deltaTime);

        // Check bounds and reverse direction if needed
        Vector2 position = spawnedPrefabs[index].transform.position;

        if (position.x < -moveBounds.x || position.x > moveBounds.x)
        {
            directions[index].x = -directions[index].x; // Reverse X direction
            position.x = Mathf.Clamp(position.x, -moveBounds.x, moveBounds.x); // Clamp position
        }

        if (position.y < -moveBounds.y || position.y > moveBounds.y)
        {
            directions[index].y = -directions[index].y; // Reverse Y direction
            position.y = Mathf.Clamp(position.y, -moveBounds.y, moveBounds.y); // Clamp position
        }

        // Update the position
        spawnedPrefabs[index].transform.position = position;
    }

    private Vector2 GetRandomDirection()
    {
        // Generate a random normalized direction
        return new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }
}
