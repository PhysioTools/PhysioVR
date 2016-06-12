using System.Collections.Generic;
using UnityEngine;

namespace Assets.Adapter
{
    public class PhysioAdapter : MonoBehaviour
    {
        public static List<Sensor> Sensors = new List<Sensor>();
        public static bool SensorAdded;

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

        public void ProcessData(string environment)
        {
            
        }
    }
}
