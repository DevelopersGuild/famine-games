using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kroulis.Components;
using UnityEngine.Networking;

namespace Kroulis.UI.MainGame
{
    public class ScoreBoardUIControl : MonoBehaviour
    {

        public Text Names,Scores,Kills,Deaths,Specials;
        private Point[] players;
        private string localname;
        
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            players = GameObject.FindObjectsOfType<Point>();
           
            localname = GameObject.Find("LOCAL Player").GetComponent<ContestInfomation>().player_name;
            Names.text = Scores.text = Kills.text = Deaths.text = Specials.text = "";
            for(int i=0;i<players.Length;i++)
            {
                Names.text += players[i].GetComponent<ContestInfomation>().player_name+"\n";
                Scores.text += players[i].points+"\n";
                Kills.text += players[i].kills + "\n";
                Deaths.text += players[i].deaths + "\n";
                if(players[i].GetComponent<ContestInfomation>().player_name == localname)
                {
                    Specials.text += "<< You\n";
                }
                else
                {
                    Specials.text += "\n";
                }

            }
        }

        private string Format(string source, int size)
        {
            string result;
            if(source.Length>size)
            {
                result = source.Substring(0, size);
            }
            else
            {
                result = source;
                for(int i=source.Length;i<size;i++)
                {
                    result += " ";
                }
            }
            return result;
        }
    }
}