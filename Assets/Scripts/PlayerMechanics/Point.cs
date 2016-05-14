using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Point : NetworkBehaviour
{
    [SyncVar]
    public int points;
    [SyncVar]
    public int deaths;
    [SyncVar]
    public int kills;

	// Use this for initialization
	void Start ()
	{
	    points = 0;
	    deaths = 0;
	    kills = 0;
	}

    public void AddPoints(int amount)
    {
        if (!isServer)
            return;

        points += amount;
        if(points > 200) CmdEndGame();
    }

    public void incKills()
    {
        if (!isServer)
            return;

        kills++;
    }

    public void incDeaths()
    {
        if (!isServer)
            return;

        deaths++;
    }

    [Command]
    public void CmdEndGame()
    {
        
    }
}
