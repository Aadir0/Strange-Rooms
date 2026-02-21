using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public static InteractManager instance { get; private set; }
    
    [SerializeField] private GameObject interactPromptUI; // The UI GameObject showing "Press E to interact"
    
    private void Awake()
    {
        // Singleton pattern
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
        // Hide the prompt at start
        if (interactPromptUI != null)
        {
            interactPromptUI.SetActive(false);
        }
    }
    
    public void ShowInteractPrompt()
    {
        if (interactPromptUI != null)
        {
            interactPromptUI.SetActive(true);
        }
    }
    
    public void HideInteractPrompt()
    {
        if (interactPromptUI != null)
        {
            interactPromptUI.SetActive(false);
        }
    }
}
