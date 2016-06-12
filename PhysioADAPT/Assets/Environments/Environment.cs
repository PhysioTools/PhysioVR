using System;
using System.Collections.Generic;

namespace Assets.Environments
{
    public class Environment
    {
        public string Name;
        public List<UpdatableVar> UpdatableVariables = new List<UpdatableVar>(); 
        public List<ReadableVar> ReadableVariables = new List<ReadableVar>();
        
        public Environment(string name, List<UpdatableVar> upd, List<ReadableVar> read)
        {
            Name = name;
            UpdatableVariables = upd;
            ReadableVariables = read;
        }
    }
}
