using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class MatchPanalBGMovControl : MonoBehaviour
    {
        private bool movplay = false;
        private bool type = false; //false means loop in, true means loop out.
        private RawImage ri;
        private Texture[] t;
        private float timer;
        // Use this for initialization
        void Start()
        {
            ri = GetComponent<RawImage>();
            t = Resources.LoadAll<Texture>("MatchPanel");
            //Debug.Log(t.Length);
            timer = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (movplay)
            {
                timer = (timer + Time.deltaTime);
                if (!type)
                {
                    if (timer <= 1.5f)
                    {
                        ri.texture = t[(int)(timer * 30)];
                    }
                    else
                    {
                        movplay = false;
                        ri.texture = t[45];
                        timer = 0;
                        GetComponentInParent<UI_MatchesControl>().UI.SetActive(true);
                        GetComponentInParent<UI_MatchesControl>().UI.GetComponent<Match_Panel_Control>().CleanRoomInfo();
                    }

                }
                else
                {
                    if (timer <= 1.5f)
                    {
                        ri.texture = t[(int)(45 + timer * 30)];
                    }
                    else
                    {
                        movplay = false;
                        ri.texture = t[89];
                        timer = 0;
                        UI_FunctionControl root = GetComponentInParent<UI_FunctionControl>();
                        root.MatchPanel.SetActive(false);
                    }
                }
            }

        }

        public void StartLoopIn()
        {
            type = false;
            timer = 0;
            movplay = true;
        }

        public void StartLoopOut()
        {
            type = true;
            timer = 0;
            movplay = true;
        }
    }

}
