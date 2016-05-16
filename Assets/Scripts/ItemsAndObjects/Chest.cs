using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour, IInteractive
{
    public GameObject ContainerTop;
    public AnimationClip OpenClip, CloseClip;
    public bool isOpen = false;
    public Animator Anim;

    public void InteractWith()
    {
        if (!isOpen)
        {
            isOpen = true;
        }
        else if (isOpen)
        {
            isOpen = false;
        }

        Anim.SetBool("IsOpen", isOpen);
    }

    public void Update()
    {
        Anim.SetBool("IsOpen", isOpen);
    }
}
