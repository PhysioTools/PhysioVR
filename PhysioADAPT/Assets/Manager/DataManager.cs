using System;
using Assets.Adapter;
using UnityEngine;

namespace Assets.Manager
{
    public class DataManager : MonoBehaviour {

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static void ParseData(string message)
        {
            string[] separators = { ",", ";", "|"};

            string[] words = message.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < words.Length; i++)
            {
                if (words[i].StartsWith("S_"))
                {
                    PhysioAdapter.SetSensor(words[i], words[i+1], words[i+2]);
                }
                else if (words[i].StartsWith("UB_"))
                {
                    //updates updatable boolean variable
                }
                else if (words[i].StartsWith("UR_"))
                {
                    //updates range variable
                }
            }
        }

        public static void CreateStringMessage()
        {

        }
    }
}
