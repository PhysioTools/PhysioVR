using System.Collections.Generic;
using Assets.Manager;
using UnityEngine;

namespace Assets.Adapter
{
    public class PhysioAdapter : MonoBehaviour
    {
        public static List<Sensor> Sensors = new List<Sensor>();
        public static bool SensorAdded;
        
        private void Update()
        {
            ProcessData(DataManager.EnvironmentName);
        }

        public static void SetSensor(string name, string type, string value)
        {
            Debug.Log("received in SetSensor: " + name + type + value);
            Debug.Log(Sensors.Count);
            if (Sensors.Count > 0)
            {
                for (var i = 0; i < Sensors.Count; i++)
                {
                    if (Sensors[i].Name == name)
                    {
                        var s = Sensors[i].Value;
                        if(s != null && s != value)
                            Sensors[i].Value = value;
                        SensorAdded = true;
                        Debug.Log("Sensor Updated");
                    }
                }
                if (!SensorAdded)
                {
                    var newSensor = new Sensor(name, type, value);
                    Sensors.Add(newSensor);
                    Debug.Log("Sensor Added");
                }
            }
            else
            {
                Debug.Log("this is a new variable");
                var newSensor = new Sensor(name, type, value);
                Sensors.Add(newSensor);
            }
        }

        private int _previousHR;

        public void ProcessData(string environment)
        {
            if (environment == "Demo")
            {
                var uv = Demo.SetEnvironment().UpdatableVariables;
                for (var i = 0; i < Sensors.Count; i++)
                {
                    if (Sensors[i].Type == "HR")
                    {
                        for (var b = 0; b < uv.Count; b++)
                        {
                            if (uv[b].Name.Contains("SphereHeight"))
                            {
                                if (int.Parse(Sensors[i].Value) >= _previousHR)
                                {
                                    uv[b].Value = (float.Parse(uv[b].Value) + 0.01f).ToString("0.00");
                                }
                                else
                                {
                                    uv[b].Value = (float.Parse(uv[b].Value) - 0.01f).ToString("0.00");
                                }
                                Demo.UpdateVariable(uv[b]);
                            }
                        }   
                    }
                }
            }
        }
    }
}
