using UnityEngine;

public class BrotherMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float stoppingDistance = 1f;
    
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform playerTransform;
    
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool playerDetected = false;
    private bool isPlayerMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Auto-find player if not assigned
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        // Check if player is within detection radius
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        playerDetected = distanceToPlayer <= detectionRadius;

        if (playerDetected)
        {
            // Check if player is moving by accessing PlayerMovement if available
            PlayerMovement playerMovement = playerTransform.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                Rigidbody2D playerRb = playerTransform.GetComponent<Rigidbody2D>();
                isPlayerMoving = playerRb != null && playerRb.linearVelocity.magnitude > 0.1f;
            }

            // Only move if player is moving and we're not too close
            if (isPlayerMoving && distanceToPlayer > stoppingDistance)
            {
                // Calculate direction to player
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                moveDirection = direction;

                // Set animator parameters for blend tree
                anim.SetFloat("inputX", direction.x);
                anim.SetFloat("inputY", direction.y);
                anim.SetBool("moving", true);
            }
            else
            {
                // Stop moving
                moveDirection = Vector2.zero;
                anim.SetBool("moving", false);
            }
        }
        else
        {
            // Player not detected, stop moving
            moveDirection = Vector2.zero;
            anim.SetBool("moving", false);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = moveDirection * speed;
        }
    }

    // Visualize detection radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}
