using System;
using Assets.Adapter;
using UnityEngine;

namespace Assets.Manager
{
    public class DataManager : MonoBehaviour
    {
        public static string EnvironmentName = "";
        public static bool SaveData, AllVariablesSet;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (SaveData && AllVariablesSet)
                DataExporter.StartLog = true;

            CreateStringMessage();
        }


        /// <summary>
        /// Splits the incoming data and instantiates/updates variables
        /// </summary>
        /// <param name="message"></param>
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


        /// <summary>
        /// This method updates the existing variables classes - one method per environment is required
        /// </summary>
        /// <param name="name">variable name</param>
        /// <param name="value">variable value</param>
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


        /// <summary>
        /// Creates the string according to the existing values
        /// </summary>
        public static void CreateStringMessage()
        {
            if (EnvironmentName == "Demo")
            {
                var uv = Demo.SetEnvironment().UpdatableVariables;
                var rv = Demo.SetEnvironment().ReadableVariables;
                var sv = PhysioAdapter.Sensors;

                var message = "";

                for (var i = 0; i < uv.Count; i++)
                {
                    if(uv[i].Name.StartsWith("UB"))
                        message = uv[i].Name + "," + uv[i].Value;
                    else
                        message = uv[i].Name + "," + uv[i].Type + "," + uv[i].Value + "," + uv[i].Min + "," + uv[i].Max;
                    UDPSender.SendStringMessage(message);
                }

                for (var i = 0; i < rv.Count; i++)
                {
                    message = rv[i].Name + "," + rv[i].Value;
                    UDPSender.SendStringMessage(message);
                }

                for (var i = 0; i < sv.Count; i++)
                {
                    message = sv[i].Name + "," + sv[i].Value;
                    UDPSender.SendStringMessage(message);
                }
            }   
        }
    }
}
