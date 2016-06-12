using System;
using Assets.Adapter;
using UnityEngine;

namespace Assets.Manager
{
    public class DataManager : MonoBehaviour
    {
        public static string EnvironmentName = "";
        
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
                else if (words[i].StartsWith("UB_") || words[i].StartsWith("UR_"))
                {
                    UpdatableVariable(words[i], words[i + 1]);
                    
                }
            }
        }

        private static void UpdatableVariable(string name, string value)
        {
            if (EnvironmentName.Contains("Demo"))
            {
                var uv = Demo.SetEnvironment().UpdatableVariables;
                if (uv.Count > 0)
                {
                    for (var i = 0; i < uv.Count; i++)
                    {
                        if (uv[i].Name.Contains(name))
                        {
                            if (uv[i].Value != null && uv[i].Value != value)
                            {
                                uv[i].Value = value;
                                Demo.UpdateVariable(uv[i]);
                            }
                        }
                    }
                }
            }
        }

        public static void CreateStringMessage()
        {

        }
    }
}
