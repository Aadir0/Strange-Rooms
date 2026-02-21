using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectDestruction : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy1;
    [SerializeField] private GameObject objectToDestroy2;

    [SerializeField] private GameObject objectToDestroy3;

    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private BoxCollider2D boxCollider2D;

    private void Start()
    {
        objectToActivate.SetActive(false);
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DestroySequence());
        }
    }

    private IEnumerator DestroySequence()
    {
        Destroy(objectToDestroy1);

        objectToDestroy2.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(objectToDestroy2);
        
        objectToDestroy3.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(objectToDestroy3);
        
        objectToActivate.SetActive(true);
    }
}
