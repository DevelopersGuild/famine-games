using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using LitJson;

namespace Kroulis.UI.Process
{
    public class ResultPanelFullControl : MonoBehaviour
    {
        public Text Name, Score, Kill, Death, Memo, Log;
        private WWW result;
        private WWWForm resultform;
        private string url_getresult = "https://www.kroulisworld.com/programs/survive/ddt/get_match_info.php";
        private string url_getplayername = "https://www.kroulisworld.com/programs/survive/ddt/get_player_name.php";
        // Use this for initialization
        void Start()
        {
            GetResult();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void GetResult()
        {
            resultform = new WWWForm();
            resultform.AddField("mid", Globe.resultid);
            result = new WWW(url_getresult, resultform);
            StartCoroutine(WaitForResultData(result));
        }
        
        private IEnumerator WaitForResultData(WWW www)
        {
            yield return www;
            if(www.error != null)
            {
                //ERROR
                Globe.errorid = "EFATAL";
                GetComponentInParent<ProcessUIControl>().GoesToError();
            }
            if(www.isDone)
            {
                string returndata = www.text;
                JsonData data = JsonMapper.ToObject(returndata);
                if((string)data["code"]=="142355")
                {
                    string[] userset = ((string)data["userset"]).Split(new char[1]{','});
                    string[] scores = ((string)data["score"]).Split(new char[1] { ',' });
                    string[] kills = ((string)data["kill"]).Split(new char[1] { ',' });
                    string[] death = ((string)data["death"]).Split(new char[1] { ',' });
                    string logs = (string)data["log"];
                    Name.text = Score.text = Kill.text = Death.text = Memo.text = Log.text = "";
                    Log.text = logs;
                    for(int i=0;i<userset.Length;i++)
                    {
                        WWWForm newform = new WWWForm();
                        newform.AddField("uid", userset[i]);
                        WWW getnamewww = new WWW(url_getplayername, newform);
                        while (!getnamewww.isDone) ;
                        if(getnamewww.error!=null)
                        {
                            Globe.errorid = "EFATAL";
                            GetComponentInParent<ProcessUIControl>().GoesToError();
                        }
                        Name.text += getnamewww.text + "\n";
                        Score.text += scores[i] + "\n";
                        Kill.text += kills[i] + "\n";
                        Death.text += death[i] + "\n";
                        if(Globe.uid==userset[i])
                        {
                            Memo.text += "<<-YOU\n";
                        }
                        else
                        {
                            Memo.text += "\n";
                        }
                    }

                }
                else
                {
                    //ERROR
                    Globe.errorid = "EFATAL";
                    GetComponentInParent<ProcessUIControl>().GoesToError();
                }
            }
        }
    }
}