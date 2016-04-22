using UnityEngine;
using System.Collections;
using System;

public class Hub : MonoBehaviour {
    static public int maxContact = 6;
    static public int minContact = 2;
    private System.Random rnd;
    public int contacts;


    // Use this for initialization
    void Start () {
        rnd = new System.Random(System.Guid.NewGuid().GetHashCode());
        contacts = rnd.Next(2, 7);
    }

    public int returnContacts()
    { return contacts; }

    // Update is called once per frame
    void Update () {
	
	}
};
