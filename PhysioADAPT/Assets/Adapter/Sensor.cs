namespace Assets.Adapter
{
    public class Sensor
    {
        public string Name;
        public string Type;
        public string Value;

        public Sensor(string name, string type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }
}
