using System;
using Unity.VisualScripting;
using UnityEngine;

public class LeverPulling : MonoBehaviour
{
    [SerializeField] private Animator leverAnimator;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject gameObjectToActivate;
    private bool hasBeenPulled = false;
    private bool playerInRange = false;

    private void Update()
    {
        if (hasBeenPulled) return; // Prevent multiple pulls
        
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
                PullLever();
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
    
    private void PullLever()
    {
        leverAnimator.SetTrigger("Pull");
        hasBeenPulled = true;
        
        // Hide the interact prompt
        if (InteractManager.instance != null)
        {
            InteractManager.instance.HideInteractPrompt();
        }
        
        Debug.Log("Lever pulled!");
        gameObjectToActivate.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}