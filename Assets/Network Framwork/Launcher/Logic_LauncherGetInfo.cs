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
    private Text[] Result;
    private WWW www;
    private WWWForm form;
    private string url_userinfo = "https://www.kroulisworld.com/programs/survive/ddt/get_info.php";
    private string url_matchinfo = "https://www.kroulisworld.com/programs/survive/ddt/get_recent_result.php";
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
                    GameObject ui_root=GameObject.Find("Launcher UI Root");
                    if(ui_root)
                    {
                        ui_root.GetComponent<UI_FunctionControl>().FinishWWWLoading();
                    }
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

    public void GetRecentMatch(Text[] Results)
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
                JsonData jd = JsonMapper.ToObject(result);
                string code = (string)jd["code"];
                if (code == "143930")
                {
                    
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


    public string GetCharacterNameA()
    {
        if (Name)
            return Name.text;
        else
            return processname;
    }
}
