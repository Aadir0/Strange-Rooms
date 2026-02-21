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
    [SerializeField] private int sceneToLoadforDoor2 = 3;
    
    private bool hasBeenOpened = false;
    private bool playerInRange = false;
    
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
                    else
                    {
                        Debug.Log("Door is locked! You need a key.");
                    }
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
        
        // Hide the interact prompt
        if (InteractManager.instance != null)
        {
            InteractManager.instance.HideInteractPrompt();
        }
        
        // Remove the key from inventory if this door required one
        if (requiresKey && InventoryManager.instance != null)
        {
            InventoryManager.instance.RemoveKey(requiredKeyType);
            Debug.Log($"Used {requiredKeyType} to open the door. Key removed from inventory.");
        }
        yield return new WaitForSeconds(1f);

        if (requiredKeyType == KeyType.Key1) // Check if we're in the first scene
        {
            Debug.Log("Loading Scene 2...");
            SceneManager.LoadScene(sceneToLoadforDoor1);
        }
        else if (requiredKeyType == KeyType.Key2) // Check if we're in the second scene
        {
            Debug.Log("Loading Scene 3...");
            SceneManager.LoadScene(sceneToLoadforDoor2);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
