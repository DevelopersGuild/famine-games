using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Kroulis.UI.Launcher
{
    public class Main_LogoutBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate() { this.Action(); });
        }
        void Action()
        {
            Debug.Log("Logout.");
            SceneManager.LoadScene("Login");
            Globe.uid = "";
            Globe.initeduser = false;
        }
    }
}

