using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private float zDistance = -10f;
    [SerializeField] private float smoothSpeed = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player reference not set and could not find object tagged as Player.");
                enabled = false;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // Get current camera position
        Vector3 currentPosition = transform.position;
        
        // Calculate target position, keeping the same Z distance
        Vector3 targetPosition = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            zDistance
        );
        
        // Smoothly move camera to target position
        Vector3 smoothedPosition = Vector3.Lerp(currentPosition, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
