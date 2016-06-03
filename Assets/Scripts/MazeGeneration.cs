using UnityEngine;
using System.Collections;

public class MazeGeneration : MonoBehaviour {

    const float LIMIT = 100;
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
    public const int maxObjects = 10000;
    int PositionCount;
    int activePosition;
    float[] positionmultiple = new float[4]{ .5f, -.5f, 1.5f, -1.5f };
    string[] objectpaths = new string[4] { "Straight", "Branch", "End" , "Hub"};
    int[] objectpotential = new int[4] { 65, 85, 95, 100 };
    private System.Random rnd;
    int flip;
    Quaternion activetransform;

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
        flip = setflip(activePosition);

        if (activePosition == 3 || activePosition == 4) //active Y
            activetransform = Quaternion.Euler(0, 0, 90);

        else if (activePosition == 5 || activePosition == 6) //active Z
            activetransform = Quaternion.Euler(0, 90, 0);

        else activetransform = Quaternion.Euler(0, 0, 0);

        for (int i = 0; i < contactTrace; i++)
        {
            createpath(activePosition, multx, multy, multz);
            activePosition = gencoords();
            flip = setflip(activePosition);

            if (activePosition == 3 || activePosition == 4) //active Y
                activetransform = Quaternion.Euler(0, 0, 90);

            else if (activePosition == 5 || activePosition == 6) //active Z
                activetransform = Quaternion.Euler(0, 90, 0);

            else activetransform = Quaternion.Euler(0, 0, 0);
        }

        Debug.Log(MazeObCount);

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
        MazeObjects[MazeObCount] = (GameObject)Instantiate(Resources.Load("Straight"), PositionRegister[PositionCount], activetransform);
        PositionCount++;
        MazeObCount++;
    }

    void makeBranch(float x, float y, float z)
    {
        Quaternion holdquat = activetransform;
        int holdpos = activePosition;
        flip = setflip(activePosition);
        PositionRegister[PositionCount] = new Vector3(x, y, z);
        MazeObjects[MazeObCount] = (GameObject)Instantiate(Resources.Load("Branch"), new Vector3(x, y, z), transform.rotation);
        PositionCount++;
        MazeObCount++;
        int branches = rnd.Next(1, 4);
        int activeface = -1;
        int[] faces = new int[2 + branches];
        faces[0] = holdpos; faces[1] = flip;
        int numfaces = 1; // index for faces
        bool exit = false;
        bool newface = true;


        while (branches!=0)
        {
            while (!exit)
            {
                activeface = rnd.Next(1, 7);
                newface = true;
                for (int i=0; i<faces.Length; i++)
                {
            
                    if (faces[i] == activeface)
                        newface = false;
                }

                if (newface)
                {
                    exit = true;

                }
            }

            if (activeface == 3 || activeface == 4) //active Y
                activetransform = Quaternion.Euler(0, 0, 90);

            else if (activeface == 5 || activeface == 6) //active Z
                activetransform = Quaternion.Euler(0, 90, 0);

            else activetransform = Quaternion.Euler(0, 0, 0);
            Debug.Log("Begin Path. Active face:" + activeface);
            if (activeface ==1) createpath(activeface, x + 1, y, z);
            else if(activeface == 2) createpath(activeface, x - 1, y, z);
            else if(activeface == 3) createpath(activeface, x, y+1, z);
            else if(activeface == 4) createpath(activeface, x, y-1, z);
            else if(activeface == 5) createpath(activeface, x, y, z+1);
            else  createpath(activeface, x, y, z-1);
            faces[numfaces] = activeface;
            branches--;

            Debug.Log("End path. Active face: " + activeface);
        }
        activePosition = holdpos;
        activetransform = holdquat;
}

    void makeEnd(float x, float y, float z)
    {
        PositionRegister[PositionCount] = new Vector3(x, y, z);
        MazeObjects[MazeObCount] = (GameObject)Instantiate(Resources.Load("End"), new Vector3(x, y, z), activetransform);
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
    //  Debug.Log("My Pos:" + activePosition);

        int makeitem = 0;

        Vector3 test; //set check variable to see if there is a block next, if yes, then stop
        Vector3 test2 = new Vector3(xGo, yGo, zGo);
        while (xGo< LIMIT && xGo > NLIMIT && yGo < LIMIT && yGo > NLIMIT && zGo < LIMIT && zGo > NLIMIT)
        {
            if (inc == 1) test = new Vector3(xGo+1, yGo, zGo);
            else if (inc == 2) test = new Vector3(xGo-1, yGo, zGo);
            else if (inc == 3) test = new Vector3(xGo, yGo+1, zGo);
            else if (inc == 4) test = new Vector3(xGo, yGo-1, zGo);
            else if (inc == 5) test = new Vector3(xGo, yGo, zGo+1);
            else test = new Vector3(xGo, yGo, zGo);//inc==5

            // bool testcheck = collissioncheck(test);

            makeitem = rnd.Next(0, 95);

             if (collissioncheck(test))
              {
                if (collissioncheck(test2))
                    return;

                else
                {
                 // Debug.Log("Making end at" + test);
                    return;
                }
              } 

            if (makeitem < objectpotential[0])
            { makeStraight(xGo, yGo, zGo); Debug.Log("Making straight at" + test); }
            else if (makeitem < objectpotential[1])
            {
                Debug.Log("Making branch at" + test); makeBranch(xGo, yGo, zGo);
            }

            else if (makeitem < objectpotential[2])//if end
            {
                makeEnd(xGo, yGo, zGo);
                Debug.Log("Making end at" + test);
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
        while (i<PositionCount)
        {
            if (PositionRegister[i] == test)
            {
                return true;
                Debug.Log("Colliding!!");
            }
            i++;
        }
        return false;
    }

    int setflip (int face)
    {
        if (face == 1)
            return 2;

        if (face == 2)
            return 1;

        if (face == 3)
            return 4;

        if (face == 4)
            return 3;

        if (face == 5)
            return 6;

        if (face == 6)
            return 5;

        return -1;
    }

}

