using UnityEngine;
using System.Collections;

public class MazeGeneration : MonoBehaviour {

    const float LIMIT = 10;
    const float NLIMIT = -1 * LIMIT;

    float multx;
    float multy;
    float multz;
    float tracerx;
    float tracery;
    float tracerz;
    GameObject[] MazeObjects;
    Hub Hubref;
    Vector3[] PositionRegister;
    const int maxObjects = 100;
    int PositionCount;
    int activePosition;
    float[] positionmultiple = new float[4]{ .5f, -.5f, 1.5f, -1.5f };
    private System.Random rnd;

    // Use this for initialization
    void Start () {
        rnd = new System.Random(System.Guid.NewGuid().GetHashCode());
        MazeObjects = new GameObject[maxObjects];
        PositionRegister = new Vector3[maxObjects];
        PositionCount = 0;
        MazeObjects[0] = makeHub(0, 0, 0);
        Hubref= MazeObjects[0].AddComponent<Hub>();
        Debug.Log("Catch"+Hubref.returnContacts());
        activePosition = gencoords();
        int contactTrace = 6;

        for (int i = 0; i < contactTrace; i++)
        {
            createpath(activePosition);
            Debug.Log(activePosition);
            activePosition = gencoords();
        }

    }
	
	// Update is called once per frame
	void Update () {

    }

    
    GameObject makeHub(float x, float y, float z)
    {
        PositionCount++;
        return (GameObject)Instantiate(Resources.Load("Hub"), new Vector3(x, y, z), transform.rotation);
    }

    void makeStraight (float x, float y, float z)
    {
        Instantiate(Resources.Load("Straight"), new Vector3(x, y, z), transform.rotation);
    }

    void makeBranch(float x, float y, float z)
    {
        Instantiate(Resources.Load("Branch"), new Vector3(x, y, z), transform.rotation);
    }

    void makeEnd(float x, float y, float z)
    {
        Instantiate(Resources.Load("End"), new Vector3(x, y, z), transform.rotation);
    }

    int gencoords()
    {
        multx = multy = multz = 0f;
        while (multx != 1.5f  && multy !=1.5f && multz != 1.5f && multx !=-1.5f && multy !=-1.5f && multz!=-1.5f)
        {
            multx = positionmultiple[rnd.Next(0, 4)];
            if (multx == 1.5f || multx == -1.5f)
            { multy = positionmultiple[rnd.Next(0, 2)]; }
            else multy = positionmultiple[rnd.Next(0, 4)];
            if (multx == 1.5f || multx == -1.5f || multy == 1.5 || multy == -1.5)
            { multz = positionmultiple[rnd.Next(0, 2)]; }
            else multz = positionmultiple[rnd.Next(0, 4)];
        }
        if (multx == 1.5) return 1;
        else if (multx == -1.5f) return 2;
        else if (multy == 1.5f) return 3;
        else if (multy == -1.5f) return 4;
        else if (multz == 1.5f) return 5;
        else if (multz == -1.5f) return 6;
        else return -1;
    }

    void createpath(int inc)
    {
        while (multx< LIMIT && multx > NLIMIT && multy < LIMIT && multy > NLIMIT && multz < LIMIT && multz > NLIMIT)
        {
            makeStraight(multx, multy, multz);
            if (inc == 1) multx++;
            else if (inc == 2) multx--;
            else if (inc == 3) multy++;
            else if (inc == 4) multy--;
            else if (inc == 5) multz++;
            else multz--; //inc==5
        }

        makeEnd(multx, multy, multz);
    }
}
