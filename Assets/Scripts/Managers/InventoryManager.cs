using UnityEngine;

public enum KeyType
{
    Key1,
    Key2
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }
    
    private bool hasKey1 = false;
    private bool hasKey2 = false;

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple InventoryManager instances detected! Destroying duplicate.");
            Destroy(this);
        }
        else
        {
            instance = this;
            Debug.Log("InventoryManager initialized successfully.");
        }
    }

    public void AddKey(KeyType keyType)
    {
        Debug.Log($"InventoryManager: Adding {keyType}");
        
        if (keyType == KeyType.Key1)
        {
            hasKey1 = true;
        }
        else if (keyType == KeyType.Key2)
        {
            hasKey2 = true;
        }
        
        // Notify UI to show the key
        if (KeyUIManager.instance != null)
        {
            Debug.Log($"Calling KeyUIManager.ShowKey({keyType})");
            KeyUIManager.instance.ShowKey(keyType);
        }
        else
        {
            Debug.LogError("KeyUIManager instance is null! Make sure KeyUIManager is in the scene.");
        }
    }

    public bool HasKey(KeyType keyType)
    {
        if (keyType == KeyType.Key1)
        {
            return hasKey1;
        }
        else if (keyType == KeyType.Key2)
        {
            return hasKey2;
        }
        return false;
    }

    public void RemoveKey(KeyType keyType)
    {
        if (keyType == KeyType.Key1)
        {
            hasKey1 = false;
        }
        else if (keyType == KeyType.Key2)
        {
            hasKey2 = false;
        }
        
        // Notify UI to hide the key
        if (KeyUIManager.instance != null)
        {
            KeyUIManager.instance.HideKey(keyType);
        }
    }
}
