﻿using UnityEngine;
using System.Collections;
using Kroulis.Components;

namespace Kroulis.UI.MainGame
{

    public class MainUIFullControl : MonoBehaviour
    {
        public GameObject StartGameWaiting;
        public GameObject MainPanel;
        public GameObject ScorePanel;
        public bool ShowUI;
        private bool StartGameFlag = false;

        void Awake()
        {
            StartGameFlag = false;
        }
        // Use this for initialization
        void Start()
        {
            StartGameWaiting.SetActive(true);
            MainPanel.SetActive(false);
            ScorePanel.SetActive(false);
            InvokeRepeating("StartGameChecker", 0, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            if (!ShowUI)
            {
                StartGameWaiting.SetActive(false);
                MainPanel.SetActive(false);
                ScorePanel.SetActive(false);
                return;
            }

            if(StartGameFlag && ShowUI)
            {
                StartGameWaiting.SetActive(false);
                MainPanel.SetActive(true);
            }
            else
            {
                MainPanel.SetActive(false);
            }

            if(StartGameFlag && Input.GetKey(KeyCode.Tab))
            {
                ScorePanel.SetActive(true);
            }
            else
            {
                ScorePanel.SetActive(false);
            }

        }

        void OnGUI()
        {
            /*if(GUILayout.Button("Upload MatchData"))
            {
                GameObject.Find("GameCoreProcess").GetComponent<Logic_ResultUpload>().UploadResult();
            }*/
        }

        private void StartGameChecker()
        {
            GameObject player = GameObject.Find("LOCAL Player");
            if(player)
            {
                StartGameFlag = true;
                GameObject.Find("CursorLocker").GetComponent<InGameCursorLocker>().LockMouse = true;
                Debug.Log("Found Local Player...");
                CancelInvoke("StartGameChecker");
            }
        }
    }

}