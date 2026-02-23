using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance { get; private set;}
    public float speed;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject deathEffectPrefab;
    [SerializeField] private GameObject lanternGameObject; 
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private bool facingRight = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        
        // Apply control swapping if the system is active
        if (ControlSwapping.instance != null)
        {
            Vector2 swappedInput = ControlSwapping.instance.GetMovementInput();
            inputX = swappedInput.x;
            inputY = swappedInput.y;
        }
        
        moveDirection = new Vector2(inputX, inputY).normalized;

        anim.SetFloat("moveX", inputX);
        anim.SetFloat("moveY", inputY);

        if (inputX > 0)
        {
            facingRight = true;
        }
        else if (inputX < 0)
        {
            facingRight = false;
        }

        if (moveDirection == Vector2.zero)
        {
            anim.SetBool("moving", false);
        }
        else
        {
            anim.SetBool("moving", true);
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }
    public bool IsFacingRight()
    {
        return facingRight;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(Die());
        }
    }
    public IEnumerator Die()
    {
        speed = 0f;
        
        if (deathEffectPrefab != null)
        {
            yield return new WaitForSeconds(0.3f); // Small delay to ensure the player is still visible before the effect
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            spriteRenderer.enabled = false; // Hide player sprite immediately
            lanternGameObject.SetActive(false); // Hide lantern immediately
        }
        yield return new WaitForSeconds(2f);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
