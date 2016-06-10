using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class controlpointContainer : MonoBehaviour {

    public GameObject[] arrayList = new GameObject[5];
    private float timer;
    public int interval;

    // Use this for initialization
    void Start() {
        timer = 0;
        int i = 0;
        foreach (Transform children in transform)
        {
            arrayList[i] = children.gameObject;
            children.gameObject.SetActive(false);
            i++;
        }

        arrayList[(int)(Mathf.Floor(Random.Range(0, arrayList.GetLength(0))))].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;

        if (timer > interval)
        {
            timer = 0;

            ResetControlPoint();
        }
    }
    
    void ResetControlPoint()
    {
        int i = 0;
        foreach (GameObject children in arrayList)
        {
            arrayList[i] = children.gameObject;
            children.gameObject.SetActive(false);
            i++;
        }

        arrayList[(int)(Mathf.Floor(Random.Range(0, arrayList.GetLength(0))))].gameObject.SetActive(true);
    }
}
