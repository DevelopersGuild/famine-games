using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Defense : NetworkBehaviour {

    public Amror amror;
    [SyncVar]
    public int currentAmror;
    [SyncVar]
    public NetworkInstanceId amrorid;

	// Use this for initialization
	void Start () {
        currentAmror = 0;
	}

    // Returns whether the target died
    public int TakeDamage(int amount)
    {
        if (!isServer)
            return -1;

        //No amror
        if (!ClientScene.FindLocalObject(amrorid))
            return 0;

        if(!amror || amror.netId!=amrorid)
        {
            amror = ClientScene.FindLocalObject(amrorid).GetComponent<Amror>();
        }

        if(!amror)
        {
            return 0;
        }

        int baseamount = 0;

        //amror check
        if (amount >= amror.GetDefense())
        {
            currentAmror -= amror.GetDefense();
            baseamount = amror.GetDefense();
        }
        else
        {
            currentAmror -= amount;
            baseamount = amount;
        }

        // Destory Amror
        if (currentAmror <= 0)
        {
            int returnamount = baseamount + currentAmror;
            /*if (amror)
                Destroy(amror);
            amror = null;*/
            currentAmror = 0;
            // called on the server, will be invoked on the clients
            return returnamount;
        }

        return baseamount;
    }
    
    public void PickedUpAmror(Amror amror)
    {
        CmdUpdateAmror(amror.netId, amror.maxDurability);
        this.amror = amror;
    }

    [Command]
    public void CmdPickedUpAmrorInChest(GameObject prefab)
    {
        if (!isServer)
            return;
        GameObject newitem = Instantiate(prefab);
        if (newitem.GetComponent<Amror>())
        {
            PickedUpAmror(newitem.GetComponent<Amror>());
        }
        else if(newitem.GetComponentInChildren<Amror>())
        {
            PickedUpAmror(newitem.GetComponentInChildren<Amror>());
        }
    }

    public int GetCurrentAmror()
    {
        if(!amror)
            return 0;
        return currentAmror * 100 / amror.maxDurability;
    }
	// Update is called once per frame
	void Update () {
	
	}

    [Command]
    public void CmdUpdateAmror(NetworkInstanceId aid, int amroramount)
    {
        if (!isServer)
            return;
        amrorid = aid;
        currentAmror = amroramount;
    }

    [Command]
    public void CmdDeadAmrorBreak()
    {
        if(amror)
            amror.CmdSetMaxDurability(currentAmror);
        currentAmror = 0;
        amrorid= new NetworkInstanceId(0);
        if (!amror)
            return;
        amror.CmdMoveToPoint(transform.position);
        amror.CmdMakeVisible();
    }
}
