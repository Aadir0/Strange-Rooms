using Unity.VisualScripting;
using UnityEngine;

public class ChestOpening : MonoBehaviour
{
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool givesKey = false; // Set to true for chests that give keys
    [SerializeField] private KeyType keyType = KeyType.Key1; // Which key this chest gives
    
    private bool hasBeenOpened = false;
    private bool playerInRange = false;
    
    private void Update()
    {
        if (hasBeenOpened) return; // Prevent multiple opens
        
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
                OpenChest();
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
    
    private void OpenChest()
    {
        chestAnimator.SetTrigger("Open");
        hasBeenOpened = true;
        
        // Hide the interact prompt
        if (InteractManager.instance != null)
        {
            InteractManager.instance.HideInteractPrompt();
        }
        
        // Give key if this chest contains one
        if (givesKey)
        {
            if (InventoryManager.instance != null)
            {
                InventoryManager.instance.AddKey(keyType);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}