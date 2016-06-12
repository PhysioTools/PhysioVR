namespace Assets.Environments
{
    public class RangeVar : UpdatableVar
    {
        public string Min;
        public string Max;

        public RangeVar(string name, string type, string value, string min, string max)
        {
            Name = name;
            Type = type;
            Value = value;
            Min = min;
            Max = max;
        }
    }
}
