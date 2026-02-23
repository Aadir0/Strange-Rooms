using UnityEngine;

public class AttractingForce : MonoBehaviour
{
    public float attractionForce = 5f;
    [SerializeField] private float attractionRadius = 100f; // Max distance for attraction
    
    private Transform playerTransform;
    private Rigidbody2D playerRb;
    void Start()
    {
        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null && playerRb != null)
        {
            // Calculate direction from player to this object (the center)
            Vector2 direction = (transform.position - playerTransform.position).normalized;
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            
            // Only apply force if within radius
            if (distance <= attractionRadius)
            {
                // Apply constant force towards the center
                playerRb.AddForce(direction * attractionForce);
            }
        }
    }
    
    // Optional: Visualize the attraction radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
