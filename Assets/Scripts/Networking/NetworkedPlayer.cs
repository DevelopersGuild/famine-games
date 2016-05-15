using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Kroulis.Components;

public class NetworkedPlayer : NetworkBehaviour
{
    public FirstPersonController fpsController;
    public Camera fpsCamera;
    public AudioSource audioSource;
    public AttackController attackController;
    public Point points;
    public BowAndArrow bowAndArrow;
    public Defense defense;
    public UserNameTab playerinfo;

    public override void OnStartLocalPlayer()
    {
        fpsController.enabled = true;
        fpsCamera.enabled = true;
        audioSource.enabled = true;
        attackController.enabled = true;
        points.enabled = true;
        bowAndArrow.enabled = true;
        defense.enabled = true;
        playerinfo.enabled = true;

        if (GameObject.Find("Logic_Network"))
        {
            playerinfo.player_name = GameObject.Find("Logic_Network").GetComponentInChildren<Logic_LauncherGetInfo>().GetCharacterNameA();
        }
        else
        {
            playerinfo.player_name = "UnRegPlayer"+Random.Range(10000,99999).ToString();
        }

        gameObject.name = "LOCAL Player";
        base.OnStartLocalPlayer();
    }

}
