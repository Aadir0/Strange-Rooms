using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance { get; private set;}
    [SerializeField] private float speed;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Die());
        }
    }
    private IEnumerator Die()
    {
        speed = 0f;
        
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            spriteRenderer.enabled = false; // Hide player sprite immediately
            lanternGameObject.SetActive(false); // Hide lantern immediately
        }
        yield return new WaitForSeconds(1f);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
