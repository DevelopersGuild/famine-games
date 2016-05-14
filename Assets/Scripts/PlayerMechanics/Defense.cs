using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Defense : NetworkBehaviour {

    public Amror amror;
    [SyncVar]
    public int currentAmror;

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
        if(!amror)
            return 0;

        int baseamount = 0;

        //amror check
        if (amount >= amror.defense)
        {
            currentAmror -= amror.defense;
            baseamount = amror.defense;
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
            if (amror)
                Destroy(amror);
            amror = null;
            currentAmror = 0;
            // called on the server, will be invoked on the clients
            return returnamount;
        }

        return baseamount;
    }
    
    public void PickedUpAmror(Amror amror)
    {
        this.amror = amror;
        currentAmror = amror.maxDurability;
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
}
