using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private GameObject background;
    
    private bool gameEnded = false;
    private Vector3 targetPosition;
    private Camera mainCamera;
    private bool shouldFollowPlayer = true;
    private float backgroundWidth;
    private float leftBound;
    private float rightBound;

    private void Start()
    {
        targetPosition = player.position + offset;
        mainCamera = Camera.main;
        
        if (background != null)
        {
            SpriteRenderer bgRenderer = background.GetComponent<SpriteRenderer>();
            if (bgRenderer != null)
            {
                backgroundWidth = bgRenderer.bounds.size.x;
                // Calculate background boundaries
                leftBound = background.transform.position.x - backgroundWidth/2 + mainCamera.orthographicSize * mainCamera.aspect;
                rightBound = background.transform.position.x + backgroundWidth/2 - mainCamera.orthographicSize * mainCamera.aspect;
            }
            else
            {
                Debug.LogError("Background GameObject has no SpriteRenderer component!");
                backgroundWidth = 0f;
            }
        }
        else
        {
            Debug.LogError("Background reference not set in CameraFollow script!");
            backgroundWidth = 0f;
        }
        
        CheckCameraView();
    }

    private void CheckCameraView()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        shouldFollowPlayer = cameraWidth < backgroundWidth;
    }

    private void LateUpdate()
    {
        if (gameEnded)
        {
            targetPosition = centerPosition;
        }
        else if (shouldFollowPlayer)
        {
            targetPosition = new Vector3(
                player.position.x + offset.x,
                transform.position.y,
                transform.position.z
            );
        }
        else
        {
            targetPosition = new Vector3(
                background.transform.position.x,
                transform.position.y,
                transform.position.z
            );
        }

        // Smoothly move the camera
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed
        );

        // Clamp the camera position within background boundaries
        if (shouldFollowPlayer)
        {
            float clampedX = Mathf.Clamp(
                smoothedPosition.x,
                leftBound,
                rightBound
            );
            smoothedPosition = new Vector3(
                clampedX,
                smoothedPosition.y,
                smoothedPosition.z
            );
        }

        transform.position = smoothedPosition;
    }

    public void EndGame()
    {
        gameEnded = true;
    }

    public void SetTarget(Transform newTarget)
    {
        player = newTarget;
        CheckCameraView();
    }

    public void SetCameraCenter(Vector3 newCenter)
    {
        offset = newCenter - player.position;
    }

    public void SetEndPosition(Vector3 endPosition)
    {
        centerPosition = endPosition;
    }

    public void SetBackground(GameObject newBackground)
    {
        background = newBackground;
        SpriteRenderer bgRenderer = background.GetComponent<SpriteRenderer>();
        if (bgRenderer != null)
        {
            backgroundWidth = bgRenderer.bounds.size.x;
            leftBound = background.transform.position.x - backgroundWidth/2 + mainCamera.orthographicSize * mainCamera.aspect;
            rightBound = background.transform.position.x + backgroundWidth/2 - mainCamera.orthographicSize * mainCamera.aspect;
            CheckCameraView();
        }
    }
}