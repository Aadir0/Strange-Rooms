using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorOpening : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool requiresKey = false; // Set to true if this door needs a key
    [SerializeField] private KeyType requiredKeyType = KeyType.Key1; // Which key is needed
    [SerializeField] private int sceneToLoadforDoor1 = 2;
    [SerializeField] private GameObject gameOverScreen; // Reference to the Game Over screen for the second door
    [SerializeField] private BoxCollider2D doorCollider; // Reference to the door's collider to disable it when the door opens
    private AttractingForce AttractingForce; // Reference to the AttractingForce script to disable it when the door opens
    
    private bool hasBeenOpened = false;
    private bool playerInRange = false;
    private void Start()
    {
        AttractingForce = FindAnyObjectByType<AttractingForce>();
    }
    
    void Update()
    {
        if (hasBeenOpened) return; // Already opened
        
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, checkRadius, playerLayer);
        
        // Show/hide interact prompt based on player proximity
        if (playerCollider != null)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                if (InteractManager.instance != null)
                {
                    InteractManager.instance.ShowInteractPrompt();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Check if key is required
                if (requiresKey)
                {
                    // Check if player has the required key
                    if (InventoryManager.instance != null && InventoryManager.instance.HasKey(requiredKeyType))
                    {
                        StartCoroutine(OpenDoor());
                    }
                }
                else
                {
                    doorCollider.isTrigger = true;
                    doorAnimator.SetTrigger("isOpen");
                    hasBeenOpened = true;
                }
            }
        }
        else
        {
            if (playerInRange)
            {
                playerInRange = false;
                if (InteractManager.instance != null)
                {
                    InteractManager.instance.HideInteractPrompt();
                }
            }
        }
    }
    
    private IEnumerator OpenDoor()
    {
        doorAnimator.SetTrigger("isOpen");
        hasBeenOpened = true;
        if(SceneManager.GetActiveScene().buildIndex == 3) // Only disable attraction force in the first scene
        {
            AttractingForce.attractionForce = 0f; // Stop the attraction force when the door opens
        }

        // Hide the interact prompt
        if (InteractManager.instance != null)
        {
            InteractManager.instance.HideInteractPrompt();
        }
        
        // Remove the key from inventory if this door required one
        if (requiresKey && InventoryManager.instance != null)
        {
            InventoryManager.instance.RemoveKey(requiredKeyType);
        }
        yield return new WaitForSeconds(1f);

        if (requiredKeyType == KeyType.Key1) // Check if we're in the first scene
        {
            SceneManager.LoadScene(sceneToLoadforDoor1);
        }
        else if (requiredKeyType == KeyType.Key2) // Check if we're in the second scene
        {
            PlayerMovement.instance.speed = 0f; // Stop player movement
            yield return new WaitForSeconds(0.5f);
            gameOverScreen.SetActive(true);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
