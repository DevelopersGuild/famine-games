using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkedPlayer : NetworkBehaviour
{
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;
    public Camera fpsCamera;
    public AudioSource audioSource;
    public AttackController attackController;
    public Point points;


    public override void OnStartLocalPlayer()
    {
        fpsController.enabled = true;
        fpsCamera.enabled = true;
        audioSource.enabled = true;
        attackController.enabled = true;
        points.enabled = true;

        gameObject.name = "LOCAL Player";
        base.OnStartLocalPlayer();
    }

}
