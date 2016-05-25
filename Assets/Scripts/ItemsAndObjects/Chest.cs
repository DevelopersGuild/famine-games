using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kroulis.UI.MainGame;

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
            MainPanelFullControl mpfc = GameObject.Find("Main_UI").GetComponentInChildren<MainPanelFullControl>();
            if(mpfc)
            {
                //mpfc.ShowLootWindow(this);
                mpfc.StartCoroutine(mpfc.ShowLootWindow(this,0.7f));
            }
        }
        else if (isOpen)
        {
            isOpen = false;
            MainPanelFullControl mpfc = GameObject.Find("Main_UI").GetComponentInChildren<MainPanelFullControl>();
            if (mpfc)
            {
                mpfc.HideLootWindow();
            }
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

    public void removeItem(GameObject item)
    {
        items.Remove(item);
    }
}
