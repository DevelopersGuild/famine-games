using UnityEngine;
using System.Collections;

public class Capsule : MonoBehaviour {
    
    public GameObject weapon;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    
	}


    void equip()
    {
        GameObject temp = Instantiate(weapon, Vector3.zero, Quaternion.identity) as GameObject;
        temp.transform.parent = gameObject.transform;
    }

}
