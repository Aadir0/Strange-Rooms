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
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
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
        
        if (keyType == KeyType.Key1)
        {
            if (key1UIObject != null)
            {
                key1UIObject.SetActive(true);
            }
        }
        else if (keyType == KeyType.Key2)
        {
            if (key2UIObject != null)
            {
                key2UIObject.SetActive(true);
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
