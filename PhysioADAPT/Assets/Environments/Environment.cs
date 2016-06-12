using System.Collections.Generic;

namespace Assets.Environments
{
    public class Environment {

        public List<UpdatableVar> UpdatableVariables = new List<UpdatableVar>(); 
        public List<ReadableVar> ReadableVariables = new List<ReadableVar>();
        
        public Environment(List<UpdatableVar> upd, List<ReadableVar> read)
        {
            UpdatableVariables = upd;
            ReadableVariables = read;
        }
    }
}
