using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using LitJson;
using Kroulis.Security;
using UnityEngine.SceneManagement;

public class Logic_Cilent_Login : MonoBehaviour {

    private WWW www;
    private WWWForm form;
    private string url = "https://www.kroulisworld.com/programs/survive/ddt/login.php";
    private bool login_succeed = false;
    private string login_msg = "";
    private string uid = "";
    private Text tips;
    private InputField password;
	public void Login(Text username, InputField password, Text tips)
    {
        this.password = password;
        this.tips = tips;
        string md5psw = Secure.MD5Encrypt(password.text);
        Debug.Log(md5psw);
        form=new WWWForm();
        form.AddField("user", username.text);
        form.AddField("password", md5psw);
        www = new WWW(url,form);
        StartCoroutine(WaitForRequestUserNameLogin(www));
    }

    private IEnumerator WaitForRequestUserNameLogin(WWW www)
    {
        yield return www;
        if (www.error != null)
        {
            Debug.Log("fail to request..." + www.error);
            login_succeed = false;
            login_msg = "Cannot Connect to the Server.";
            uid = "";
            FinishLogin();
        }    
        else
        {
            if (www.isDone)
            {
                string result = www.text;
                JsonData jd = JsonMapper.ToObject(result);
                string code = (string)jd["code"];
                if(code=="143950")
                {
                    login_succeed = true;
                    login_msg = "Login Succeed. Please wait...";
                    uid=(string)jd["result"];
                }
                else if(code=="150999")
                {
                    login_succeed = false;
                    login_msg = (string)jd["MSG"];
                    uid = "";
                }
                else
                {
                    login_succeed = false;
                    login_msg = "Cannot Connect to the Server.";
                    uid = "";
                }
                FinishLogin();
            }
        }
    }

    private void FinishLogin()
    {
        tips.text = login_msg;
        if(login_succeed)
        {
            Debug.Log("Login Success.");
            tips.color = Color.green;
            Globe.uid = uid;
            SceneManager.LoadScene("Launcher");
        }
        else
        {
            Debug.Log("Login Failed.");
            tips.color = Color.red;
            password.text = "";
        }
    }
}
