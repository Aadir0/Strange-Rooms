using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Entry : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has entered the room!");
            SceneManager.LoadScene(1);
        }
    }
}
