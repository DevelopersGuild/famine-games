using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Point : NetworkBehaviour
{
    [SyncVar]
    public int points;

	// Use this for initialization
	void Start ()
	{
	    points = 0;
	}

    public void AddPoints(int amount)
    {
        if (!isServer)
            return;

        points += amount;
    }
}
