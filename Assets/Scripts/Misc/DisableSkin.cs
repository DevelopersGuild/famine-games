using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DisableSkin : NetworkBehaviour {

    // Use this for initialization
    public void Start()
    {
        GetComponent<SkinnedMeshRenderer>().gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
