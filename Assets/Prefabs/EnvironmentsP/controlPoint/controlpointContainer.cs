﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class controlpointContainer : MonoBehaviour {

    public GameObject[] arrayList = new GameObject[5];
    private float timer;
    public int interval;
    public GameObject goldenChest;
    private GameObject chestRef;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        int i = 0;
        foreach (Transform children in transform)
        {
            arrayList[i] = children.gameObject;
            children.gameObject.SetActive(false);
            i++;
        }

        int randomInt = (int)(Mathf.Floor(Random.Range(0, arrayList.GetLength(0))));
        arrayList[randomInt].gameObject.SetActive(true);
        chestRef = (GameObject)Instantiate(goldenChest, arrayList[randomInt].transform.position, Quaternion.identity);
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
        Destroy(chestRef);
        int i = 0;
        foreach (GameObject children in arrayList)
        {
            arrayList[i] = children.gameObject;
            children.gameObject.SetActive(false);
            i++;
        }

        int randomInt = (int)(Mathf.Floor(Random.Range(0, arrayList.GetLength(0))));
        arrayList[randomInt].gameObject.SetActive(true);
        chestRef = (GameObject)Instantiate(goldenChest, arrayList[randomInt].transform.position, Quaternion.identity);
    }
}
