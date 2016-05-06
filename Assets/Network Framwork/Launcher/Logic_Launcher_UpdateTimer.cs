using UnityEngine;
using System.Collections;

public class Logic_Launcher_UpdateTimer : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}

    public void StartToUpdate()
    {
        InvokeRepeating("UpdateInfo", 30f, 30f);
        InvokeRepeating("UpdateMatches", 30f, 120f);
    }
	
	void UpdateInfo()
    {

    }

    void UpdateMatches()
    {

    }

    public void StopUpdating()
    {
        CancelInvoke();
    }
}
