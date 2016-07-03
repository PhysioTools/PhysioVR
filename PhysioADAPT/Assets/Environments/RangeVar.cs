namespace Assets.Environments
{
    public class RangeVar : UpdatableVar
    {
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
