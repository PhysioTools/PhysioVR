using System.Collections.Generic;
using UnityEngine;

namespace Assets.Adapter
{
    public class PhysioAdapter : MonoBehaviour
    {
        public static List<Sensor> Sensors;
        public static bool SensorAdded;

        public static void SetSensor(string name, string type, string value)
        {
            if (Sensors.Count > 0)
            {
                for (var i = 0; i < Sensors.Count; i++)
                {
                    if (Sensors[i].Name == name)
                    {
                        Sensors[i].Value = value;
                        SensorAdded = true;
                    }
                }
                if (!SensorAdded)
                {
                    var newSensor = new Sensor(name, type, value);
                    Sensors.Add(newSensor);
                }
            }
            else
            {
                var newSensor = new Sensor(name, type, value);
                Sensors.Add(newSensor);
            }
        }

        public void ProcessData(string environment)
        {
            
        }
    }
}
