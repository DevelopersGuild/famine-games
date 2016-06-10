using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DisableModel : NetworkBehaviour
{

    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
            gameObject.SetActive(false);
    }

}
