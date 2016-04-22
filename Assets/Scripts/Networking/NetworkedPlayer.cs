using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkedPlayer : NetworkBehaviour
{
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;
    public Camera fpsCamera;
    public AudioSource audioSource;

    public override void OnStartLocalPlayer()
    {
        fpsController.enabled = true;
        fpsCamera.enabled = true;
        audioSource.enabled = true;

        gameObject.name = "LOCAL Player";
        base.OnStartLocalPlayer();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
