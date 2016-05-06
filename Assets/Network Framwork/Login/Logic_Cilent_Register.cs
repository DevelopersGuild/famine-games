using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using LitJson;
using Kroulis.Security;

public class Logic_Cilent_Register : MonoBehaviour {

    private WWW www;
    private WWWForm form;
    private string url = "https://www.kroulisworld.com/programs/survive/ddt/register.php";
    private bool reg_succeed = false;
    private string reg_msg = "";
    private Text tips;
    public void Register(Text username, Text password, Text email ,Text name,Text tips)
    {
        this.tips = tips;
        string md5psw = Secure.MD5Encrypt(password.text);
        Debug.Log(md5psw);
        form = new WWWForm();
        form.AddField("user", username.text);
        form.AddField("password", md5psw);
        form.AddField("email", email.text);
        form.AddField("name", name.text);
        www = new WWW(url, form);
        StartCoroutine(WaitForRequestUserNameLogin(www));
    }

    private IEnumerator WaitForRequestUserNameLogin(WWW www)
    {
        yield return www;
        if (www.error != null)
        {
            Debug.Log("fail to request..." + www.error);
            reg_succeed = false;
            reg_msg = "Cannot Connect to the Server.";
            FinishRegister();
        }
        else
        {
            if (www.isDone)
            {
                string result = www.text;
                JsonData jd = JsonMapper.ToObject(result);
                string code = (string)jd["code"];
                if (code == "147980")
                {
                    reg_succeed = true;
                    reg_msg = (string)jd["MSG"];
                }
                else if (code == "150999")
                {
                    reg_succeed = false;
                    reg_msg = (string)jd["MSG"];
                }
                else
                {
                    reg_succeed = false;
                    reg_msg = "Cannot Connect to the Server.";
                }
                FinishRegister();
            }
        }
    }

    private void FinishRegister()
    {
        tips.text = reg_msg;
        if (reg_succeed)
        {
            Debug.Log("Register Success.");
            tips.color = Color.green;
        }
        else
        {
            Debug.Log("Register Failed.");
            tips.color = Color.red;
        }
    }
}
