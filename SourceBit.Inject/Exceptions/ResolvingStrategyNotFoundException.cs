using System;

namespace SourceBit.Inject.Exceptions
{
    [Serializable]
    public class ResolvingStrategyNotFoundException : Exception
    {
        public ResolvingStrategyNotFoundException(string message)
            : base(message)
        {
        }
    }
}
