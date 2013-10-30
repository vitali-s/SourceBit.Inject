using System;
using System.Collections.Generic;

namespace SourceBit.Inject
{
    public class TypeDetails
    {
        public TypeDetails()
        {
            Dependencies = new List<Type>();
        }

        public Type Type { get; set; }

        public int StrategyType { get; set; }

        public List<Type> Dependencies { get; set; }

        public Func<object> Instantiator { get; set; }
    }
}
