using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Login_LoginBtn : MonoBehaviour {

    public Text username;
    public InputField password;
    public Text tips;
    public Logic_Cilent_Login login;
	// Use this for initialization
	void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate() { this.Login(); });
	}
	
    void Login()
    {
        if(username.text=="" || password.text=="")
        {
            tips.color = Color.red;
            tips.text = "Please input your username or password.";
            return;
        }
        login.Login(username, password, tips);
    }
}
