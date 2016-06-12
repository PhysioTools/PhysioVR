namespace Assets.Environments
{
    public class ReadableVar : Variable {

        public ReadableVar(string name, string type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }
}
