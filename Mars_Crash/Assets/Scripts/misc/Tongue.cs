using UnityEngine;

public class Tongue : MonoBehaviour
{
     Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!animator.GetBool("tong_out") && !animator.GetBool("tong_in"))
            return;

        TongueSound();
    }
    public void TongueSound()
    {
        SoundManager.Instance.PlaySound2D("Tong");
    }
}
