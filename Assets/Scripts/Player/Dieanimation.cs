using UnityEngine;

public class Dieanimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    void Start()
    {
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }
}
