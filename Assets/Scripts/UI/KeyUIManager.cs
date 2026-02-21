using UnityEngine;
using UnityEngine.UI;

public class KeyUIManager : MonoBehaviour
{
    public static KeyUIManager instance { get; private set; }
    
    [SerializeField] private GameObject key1UIObject; // The UI GameObject (Image) for Key 1
    [SerializeField] private GameObject key2UIObject; // The UI GameObject (Image) for Key 2

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple KeyUIManager instances detected! Destroying duplicate.");
            Destroy(this);
        }
        else
        {
            instance = this;
            Debug.Log("KeyUIManager initialized successfully.");
        }
    }

    private void Start()
    {
        Debug.Log($"KeyUIManager Start - Key1 UI assigned: {key1UIObject != null}, Key2 UI assigned: {key2UIObject != null}");
        
        // Hide both keys UI at start
        if (key1UIObject != null)
        {
            key1UIObject.SetActive(false);
        }
        if (key2UIObject != null)
        {
            key2UIObject.SetActive(false);
        }
    }

    public void ShowKey(KeyType keyType)
    {
        Debug.Log($"KeyUIManager: ShowKey called for {keyType}");
        
        if (keyType == KeyType.Key1)
        {
            if (key1UIObject != null)
            {
                Debug.Log("Setting Key1 UI to active");
                key1UIObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Key1 UI Object is not assigned in KeyUIManager! Assign it in the Inspector.");
            }
        }
        else if (keyType == KeyType.Key2)
        {
            if (key2UIObject != null)
            {
                Debug.Log("Setting Key2 UI to active");
                key2UIObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Key2 UI Object is not assigned in KeyUIManager! Assign it in the Inspector.");
            }
        }
    }

    public void HideKey(KeyType keyType)
    {
        if (keyType == KeyType.Key1 && key1UIObject != null)
        {
            key1UIObject.SetActive(false);
        }
        else if (keyType == KeyType.Key2 && key2UIObject != null)
        {
            key2UIObject.SetActive(false);
        }
    }
}
