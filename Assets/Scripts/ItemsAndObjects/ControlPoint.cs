using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ControlPoint : MonoBehaviour {   
    private Point points;
    private ParticleSystem[] particles = new ParticleSystem[2];
    public int timer;
    private float currentTime;
    private bool isactivated;
    private int numPlayers;  

    // Use this for initialization
    void Start () {
        isactivated = false;
        numPlayers = 0;
        currentTime = 0;

        int i = 0;
        foreach (Transform children in transform)
        {
            ParticleSystem child;
            if(child = children.GetComponent<ParticleSystem>())
            {
                particles[i] = child;
                i++;
            }
        }
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
                ChangeColors(Color.red);
                return;
            }
            else
            {
                ChangeColors(Color.blue);
                StartCoroutine("controlPointTimer");
            }
        }

    }

    void OnTriggerStay(Collider other)
    {
        points = other.GetComponent<Point>();
        if (points != null)
        {
            currentTime += Time.deltaTime;

            if (numPlayers > 1)
            {
                currentTime = 0;
                ChangeColors(Color.red);
                return;
            }
            else
            {
                ChangeColors(Color.blue);
                StartCoroutine("controlPointTimer");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        numPlayers--;
        if (numPlayers == 1)
        {
            ChangeColors(Color.blue);
            StartCoroutine("controlPointTimer");
        }
        else if(numPlayers < 1)
        {
            ChangeColors(Color.white);
            StopCoroutine("controlPointTimer");
        }
        else
        {
            ChangeColors(Color.red);
        }
    }

    void ChangeColors(Color color)
    {
        Debug.Log(color);
        for(int i = 0; i < particles.Length; i++)
        {
            particles[i].startColor = color;
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
