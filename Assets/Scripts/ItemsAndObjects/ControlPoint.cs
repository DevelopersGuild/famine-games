using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ControlPoint : MonoBehaviour {   
    private Point points;
    public int timer;
    private bool isactivated;
    private int numPlayers;  

    // Use this for initialization
    void Start () {
        isactivated = false;
        numPlayers = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        points = other.GetComponent<Point>();
        if (points != null)
        {
            numPlayers++;
            if (numPlayers > 1)
            {
                return;
            }
            else
            {
                StartCoroutine("controlPointTimer");
            }
        }

    }

    void OnTriggerStay(Collider other)
    {
        points = other.GetComponent<Point>();
        if (points != null)
        {
            if (numPlayers > 1)
            {
                return;
            }
            else
            {
                StartCoroutine("controlPointTimer");
            }
            }
    }

    void OnTriggerExit(Collider other)
    {
        numPlayers--;
        if (numPlayers < 2)
        {
            StartCoroutine("controlPointTimer");
        }
        else
        {
            StopCoroutine("controlPointTimer");
        }
    }

    IEnumerator controlPointTimer()
    {
        yield return new WaitForSeconds(timer);
        points.AddPoints(1);
        controlPointTimerUndo();
    }

    void controlPointTimerUndo()
    {
        StopCoroutine("controlPointTimer");

    }
}
