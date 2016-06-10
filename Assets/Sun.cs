using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

    public float rotationSpeed = 2;

    Transform centerOfEarth;

	// Use this for initialization
	void Start () {

        centerOfEarth = gameObject.transform.parent;
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(centerOfEarth.position, rotationSpeed * Time.deltaTime);
	}
}
