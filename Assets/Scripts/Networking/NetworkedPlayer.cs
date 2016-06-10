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
    public ContestInfomation playerinfo;
    public GameObject playerModel;

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
        playerModel.layer = LayerMask.NameToLayer("Player");
        GetComponentInChildren<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponentInChildren<NetworkAnimator>().SetParameterAutoSend(1, true);

        if (GameObject.Find("Logic_Network"))
        {
            //playerinfo.player_name = GameObject.Find("Logic_Network").GetComponentInChildren<Logic_LauncherGetInfo>().GetCharacterNameA();
            playerinfo.CmdUpdatePlayerName(GameObject.Find("Logic_Network").GetComponentInChildren<Logic_LauncherGetInfo>().GetCharacterNameA(),Globe.uid);
        }
        else
        {
            //playerinfo.player_name = "UnRegPlayer"+Random.Range(10000,99999).ToString();
            playerinfo.CmdUpdatePlayerName("UnRegPlayer" + Random.Range(10000, 99999).ToString(),"");
        }

        gameObject.name = "LOCAL Player";
        base.OnStartLocalPlayer();
    }

    public override void PreStartClient()
    {
        GetComponentInChildren<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponentInChildren<NetworkAnimator>().SetParameterAutoSend(1, true);
    }

}
