using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AttackController : NetworkBehaviour
{

    public GameObject attackCollider;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetMouseButtonDown(0))
	    {
            CmdAttack();
        }
    }

    [Command]
    void CmdAttack()
    {
        Debug.Log("BAM!");
        // create the bullet object from the bullet prefab
        var attack = (GameObject)Instantiate(
            attackCollider,
            transform.position - transform.forward * -2,
            Quaternion.identity);

        NetworkServer.Spawn(attack);

        // make attack colliders disappear after 2 seconds
        Destroy(attack, .1f);
    }
}
