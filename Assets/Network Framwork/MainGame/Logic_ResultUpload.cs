using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using LitJson;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kroulis.Components;

public class Logic_ResultUpload : NetworkBehaviour {

    private static string uploadurl = "https://www.kroulisworld.com/programs/survive/ddt/upload_match_info.php";
    private static string uploadkey = "d9bcdb94477e41ae1f0c60c4ea3a77b9";
    private WWW www;
    private WWWForm form;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Server]
    public void UploadResult()
    {
        form = new WWWForm();
        form.AddField("code", uploadkey);
        //Create json
        Point[] player_list = GameObject.FindObjectsOfType<Point>();
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.WriteObjectStart();
            //userset
                string userset = "";
                for(int i=0;i<player_list.Length;i++)
                {
                    userset += player_list[i].GetComponent<ContestInfomation>().GetPid() + ",";
                }
                writer.WritePropertyName("userset");
                writer.Write(userset.Remove(userset.Length-1));
            //scores
                string scores = "";
                for (int i = 0; i < player_list.Length; i++)
                {
                    scores += player_list[i].points + ",";
                }
                writer.WritePropertyName("score");
                writer.Write(scores.Remove(scores.Length-1));
            //kills
                string kill = "";
                for(int i=0;i<player_list.Length;i++)
                {
                    kill += player_list[i].kills + ",";
                }
                writer.WritePropertyName("kill");
                writer.Write(kill.Remove(kill.Length - 1));
            //deaths
                string death = "";
                for (int i = 0; i < player_list.Length; i++)
                {
                    death += player_list[i].deaths + ",";
                }
                writer.WritePropertyName("death");
                writer.Write(death.Remove(death.Length - 1));
            //assists
                string assist = "";
                for (int i = 0; i < player_list.Length; i++)
                {
                    assist += "N/A,";
                }
                writer.WritePropertyName("assist");
                writer.Write(assist.Remove(assist.Length - 1));
            //log
                writer.WritePropertyName("log");
                writer.Write(GetComponent<GameProcess>().GetLog());
            //time
                writer.WritePropertyName("timeused");
                writer.Write(GetComponent<GameProcess>().timestamp.ToString());
        writer.WriteObjectEnd();
        form.AddField("data", sb.ToString());
        www = new WWW(uploadurl, form);
        StartCoroutine(WaitForRequestUploadData(www));
    }

    private IEnumerator WaitForRequestUploadData(WWW www)
    {
        yield return www;
        if (www.error != null)
        {
            Debug.Log("fail to request..." + www.error);
            RpcFailedUploading();
            NetworkServer.Shutdown();
        }
        else
        {
            if (www.isDone)
            {
                string result = www.text;
                JsonData jd = JsonMapper.ToObject(result);
                string code = (string)jd["code"];
                if (code == "149090")
                {
                    RpcSuccessfullyUploaded((string)jd["rid"]);
                    NetworkServer.Shutdown();
                }
                else if (code == "150999")
                {
                    Debug.LogError("UserNotExist.");
                    RpcFailedUploading();

                    NetworkServer.Shutdown();
                }
                else
                {
                    Debug.LogError("Cannot Connect to the Server.");
                    
                }
            }
        }
    }

    [ClientRpc]
    public void RpcFailedUploading()
    {
        Globe.errorid = "EFATAL";
        Globe.resultid = "";
    }

    [ClientRpc]
    public void RpcSuccessfullyUploaded(string rid)
    {
        Globe.resultid = rid;
        Globe.errorid = "0";
    }
}
