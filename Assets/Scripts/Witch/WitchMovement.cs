using UnityEngine;

public class WitchMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1.5f;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1.5f;
    
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float hurtDuration = 0.5f;
    
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Transform player;
    private Rigidbody2D rb;
    private float lastAttackTime;
    private int currentHealth;
    private bool isHurt = false;
    private bool isDead = false;
    private float hurtTimer = 0f;
    
    private enum EnemyState
    {
        Idle,
        Chasing,
        Attacking
    }
    
    private EnemyState currentState = EnemyState.Idle;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        
        // Find the player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("WitchMovement: Player not found! Make sure the player has the 'Player' tag.");
        }
        
        // Get animator if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        // Get sprite renderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (player == null || isDead) return;
        
        // Handle hurt state timer
        if (isHurt)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0)
            {
                isHurt = false;
                if (animator != null)
                {
                    animator.SetBool("hurt", false);
                }
            }
            return; // Don't do anything else while hurt
        }
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Update state based on distance
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            currentState = EnemyState.Idle;
        }
        
        // Handle state behaviors
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;
            case EnemyState.Chasing:
                HandleChasing();
                break;
            case EnemyState.Attacking:
                HandleAttacking();
                break;
        }
    }
    
    private void HandleIdle()
    {
        // Stop moving
        rb.linearVelocity = Vector2.zero;
        
        if (animator != null)
        {
            animator.SetBool("walk", false);
            animator.SetBool("attack", false);
        }
    }
    
    private void HandleChasing()
    {
        // Move towards player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        
        // Flip sprite to face player
        if (spriteRenderer != null)
        {
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // Face left
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // Face right
            }
        }
        
        // Update animation
        if (animator != null)
        {
            animator.SetBool("walk", true);
            animator.SetBool("attack", false);
        }
    }
    
    private void HandleAttacking()
    {
        // Stop moving when attacking
        rb.linearVelocity = Vector2.zero;
        
        // Face the player while attacking
        if (spriteRenderer != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // Face left
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // Face right
            }
        }
        
        // Attack if cooldown is ready
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
        
        if (animator != null)
        {
            animator.SetBool("walk", false);
            animator.SetBool("attack", true);
        }
    }
    
    private void Attack()
    {
        Debug.Log("Witch attacks the player!");
        
        // Check if player is still in range and deal damage
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            // Trigger player's Die coroutine
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.StartCoroutine("Die");
                Debug.Log("Player has been killed by the witch!");
            }
            else
            {
                Debug.LogWarning("PlayerMovement component not found on player!");
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead || isHurt) return;
        
        currentHealth -= damage;
        Debug.Log($"Witch took {damage} damage! Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Enter hurt state
            isHurt = true;
            hurtTimer = hurtDuration;
            rb.linearVelocity = Vector2.zero;
            
            if (animator != null)
            {
                animator.SetBool("hurt", true);
                animator.SetBool("walk", false);
                animator.SetBool("attack", false);
            }
        }
    }
    
    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        
        Debug.Log("Witch has been defeated!");
        
        if (animator != null)
        {
            animator.SetBool("die", true);
            animator.SetBool("walk", false);
            animator.SetBool("attack", false);
            animator.SetBool("hurt", false);
        }
        
        // Disable collider and rigidbody
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        // Destroy witch after animation plays (adjust time based on your death animation length)
        Destroy(gameObject, 2f);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Take damage when hitting objects with "Enemy" tag (traps, spikes, etc.)
        if (collision.CompareTag("Enemy") && !isDead)
        {
            TakeDamage(1);
        }
    }
    
    // Visualize detection and attack ranges in the editor
    private void OnDrawGizmosSelected()
    {
        // Detection range (yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Attack range (red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
