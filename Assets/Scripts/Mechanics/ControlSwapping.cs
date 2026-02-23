using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlSwapping : MonoBehaviour
{
    public static ControlSwapping instance { get; private set; }
    
    private bool controlsSwapped = false;
    
    private void Awake()
    {
        // Singleton pattern - persist across scenes
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }
    
    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Invert controls when entering Level 2 (scene index 2)
        if (scene.name == "Level 2" || scene.buildIndex == 2)
        {
            controlsSwapped = true;
        }
        else
        {
            controlsSwapped = false;
        }
    }
    
    public bool AreControlsSwapped()
    {
        return controlsSwapped;
    }
    
    // Helper method to get the correct input based on swap state
    public Vector2 GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        if (controlsSwapped)
        {
            // Invert both axes: A becomes right, D becomes left, W becomes down, S becomes up
            return new Vector2(-horizontal, -vertical);
        }
        else
        {
            return new Vector2(horizontal, vertical);
        }
    }
}
