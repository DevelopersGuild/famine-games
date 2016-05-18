using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chest : MonoBehaviour, IInteractive
{
    private bool isOpen;
    private Animator Anim;
    public List<GameObject> items = new List<GameObject>();

    public void Start()
    {
        isOpen = false;
        Anim = GetComponent<Animator>();
    }

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

    public void addItem(GameObject item)
    {
        items.Add(item);
    }
}
