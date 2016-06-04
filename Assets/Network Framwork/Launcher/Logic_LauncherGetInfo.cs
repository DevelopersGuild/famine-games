using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using LitJson;
using Kroulis.UI.Launcher;
using UnityEngine.SceneManagement;

public class Logic_LauncherGetInfo : MonoBehaviour {
    
    private Text Name;
    private Text level;
    private Text EXP_text;
    private Image EXP_image;
    private Text Gold;
    private Text Diamond;
    private RecentResult_Control Result;
    private WWW www;
    private WWWForm form;
    private string url_userinfo = "https://www.kroulisworld.com/programs/survive/ddt/get_info.php";
    private string url_matchinfo = "https://www.kroulisworld.com/programs/survive/ddt/get_recent_result.php";
    private string url_getusername = "https://www.kroulisworld.com/programs/survive/ddt/get_player_name.php";
    private string processname;
	public void GetInfo(Text name,Text level,Text EXP_text,Image EXP_image,Text Gold,Text Diamond)
    {
        this.Name = name;
        this.level = level;
        this.EXP_text = EXP_text;
        this.EXP_image = EXP_image;
        this.Gold = Gold;
        this.Diamond = Diamond;
        form = new WWWForm();
        form.AddField("uid", Globe.uid);
        if(!Globe.initeduser)
        {
            form.AddField("type", "init");
        }
        else
        {
            form.AddField("type", "update");
        }
        www = new WWW(url_userinfo, form);
        StartCoroutine(WaitForRequestUserInfo(www));
    }

    private IEnumerator WaitForRequestUserInfo(WWW www)
    {
        yield return www;
        if (www.error != null)
        {
            Debug.Log("fail to request..." + www.error);
            SceneManager.LoadScene("Login");
        }
        else
        {
            if (www.isDone)
            {
                string result = www.text;
                JsonData jd = JsonMapper.ToObject(result);
                string code = (string)jd["code"];
                if (code == "143930")
                {
                    Name.text = (string)jd["name"];
                    processname = Name.text;
                    level.text = GetLevelText((string)jd["level"]);
                    EXP_text.text = (string)jd["exp"] + @"/1000";
                    EXP_image.fillAmount = float.Parse((string)jd["exp"]) / 1000f;
                    Gold.text = (string)jd["gold"];
                    Diamond.text = (string)jd["diamond"];
                    /*GameObject ui_root=GameObject.Find("Launcher UI Root");
                    if(ui_root)
                    {
                        ui_root.GetComponent<UI_FunctionControl>().FinishWWWLoading();
                    }*/
                }
                else if (code == "150999")
                {
                    Debug.LogError("UserNotExist.");
                    SceneManager.LoadScene("Login");
                }
                else
                {
                    Debug.LogError("Cannot Connect to the Server.");
                    SceneManager.LoadScene("Login");
                }
            }
        }
    }

    private string GetLevelText(string level)
    {
        int levelc = int.Parse(level);
        switch(levelc)
        {
            case 1:
                return "New Player ☆";
            case 2:
                return "New Player ☆☆";
            case 3:
                return "New Player ☆☆☆";
            case 4:
                return "Inexperienced Player ☆";
            case 5:
                return "Inexperienced Player ☆☆";
            case 6:
                return "Inexperienced Player ☆☆☆";
            case 7:
                return "Experienced Player ☆";
            case 8:
                return "Experienced Player ☆☆";
            case 9:
                return "Experienced Player ☆☆☆";
            case 10:
                return "Experienced Player ☆☆☆☆";
            case 11:
                return "Good Player ☆";
            case 12:
                return "Good Player ☆☆";
            case 13:
                return "Good Player ☆☆☆";
            case 14:
                return "Good Player ☆☆☆☆";
            case 15:
                return "Good Player ☆☆☆☆☆";
            case 16:
                return "Master ☆";
            case 17:
                return "Master ☆☆";
            case 18:
                return "Master ☆☆☆";
            case 19:
                return "Master ☆☆☆☆";
            case 20:
                return "Master ☆☆☆☆☆";
            case 21:
                return "▷Master◁";
            default:
                return "Player";

        }
    }

    public void GetRecentMatch(RecentResult_Control Results)
    {
        this.Result = Results;
        form = new WWWForm();
        form.AddField("uid", Globe.uid);
        www = new WWW(url_matchinfo, form);
        StartCoroutine(WaitForRequestMatchData(www));
    }

    private IEnumerator WaitForRequestMatchData(WWW www)
    {
        yield return www;
        if (www.error != null)
        {
            Debug.Log("fail to request..." + www.error);
            SceneManager.LoadScene("Login");
        }
        else
        {
            if (www.isDone)
            {
                string result = www.text;
                //Debug.Log(www.text);
                JsonData jd = JsonMapper.ToObject(result);
                string code = (string)jd["code"];
                if (code == "148652")
                {
                    if(jd["data"].IsArray)
                    {
                        for(int i=0;i<jd["data"].Count;i++)
                        {
                            PlayerResultInfo pri=GetResultInfo(jd["data"][i]);
                            Result.MID[i].text = (string)jd["data"][i]["mid"];
                            Result.Result[i].text = pri.IsWinner ? "Victory" : "Defeat";
                            Result.Result[i].color = pri.IsWinner ? Color.green : Color.red;
                            //Result.PlayerList[i].text = "You with others";
                            Result.PlayerList[i].text = pri.nameset;
                            Result.Rewards[i].text = "Currently No Reward";
                            Result.Kill[i].text = pri.kill;
                            Result.Death[i].text = pri.death;
                            Result.Assist[i].text = "N/A";
                        }
                        for(int i=jd["data"].Count;i<16;i++)
                        {
                            Result.MID[i].text = "";
                            Result.Result[i].text = "";
                            Result.PlayerList[i].text = "";
                            Result.Rewards[i].text = "";
                            Result.Kill[i].text ="";
                            Result.Death[i].text = "";
                            Result.Assist[i].text = "";
                        }
                    }
                    else
                    {
                        PlayerResultInfo pri = GetResultInfo(jd["data"]);
                        Result.MID[0].text = (string)jd["data"]["mid"];
                        Result.Result[0].text = pri.IsWinner ? "Victory" : "Defeat";
                        Result.Result[0].color = pri.IsWinner ? Color.green : Color.red;
                        Result.PlayerList[0].text = "You with others";
                        Result.Rewards[0].text = "Currently No Reward";
                        Result.Kill[0].text = pri.kill;
                        Result.Death[0].text = pri.death;
                        Result.Assist[0].text = "N/A";
                        for(int i=1;i<16;i++)
                        {
                            Result.MID[i].text = "";
                            Result.Result[i].text = "";
                            Result.PlayerList[i].text = "";
                            Result.Rewards[i].text = "";
                            Result.Kill[i].text = "";
                            Result.Death[i].text = "";
                            Result.Assist[i].text = "";
                        }
                    }
                    GameObject ui_root = GameObject.Find("Launcher UI Root");
                    if (ui_root)
                    {
                        ui_root.GetComponent<UI_FunctionControl>().FinishWWWLoading();
                    }
                }
                else if (code == "150999")
                {
                    Debug.LogError("UserNotExist.");
                    SceneManager.LoadScene("Login");
                }
                else if(code=="150777")
                {
                    for (int i = 0; i < 16; i++)
                    {
                        Result.MID[i].text = "";
                        Result.Result[i].text = "";
                        Result.PlayerList[i].text = "";
                        Result.Rewards[i].text = "";
                        Result.Kill[i].text = "";
                        Result.Death[i].text = "";
                        Result.Assist[i].text = "";
                    }
                    GameObject ui_root = GameObject.Find("Launcher UI Root");
                    if (ui_root)
                    {
                        ui_root.GetComponent<UI_FunctionControl>().FinishWWWLoading();
                    }
                }
                else
                {
                    Debug.LogError("Cannot Connect to the Server.");
                    SceneManager.LoadScene("Login");
                }
            }
        }
    }

    struct PlayerResultInfo
    {
        public string nameset;
        public string score;
        public string kill;
        public string assist;
        public string death;
        public bool IsWinner;
    }

    private PlayerResultInfo GetResultInfo(JsonData jd)
    {
        PlayerResultInfo pri=new PlayerResultInfo();
        int count=-1;
        string userlist = (string)jd["userset"];
        string[] userlist_u = userlist.Split(new char[1]{','});
        for (int i = 0; i < userlist_u.Length && count==-1;i++)
        {
            if(userlist_u[i]==Globe.uid)
            {
                count = i;
            }
        }
        string[] score_list = ((string)jd["score"]).Split(new char[1] { ',' });
        string[] kill_list = ((string)jd["kill"]).Split(new char[1] { ',' });
        string[] death_list = ((string)jd["death"]).Split(new char[1] { ',' });
        string nameset = "";
        int maxscore=0;
        for (int i = 0; i < score_list.Length;i++)
        {
            if(i==count)
            {
                pri.score = score_list[i];
                pri.kill = kill_list[i];
                pri.death = death_list[i];
            }
            if(int.Parse(score_list[i])>maxscore)
            {
                maxscore = int.Parse(score_list[i]);
            }
            WWWForm wf = new WWWForm();
            wf.AddField("uid", userlist_u[i]);
            WWW unq = new WWW(url_getusername, wf);
            while (!unq.isDone) ;
            //Debug.Log(unq.text);
            if(unq.error!=null)
            {
                nameset += "Unknown";

                if(i!=score_list.Length-1)
                {
                    nameset += ",";
                }
            }
            else
            {
                nameset += unq.text;
                if (i != score_list.Length - 1)
                {
                    nameset += ",";
                }
            }
        }
        if(pri.score==maxscore.ToString())
        {
            pri.IsWinner = true;
        }
        else
        {
            pri.IsWinner = false;
        }
        pri.nameset = nameset;
        return pri;
    }

    public string GetCharacterNameA()
    {
        if (Name)
            return Name.text;
        else
            return processname;
    }
}
