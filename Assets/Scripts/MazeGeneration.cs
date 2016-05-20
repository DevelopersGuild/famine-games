using UnityEngine;
using System.Collections;

public class MazeGeneration : MonoBehaviour {

    const float LIMIT = 50;
    const float NLIMIT = -1 * LIMIT;

    float multx;
    float multy;
    float multz;
    float tracerx;
    float tracery;
    float tracerz;
    GameObject[] MazeObjects;
    int MazeObCount;
    Hub Hubref;
    Vector3[] PositionRegister;
    const int maxObjects = 1000;
    int PositionCount;
    int activePosition;
    float[] positionmultiple = new float[4]{ .5f, -.5f, 1.5f, -1.5f };
    string[] objectpaths = new string[4] { "Straight", "Branch", "End" , "Hub"};
    int[] objectpotential = new int[4] { 75, 85, 95, 100 };
    private System.Random rnd;

    // Use this for initialization
    void Start () {
        rnd = new System.Random(System.Guid.NewGuid().GetHashCode());
        MazeObjects = new GameObject[maxObjects];
        PositionRegister = new Vector3[maxObjects];
        PositionCount = 0;
        MazeObCount = 0;
        MazeObCount++;
        MazeObjects[MazeObCount] = makeHub(0, 0, 0);
       // Hubref= MazeObjects[0].GetComponent<Hub>();

        activePosition = gencoords();
        int contactTrace = rnd.Next(2, 7);

        for (int i = 0; i < contactTrace; i++)
        {
            createpath(activePosition, multx, multy, multz);
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
        PositionRegister[PositionCount] = new Vector3(x, y, z);
        MazeObjects[MazeObCount] = (GameObject)Instantiate(Resources.Load("Straight"), PositionRegister[PositionCount], transform.rotation);
        PositionCount++;
        MazeObCount++;
    }

    void makeBranch(float x, float y, float z)
    {
        PositionRegister[PositionCount] = new Vector3(x, y, z);
        MazeObjects[MazeObCount] = (GameObject)Instantiate(Resources.Load("Branch"), new Vector3(x, y, z), transform.rotation);
        PositionCount++;
        MazeObCount++;
        int facenum = rnd.Next(1, 4);
        int activeface = -1;
        while (facenum!=0)
        {
            activeface = rnd.Next(1, 7);
            while (activeface==activePosition)
                activeface = rnd.Next(1, 7);

            createpath(activeface, x, y, z);
            facenum--;
        }

    }

    void makeEnd(float x, float y, float z)
    {
        PositionRegister[PositionCount] = new Vector3(x, y, z);
        MazeObjects[MazeObCount] = (GameObject)Instantiate(Resources.Load("End"), new Vector3(x, y, z), transform.rotation);
        MazeObCount++;
        PositionCount++;
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

    void createpath(int inc, float xGo, float yGo, float zGo)
    {
        int makeitem = 0;
        while (xGo< LIMIT && xGo > NLIMIT && yGo < LIMIT && yGo > NLIMIT && zGo < LIMIT && zGo > NLIMIT)
        {
            makeitem = rnd.Next(0, 95);
            if (makeitem<objectpotential[0]) makeStraight(xGo, yGo, zGo);
            else if (makeitem<objectpotential[1])
            {
                makeBranch(xGo, yGo, zGo);
            }
            else if (makeitem<objectpotential[2])//if end
            {
                makeEnd(xGo, yGo, zGo);
                return;
            }

            if (inc == 1) xGo++;
            else if (inc == 2) xGo--;
            else if (inc == 3) yGo++;

            else if (inc == 4) yGo--;
            else if (inc == 5) zGo++;
            else zGo--; //inc==5
        }

        makeEnd(xGo, yGo, zGo);
    }
    
    bool collissioncheck(Vector3 test)
    //simple check, only checks based on center, not actual collission.
    {
        int i = 0;
        while (i!=PositionCount)
        {
            if (PositionRegister[i] == test) return false;
            i++;
        }
        return true;
    }

}
