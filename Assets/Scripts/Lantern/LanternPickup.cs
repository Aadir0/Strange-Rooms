using UnityEngine;
using UnityEngine.InputSystem;

public class LanternPickup : MonoBehaviour
{
    [SerializeField] private GameObject lantern;
    [SerializeField] private GameObject thisLantern;
    [SerializeField] private float checkRadius;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private SpriteRenderer SpriteRenderer;
    
    private bool playerInRange = false;

    private void Update()
    {
        // Check if player is within pickup radius
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
                PickUpLantern();
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

    private void PickUpLantern()
    {
        // Hide the interact prompt
        if (InteractManager.instance != null)
        {
            InteractManager.instance.HideInteractPrompt();
        }
        
        lantern.SetActive(true);
        SpriteRenderer.color = new Color(0.65f, 0.65f, 0.65f, 1f);
        Destroy(thisLantern);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere in the editor to visualize the pickup radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
