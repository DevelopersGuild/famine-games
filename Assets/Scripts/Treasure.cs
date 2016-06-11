﻿using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {

    public GameObject[] Teir1 = new GameObject[5];
    public GameObject[] Teir2 = new GameObject[6];
    public GameObject[] Teir3 = new GameObject[5];

    private System.Random rnd;
    Chest thisChest;

    // Use this for initialization
    void Start () {
        thisChest = GetComponent<Chest>();
        rnd = new System.Random(System.Guid.NewGuid().GetHashCode());

        bool getTeir1 = false;
        bool getTeir2 = false;
        bool getTeir3 = false;
         
        int maxbound = Teir1.Length + Teir2.Length + Teir3.Length;
        int item = rnd.Next(0, maxbound-1);
        int numItems = rnd.Next(1, 3);



        if (item >= Teir1.Length)
            item = item - Teir1.Length;
        else
            getTeir1 = true;

        if (item >= Teir2.Length)
            item = item - Teir2.Length;
        else
            getTeir2 = true;

        if (getTeir1 == false && getTeir2 == false)
            getTeir3 = true;

        if (getTeir1)
            thisChest.addItem(Teir1[item]);
        else if (getTeir2)
            thisChest.addItem(Teir2[item]);
        else if (getTeir3)
            thisChest.addItem(Teir3[item]);
        else
            Debug.Log("Something went wrong");
}
	
	// Update is called once per frame
	void Update () {
	
	}
}